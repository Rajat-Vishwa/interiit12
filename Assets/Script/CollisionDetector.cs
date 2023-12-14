using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public float minSqrDist = 0.1f;
    public Transform bl, br, tl, tr; // bottom left, bottom right, top left, top right
    public Vector2 blUV, trUV;
    public LayerMask slicable;
    public GameObject currentObstacle;
    public bool hasHit = false;
    public bool hasCheckedCollision = false;

    void Start()
    {
        slicable = LayerMask.GetMask("Slicable");
        hasCheckedCollision = false;
        hasHit = false;
    }


    void Update()
    {
        currentObstacle = LevelManager.instance.obstacles[0];

        float sqrDist = Vector3.SqrMagnitude(transform.position - currentObstacle.transform.position);
        Debug.Log("Distance: " + sqrDist);

        if (sqrDist <= minSqrDist && !hasCheckedCollision){
            CheckCollision();
            hasCheckedCollision = true;
        }
    }

    public void CheckCollision()
    {
        // Shoot rays from each corner in the forward direction and check if they hit the obstacle and get the UV coordinates
        RaycastHit hit;
        if (Physics.Raycast(bl.position, transform.forward, out hit, 100f, slicable)){
            blUV = hit.textureCoord;
        }
        if(Physics.Raycast(tr.position, transform.forward, out hit, 100f, slicable)){
            trUV = hit.textureCoord;
        }

        // If any of the UV coordinates are not set return from the function
        if(blUV == Vector2.zero ||  trUV == Vector2.zero){
            hasCheckedCollision = false;
            return;
        }

        // Recalculate UV Coordinates for drawing plane scale 2 times main plane
        blUV = RecalculateUV(blUV);
        trUV = RecalculateUV(trUV);

        Debug.Log("UV Coordinates " + blUV + "; " + trUV);

        // Get both textures from the obstacle
        Texture2D alphaTex = currentObstacle.GetComponentInChildren<MeshRenderer>().material.GetTexture("_AlphaTexture") as Texture2D;
        Texture2D mirrorTex = currentObstacle.GetComponentInChildren<MeshRenderer>().material.GetTexture("_MirrorTexture") as Texture2D;

        // Get the pixel coordinates of the UV coordinates
        Vector2Int blPixel = new Vector2Int((int)(blUV.x * alphaTex.width), (int)(blUV.y * alphaTex.height));
        Vector2Int trPixel = new Vector2Int((int)(trUV.x * alphaTex.width), (int)(trUV.y * alphaTex.height));

        bool inMirror = true;
        bool inAlpha = false;

        for(int i = trPixel.x+1; i < blPixel.x-1; i++){
            for(int j = trPixel.y+1; j < blPixel.y-1; j++){

                if(alphaTex != null)
                    inAlpha = (alphaTex.GetPixel(i, j) == Color.white && 
                                    alphaTex.GetPixel(i+1, j) == Color.white && 
                                    alphaTex.GetPixel(i-1, j) == Color.white && 
                                    alphaTex.GetPixel(i, j+1) == Color.white && 
                                    alphaTex.GetPixel(i, j-1) == Color.white && 
                                    alphaTex.GetPixel(i+1, j+1) == Color.white &&
                                    alphaTex.GetPixel(i-1, j-1) == Color.white &&
                                    alphaTex.GetPixel(i+1, j-1) == Color.white && 
                                    alphaTex.GetPixel(i-1, j+1) == Color.white);

                if(mirrorTex != null)
                    inMirror = (mirrorTex.GetPixel(i, j) == Color.white && 
                                    mirrorTex.GetPixel(i+1, j) == Color.white && 
                                    mirrorTex.GetPixel(i-1, j) == Color.white && 
                                    mirrorTex.GetPixel(i, j+1) == Color.white && 
                                    mirrorTex.GetPixel(i, j-1) == Color.white && 
                                    mirrorTex.GetPixel(i+1, j+1) == Color.white &&
                                    mirrorTex.GetPixel(i-1, j-1) == Color.white &&
                                    mirrorTex.GetPixel(i+1, j-1) == Color.white && 
                                    mirrorTex.GetPixel(i-1, j+1) == Color.white);
                
                // set hasHit to true if all 9 pixels are white
                if (inAlpha && inMirror){
                    Debug.Log("Hit");
                    hasHit = true;
                    break;
                }

            }
            if(hasHit){
                break;
            }
        } 

        
    }

    private Vector2 RecalculateUV(Vector2 uv)
    {
        return (uv - Vector2.one * 0.25f) * 2f;
    }
}
