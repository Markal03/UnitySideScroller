using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void ShowGameOverMenu()
    {
        gameObject.SetActive(true);
        Pause();
    }
    public void LoadMainMenu()
    {
        Resume();
        gameObject.SetActive(false);
       
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Resume();
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }
    
    void Resume()
    {
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }
}
