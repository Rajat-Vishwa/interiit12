using System.Threading.Tasks;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public Transform bl;
    public Transform tr;
    public LayerMask slicable;
    private Vector2 blUV;
    private Vector2 trUV;
    public bool hasCheckedCollision;
    public bool hasHit;
    private GameObject currentObstacle;
    public float minDist;

    public Texture2D alphaTex, mirrorTex;

    void Start()
    {
        slicable = LayerMask.GetMask("Slicable");
        hasCheckedCollision = false;
        hasHit = false;
    }

    private void FixedUpdate()
    {
        currentObstacle = LevelManager.instance.obstacles[0];
        float dist = currentObstacle.transform.position.z - transform.position.z;
        Debug.Log("Distance: " + dist);

        if (dist <= minDist + LevelManager.instance.obstacleSpeed / 12.8f && !hasCheckedCollision)
        {
            CheckCollisionAsync();
            hasCheckedCollision = true;

            if (!hasHit)
            {
                LevelManager.instance.Score += LevelManager.instance.ScoreIncrement;
            }
        }
    }

    private async void CheckCollisionAsync()
    {
        // Shoot rays from each corner in the forward direction and check if they hit the obstacle and get the UV coordinates
        RaycastHit hit;
        if (Physics.Raycast(bl.position, bl.forward, out hit, 100f, slicable))
        {
            blUV = hit.textureCoord;
        }
        if (Physics.Raycast(tr.position, tr.forward, out hit, 100f, slicable))
        {
            trUV = hit.textureCoord;
        }

        // If any of the UV coordinates are not set return from the function
        if (blUV == Vector2.zero || trUV == Vector2.zero)
        {
            hasCheckedCollision = false;
            return;
        }

        // Recalculate UV Coordinates for drawing plane scale 2 times main plane
        blUV = RecalculateUV(blUV);
        trUV = RecalculateUV(trUV);

        Debug.Log("Collision UV Coordinates " + blUV + "; " + trUV);

        // Get both textures from the obstacle
        alphaTex = currentObstacle.GetComponentInChildren<MeshRenderer>().material.GetTexture("_AlphaTexture") as Texture2D;
        mirrorTex = currentObstacle.GetComponentInChildren<MeshRenderer>().material.GetTexture("_MirrorTexture") as Texture2D;

        // Get texture properties on the main thread
        int alphaTexWidth = alphaTex.width;
        int alphaTexHeight = alphaTex.height;
        Color[] alphaTexPixels = alphaTex.GetPixels();

        Color[] mirrorTexPixels = mirrorTex == null ? null : mirrorTex.GetPixels();

        // Check for collision
        hasHit = await Task.Run(() => CheckForHit(alphaTexPixels, mirrorTexPixels, alphaTexWidth, alphaTexHeight, blUV, trUV));

        if (hasHit)
        {
            GameOverSequence();
        }
    }

    private Vector2 RecalculateUV(Vector2 uv)
    {
        return (uv - Vector2.one * 0.25f) * 2f;
    }

    private bool CheckForHit(Color[] alphaTexPixels, Color[] mirrorTexPixels, int texWidth, int texHeight, Vector2 blUV, Vector2 trUV)
    {
        Vector2Int blPixel = new Vector2Int((int)(blUV.x * texWidth), (int)(blUV.y * texHeight));
        Vector2Int trPixel = new Vector2Int((int)(trUV.x * texWidth), (int)(trUV.y * texHeight));

        bool inMirror = true;
        bool inAlpha = false;
        for (int j = trPixel.y + 1; j < blPixel.y - 1; j++)
        {
            for (int i = trPixel.x + 1; i < blPixel.x - 1; i++)
            {
                int pixelIndex = i + j * texWidth;

                inAlpha = (alphaTexPixels[pixelIndex] == Color.white &&
                            alphaTexPixels[pixelIndex + 1] == Color.white &&
                            alphaTexPixels[pixelIndex - 1] == Color.white &&
                            alphaTexPixels[pixelIndex + texWidth] == Color.white &&
                            alphaTexPixels[pixelIndex - texWidth] == Color.white &&
                            alphaTexPixels[pixelIndex + texWidth + 1] == Color.white &&
                            alphaTexPixels[pixelIndex - texWidth - 1] == Color.white &&
                            alphaTexPixels[pixelIndex + texWidth - 1] == Color.white &&
                            alphaTexPixels[pixelIndex - texWidth + 1] == Color.white);

                if (mirrorTexPixels != null)
                {
                    try{
                        inMirror = (mirrorTexPixels[pixelIndex] == Color.white &&
                                    mirrorTexPixels[pixelIndex + 1] == Color.white &&
                                    mirrorTexPixels[pixelIndex - 1] == Color.white &&
                                    mirrorTexPixels[pixelIndex + texWidth] == Color.white &&
                                    mirrorTexPixels[pixelIndex - texWidth] == Color.white &&
                                    mirrorTexPixels[pixelIndex + texWidth + 1] == Color.white &&
                                    mirrorTexPixels[pixelIndex - texWidth - 1] == Color.white &&
                                    mirrorTexPixels[pixelIndex + texWidth - 1] == Color.white &&
                                    mirrorTexPixels[pixelIndex - texWidth + 1] == Color.white);
                    }catch(System.IndexOutOfRangeException e){
                        Debug.Log("Index out of range");
                    }
                }

                if (inAlpha && inMirror)
                {
                    Debug.Log("Hit");
                    return true;
                }
            }
        }

        return false;
    }

    private void GameOverSequence()
    {
        Debug.Log("Game Over");
        LevelManager.instance.gameOver = true;
        LevelManager.instance.ScoreText.gameObject.SetActive(false);
        LevelManager.instance.gameObject.GetComponent<GameOverHandler>().GameOver();
    }

}