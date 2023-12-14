using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObstacleBehaviour : MonoBehaviour
{
    private float scrollSpeed = 0.8f;
    public GameObject mainPlane;

    void Start()
    {
        scrollSpeed = LevelManager.instance.obstacleSpeed;
    }

    void Update()
    {
        scrollSpeed = LevelManager.instance.obstacleSpeed;
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }

    public void Slice(Vector2 startUV, Vector2 endUV)
    {
        Debug.Log("Received UV Coordinates " + startUV + "; " + endUV);
        Texture2D texture = mainPlane.GetComponent<Renderer>().material.GetTexture("_AlphaTexture") as Texture2D;
        StartCoroutine(Symmetrize(texture, startUV, endUV));
    }

    IEnumerator Symmetrize(Texture2D texture, Vector2 startUV, Vector2 endUV)
    {
        // Convert UV coordinates to pixel coordinates
        int startX = (int)(startUV.x * texture.width);
        int startY = (int)(startUV.y * texture.height);
        int endX = (int)(endUV.x * texture.width);
        int endY = (int)(endUV.y * texture.height);

        // Calculate the line's normal vector
        Vector2 lineDir = new Vector2(endX - startX, endY - startY).normalized;
        Vector2 lineNormal = new Vector2(-lineDir.y, lineDir.x);

        Texture2D newTexture = new Texture2D(texture.width, texture.height);

        // Iterate over each pixel in the texture
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                // Calculate the pixel's position relative to the start of the line
                Vector2 relPos = new Vector2(x - startX, y - startY);

                // Calculate the distance from the pixel to the line
                float distToLine = Vector2.Dot(relPos, lineNormal);

                // Calculate the position of the mirrored pixel
                Vector2 mirroredPos = relPos - 2 * distToLine * lineNormal;

                int mirroredX = (int)(mirroredPos.x + startX);
                int mirroredY = (int)(mirroredPos.y + startY);

                
                if (mirroredX > 0 && mirroredX < texture.width-1 && mirroredY > 0 && mirroredY < texture.height-1){
                    newTexture.SetPixel(mirroredX, mirroredY, texture.GetPixel(x, y));
                    
                    // Set neighbouring 8 pixels to the same color
                    newTexture.SetPixel(mirroredX+1, mirroredY, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX-1, mirroredY, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX, mirroredY+1, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX, mirroredY-1, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX+1, mirroredY+1, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX-1, mirroredY-1, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX+1, mirroredY-1, texture.GetPixel(x, y));
                    newTexture.SetPixel(mirroredX-1, mirroredY+1, texture.GetPixel(x, y));
                    
                }
            }
            if(y % 10 == 0)
                yield return null;
        }
        

        newTexture.Apply();

        // Save the new texture on to textures folder (just for debugging)
        // byte[] bytes = newTexture.EncodeToPNG();
        // File.WriteAllBytes(Application.dataPath + "/Textures/texture.png", bytes);

        //mainPlane.GetComponent<Renderer>().material.SetTexture("_AlphaTexture", newTexture);
        mainPlane.GetComponent<Renderer>().material.SetTexture("_MirrorTexture", newTexture);
    }

    

}
