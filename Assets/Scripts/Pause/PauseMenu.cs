﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI, optionsUI, exitUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                optionsUI.SetActive(false);
                exitUI.SetActive(false);
            } else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMainmenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }

    public void QuitGame()
    {
        //Debug.Log("Quitting game...");
        Application.Quit();
    }

    public bool GetPauseStatus()
    {
        return GameIsPaused;
    }
}
