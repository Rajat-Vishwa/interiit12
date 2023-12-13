using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    public bool animateShader = true;
    public GameObject[] obstacles;
    public GameObject platform;
    public Transform MirrorMount;

    private Material tunnelMaterial, platformMaterial;
    void Start()
    {
        LevelManager.instance.obstacles = new List<GameObject>(obstacles);
        tunnelMaterial = GetComponent<Renderer>().material;
        platformMaterial = platform.GetComponent<Renderer>().material;
    }

    void Update()
    {
        UpdateShader();
        MirrorMount.parent = LevelManager.instance.obstacles[0].transform;
        MirrorMount.localPosition = Vector3.zero;
    }

    public void UpdateShader()
    {
        float scrollSpeed = LevelManager.instance.obstacleSpeed;
        
        Vector2 temp = tunnelMaterial.GetVector("_noiseSpeed");
        temp.y = scrollSpeed / 80f; // IDK why but have to divide by 80
        tunnelMaterial.SetVector("_noiseSpeed", temp);

        temp = platformMaterial.GetVector("_scrollSpeed");
        temp.y = scrollSpeed / 80f; 
        platformMaterial.SetVector("_scrollSpeed", temp);
    }
}
