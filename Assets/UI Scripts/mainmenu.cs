using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Startgame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("start game");
    }
    public void Credits()
    {
        SceneManager.LoadScene("credits");
        Debug.Log("credits");
    }
}