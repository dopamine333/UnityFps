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
    public List<GameObject> ObjectDataList = new List<GameObject>();
    public GameObject Player;
   // public List<GameObject> ObjectDataList_test;
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        LoadObject();
        LoadPlayer();
    }

    // Update is called once per frame
    void LoadObject()
    {
        for (int i = 0; i < ObjectDataList.Count; i++)
        {
            ObjectData od = SaveAndLoadGameData.LoadObject(i);
            ObjectDataList[i].GetComponent<ObjData>().Load(od);

        }

    }
    void LoadPlayer()
    {
        PlayerData p = SaveAndLoadGameData.LoadPlayer();
        Player.GetComponent<PlayerAttack>().Load(p);
    }

    void Update()
    {
       // ObjectDataList_test = ObjectDataList;
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
            //Debug.Log("now gamestatus is " + GameStatus.GameStatus.status);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //GameObject gameObject = GameObject.FindWithTag("Object");
            //SaveAndLoadGameData.SaveObject(ObjectDataList);
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
        GameStatus.GameStatus.status = gameStatus.Playing;

    }
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameStatus.GameStatus.status = gameStatus.Pause;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SaveAndLoadGameData.SaveObject(ObjectDataList);
        SaveAndLoadGameData.SavePlayer(Player);
        SceneManager.LoadScene("menu");
        GameStatus.GameStatus.status = gameStatus.MainMenu;
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
