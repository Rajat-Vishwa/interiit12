using System.Threading.Tasks;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public GameObject mainPlane;
    private float rotateSpeed;
    private float scrollSpeed;
    private Color[] pixels;
    private int textureWidth;
    private int textureHeight;

    public void Awake()
    {
        Texture2D texture = mainPlane.GetComponent<Renderer>().material.GetTexture("_AlphaTexture") as Texture2D;
        pixels = texture.GetPixels();
        textureWidth = texture.width;
        textureHeight = texture.height;
    }

    private void Update()
    {
        rotateSpeed = LevelManager.instance.rotateSpeed;
        scrollSpeed = LevelManager.instance.obstacleSpeed;
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public async void Slice(Vector2 startUV, Vector2 endUV)
    {
        Debug.Log("Received UV Coordinates " + startUV + "; " + endUV);

        // Convert UV coordinates to pixel coordinates
        int startX = (int)(startUV.x * textureWidth);
        int startY = (int)(startUV.y * textureHeight);
        int endX = (int)(endUV.x * textureWidth);
        int endY = (int)(endUV.y * textureHeight);

        // Start the slicing operation on a separate thread
        Color[] newPixels = await Task.Run(() => Symmetrize(pixels, textureWidth, textureHeight, startX, startY, endX, endY));

        // Apply the results on the main thread
        Texture2D newTexture = new Texture2D(textureWidth, textureHeight);
        newTexture.SetPixels(newPixels);
        newTexture.Apply();

        mainPlane.GetComponent<Renderer>().material.SetTexture("_MirrorTexture", newTexture);
    }

    private Color[] Symmetrize(Color[] pixels, int width, int height, int startX, int startY, int endX, int endY)
    {
        // Calculate the line's normal vector
        Vector2 lineDir = new Vector2(endX - startX, endY - startY).normalized;
        Vector2 lineNormal = new Vector2(-lineDir.y, lineDir.x);

        // Create a new array to store the mirrored pixels all white
        Color[] newPixels = new Color[pixels.Length];
        for (int i = 0; i < newPixels.Length; i++){
            newPixels[i] = Color.white;
        }

        // Iterate over each pixel in the texture
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate the pixel's position relative to the start of the line
                Vector2 relPos = new Vector2(x - startX, y - startY);

                // Calculate the distance from the pixel to the line
                float distToLine = Vector2.Dot(relPos, lineNormal);

                // Calculate the position of the mirrored pixel
                Vector2 mirroredPos = relPos - 2 * distToLine * lineNormal;

                int mirroredX = (int)(mirroredPos.x + startX);
                int mirroredY = (int)(mirroredPos.y + startY);

                if (mirroredX > 0 && mirroredX < width - 1 && mirroredY > 0 && mirroredY < height - 1)
                {
                    int index = mirroredY * width + mirroredX;
                    Color pixelColor = pixels[y * width + x];

                    newPixels[index] = pixelColor;

                    // Set neighbouring 8 pixels to the same color
                    newPixels[index + 1] = pixelColor;
                    newPixels[index - 1] = pixelColor;
                    newPixels[index + width] = pixelColor;
                    newPixels[index - width] = pixelColor;
                    newPixels[index + width + 1] = pixelColor;
                    newPixels[index - width - 1] = pixelColor;
                    newPixels[index + width - 1] = pixelColor;
                    newPixels[index - width + 1] = pixelColor;
                }
            }
        }

        return newPixels;
    }
}