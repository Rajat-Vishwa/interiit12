using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSymmetrizer : MonoBehaviour
{
    public Texture2D texture;

    public void Symmetrize(Vector2 startUV, Vector2 endUV)
    {
        // Convert UV coordinates to pixel coordinates
        int startX = (int)(startUV.x * texture.width);
        int startY = (int)(startUV.y * texture.height);
        int endX = (int)(endUV.x * texture.width);
        int endY = (int)(endUV.y * texture.height);

        // Calculate the line's normal vector
        Vector2 lineDir = new Vector2(endX - startX, endY - startY).normalized;
        Vector2 lineNormal = new Vector2(-lineDir.y, lineDir.x);

        // Create a new texture to store the symmetrized image
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

                // Convert the mirrored position back to absolute pixel coordinates
                int mirroredX = (int)(mirroredPos.x + startX);
                int mirroredY = (int)(mirroredPos.y + startY);

                // If the mirrored position is within the texture, set the color of the mirrored pixel to the color of the original pixel
                if (mirroredX >= 0 && mirroredX < texture.width && mirroredY >= 0 && mirroredY < texture.height)
                {
                    newTexture.SetPixel(mirroredX, mirroredY, texture.GetPixel(x, y));
                }
            }
        }

        // Apply the changes to the new texture
        newTexture.Apply();

        // Replace the original texture with the new texture
        texture = newTexture;
    }
}
