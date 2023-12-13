using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Vector3 spawnPos = new Vector3(0f, 0f, 0f);
    public Vector3 endPos = new Vector3(0f, 0f, 0f);
    public float obstacleSpeed = 6.4f;
    public GameObject ObstaclePrefab;
    public Transform player;
    public Transform MirrorMount;


    public List<GameObject> obstacles = new();
    public int maxObstacles = 2;
    public float spawnCooldown = 1f;
    private float spawnTimer = 0f;

    public static LevelManager instance;

    void Start()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(obstacles[0].transform.localPosition.z <= endPos.z){
            obstacles[0].transform.localPosition = spawnPos;

            obstacles[0].GetComponentInChildren<Renderer>().material.SetTexture("_MirrorTexture", Texture2D.whiteTexture);

            // Move the obstacle to the end of the list
            obstacles.Add(obstacles[0]);
            obstacles.RemoveAt(0);

            MirrorMount.parent = LevelManager.instance.obstacles[0].transform;
            MirrorMount.localPosition = Vector3.zero;

        }

    }


}
