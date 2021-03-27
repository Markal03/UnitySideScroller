using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ResumeGame();
        }
    }
    public void ShowPauseMenu()
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
    
    public void ResumeGame()
    {
        Resume();
        gameObject.SetActive(false);
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
