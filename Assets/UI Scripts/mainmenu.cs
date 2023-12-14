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
        SceneManager.LoadScene("MainScene");
        Debug.Log("start game");
    }
    public void BACK()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("back");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        Debug.Log("credits");
    }
}