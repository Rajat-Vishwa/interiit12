using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pausemenu : MonoBehaviour
{
     public GameObject pauseMenu;

    public static bool isPaused;
    
    private GameObject playerinstance;
    void Start(){
        pauseMenu.SetActive(false);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }else{
                PauseGame();
            }
        }
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale=0f;
        isPaused=true;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
        isPaused=false;
    }

    public void BackToMenu()
    {
        Time.timeScale=1f;
        SceneManager.LoadScene("TestLeaderBoard");
    }

    public void QuitGame()
    {
        Time.timeScale=1f;
        Application.Quit();
        Debug.Log("Quit");
    }
}
