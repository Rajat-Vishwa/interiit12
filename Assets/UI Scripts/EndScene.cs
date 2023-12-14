using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class EndScene : MonoBehaviour
{
    public TMP_InputField nameInput;
    public GameObject endMenu;

    public  TMP_Text ScoreText;

    public void SaveName()
    {
        int pscore = ScoreManager.highScore;
        string playerName = nameInput.text.Trim();
        const int maxNameLength = 8;
        if (playerName.Length > maxNameLength)
        {
            playerName = playerName.Substring(0, Math.Min(maxNameLength, 8)) + "..";
        }
        HighscoreTable.AddHighscoreEntry(pscore, playerName);
        Debug.Log("Saved");
    }

    public void Replay()
    {
        Time.timeScale=1f;
        endMenu.SetActive(false);
        SceneManager.LoadScene("MainScene");
        Debug.Log("Restart");
    }
}
