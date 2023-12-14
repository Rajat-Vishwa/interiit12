using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pausemenu : MonoBehaviour
{
     public GameObject pauseMenu;

    public static bool isPaused;
    private AudioSource audioSourceoff;

    public GameObject otherGameObject;
  


    private GameObject playerinstance;
    void Start(){
        audioSourceoff = otherGameObject.GetComponent<AudioSource>();
       
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
        audioSourceoff.Stop();


    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
        isPaused=false;
        audioSourceoff.Play();
    }
    public void RestartGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale=1f;
        isPaused = false;
        SceneManager.LoadScene("TestLeaderBoard");
    }

    public void QuitGame()
    {
        Time.timeScale=1f;
        Application.Quit();
        isPaused = false;
        Debug.Log("quit");
    }
}
