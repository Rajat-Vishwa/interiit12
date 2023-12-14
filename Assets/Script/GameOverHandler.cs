using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverHandler : MonoBehaviour
{
    public GameObject endMenu;
    public GameObject player;
    public pausemenu pauseManager;
    public GameObject Sounds;

    public void GameOver()
    {
        Time.timeScale = 0f;
        endMenu.SetActive(true);
        endMenu.GetComponentInChildren<TMP_Text>().text = "Score : " + LevelManager.instance.Score.ToString();
        player.SetActive(false);
        Sounds.SetActive(false);
        pauseManager.enabled = false;
    }
}
