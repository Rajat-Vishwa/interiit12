using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pausemenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    public GameObject[] audioSources;
    
    private GameObject playerinstance;
    
    void Start(){
        playerinstance = GameObject.Find("Player");
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

        playerinstance.GetComponent<PlayerController>().enabled=false;
        playerinstance.GetComponentInChildren<LaserBehaviour>().enabled=false;

        pauseMenu.SetActive(true);
        Time.timeScale=0f;
        isPaused=true;
        
        foreach(GameObject audioSource in audioSources){
            audioSource.SetActive(false);
        }
    }

    public void ResumeGame(){

        playerinstance.GetComponent<PlayerController>().enabled=true;
        playerinstance.GetComponentInChildren<LaserBehaviour>().enabled=true;

        pauseMenu.SetActive(false);
        Time.timeScale=1f;
        isPaused=false;
        
        foreach(GameObject audioSource in audioSources){
            audioSource.SetActive(true);
        }
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
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale=1f;
        Application.Quit();
        isPaused = false;
        Debug.Log("quit");
    }
}
