using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Vector3 spawnPos = new Vector3(0f, 0f, 0f);
    public Vector3 endPos = new Vector3(0f, 0f, 0f);
    public float obstacleSpeed = 6.4f;
    public float rotateSpeed = 0f;
    public float rotateSpeedIncrement = 0.5f;
    public float targetRotateSpeed = 0f;
    public float targetObstacleSpeed = 0f;

    [Space]
    public GameObject ObstaclePrefab;
    public Transform player;
    public Transform MirrorMount;
    public  TMP_Text ScoreText;

    [Space]
    public List<GameObject> obstacles = new();
    public int maxObstacles = 2;
    
    [Space]
    public int difficulty = 1;
    public float difficultySpeedIncrement = 0.5f;
    public int currentLevel = 0;
    public Texture2D[] levelTextures;

    public static LevelManager instance;

    [Space]
    public int Score = 0;
    public int ScoreIncrement = 100;
    public bool gameOver = false;

    void Start()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        gameOver = false;   
        player = GameObject.Find("Player").transform;

        targetObstacleSpeed = obstacleSpeed;
        targetRotateSpeed = rotateSpeed;
        difficultySpeedIncrement = obstacleSpeed / 4f;
    }

    void Update()
    {
        ScoreText.text = "Score : " + Score.ToString();

        if(obstacleSpeed < targetObstacleSpeed){
            obstacleSpeed += Time.deltaTime * 2f;
        }
        else if(obstacleSpeed > targetObstacleSpeed){
            obstacleSpeed = targetObstacleSpeed;
        }
        
        // Manage the obstacles
        if(obstacles[0].transform.localPosition.z <= endPos.z){

            currentLevel++;

            if(currentLevel % 5 == 0){
                difficulty++;
                if(difficulty <= 5){
                    targetObstacleSpeed += difficultySpeedIncrement;
                }
            }

            if(difficulty % 5 == 0){
                rotateSpeed += rotateSpeedIncrement;
            }

            int randLevel = Random.Range(0, levelTextures.Length);

            obstacles[0].transform.localPosition = spawnPos;
            obstacles[0].GetComponentInChildren<Renderer>().material.SetTexture("_AlphaTexture", levelTextures[randLevel]);
            obstacles[0].GetComponentInChildren<Renderer>().material.SetTexture("_MirrorTexture", Texture2D.whiteTexture);

            obstacles[0].GetComponentInChildren<Renderer>().material.SetFloat("_GlowEnabled", 0f);

            // Move the obstacle to the end of the list
            obstacles.Add(obstacles[0]);
            obstacles.RemoveAt(0);
            obstacles[0].GetComponentInChildren<Renderer>().material.SetFloat("_GlowEnabled", 1f);

            player.GetComponent<CollisionDetector>().hasCheckedCollision = false;
            player.GetComponent<CollisionDetector>().hasHit = false;

            MirrorMount.parent = obstacles[0].transform;
            MirrorMount.localPosition = Vector3.zero;

        }

    }


}
