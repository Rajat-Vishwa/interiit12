using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pausemenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        pauseUi.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    void Pause()
    {
        pauseUi.SetActive(true);
        Time.timeScale = 0f;
        paused = true;

    }
    public void MM()
    {
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("startmenu");
        }
    }
}
