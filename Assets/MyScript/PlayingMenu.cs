using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.Hosting;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameStatus;

public class PlayingMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.GameStatus.status == gameStatus.GameOver)
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameStatus.GameStatus.status == gameStatus.Playing)
        {
            if (GameStatus.GameStatus.status != gameStatus.Pause)
            {
                Pause();
            }

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("now gamestatus is " + GameStatus.GameStatus.status);
        }
    }
    void GameOver()
    {
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(true);
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //GameIsPaused = false;
        GameStatus.GameStatus.status = gameStatus.Playing;

    }
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        //GameIsPaused = true;
        GameStatus.GameStatus.status = gameStatus.Pause;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
        GameStatus.GameStatus.status = gameStatus.MainMenu;

    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
