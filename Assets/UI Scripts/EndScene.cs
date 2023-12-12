using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class EndScene : MonoBehaviour
{
    public TMP_InputField nameInput;
    [SerializeField]GameObject scorecard;
    public GameObject endMenu;

    public  TMP_Text ScoreText;

    public void Update(){
        ScoreText.text=ScoreManager.highScore.ToString();
    }

    public void SaveName()
    {
        int pscore = ScoreManager.highScore;
        string playerName = nameInput.text.Trim();
        const int maxNameLength = 6;
        if (playerName.Length > maxNameLength)
        {
            playerName = playerName.Substring(0, Math.Min(maxNameLength, 6)) + "...";
        }
        HighscoreTable.AddHighscoreEntry(pscore, playerName);
    }
    public void Replay()
    {
        Time.timeScale=1f;
        endMenu.SetActive(false);
        SceneManager.LoadScene("WorldGeneration");
    }
}
