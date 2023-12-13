using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int highScore = 0;
    public TextMeshProUGUI scoreText;

    public bool IsDead;
    public GameObject endMenu;
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScoreManager");
                instance = go.AddComponent<ScoreManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = 0;
        IsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        highScore = Mathf.FloorToInt(Time.time * 50);
        scoreText.text = "SCORE: " + highScore.ToString();
    }
}
