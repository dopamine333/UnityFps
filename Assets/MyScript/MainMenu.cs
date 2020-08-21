using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.Hosting;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject cam;

    Vector3 targetPosition;   // 目標位置
    Vector3 currentVelocity = Vector3.zero;     // 當前速度，這個值由你每次呼叫這個函式時被修改
    public float maxSpeed = 100f;    // 選擇允許你限制的最大速度
    public float smoothTime = 10f;      // 達到目標大約花費的時間。 一個較小的值將更快達到目標。
    float x = 1;
    float time = 0;
    void Update()
    {
        if (time % Mathf.Ceil(smoothTime*60) == 0)
        {
            x *= -1;
            targetPosition = cam.transform.position+cam.transform.right * x;
        }
        time += 1;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref currentVelocity, smoothTime, maxSpeed);
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("now gamestatus is " + GameStatus.status);
        }
    }
    public void LoadMainScene()
    {
        //Time.timeScale = 1f;
        //PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene("main");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameStatus.status = gameStatus.Playing;

    }
    public void Load1()
    {
        GameStatus.fileName = FileName.File1;
    }
    public void Load2()
    {
        GameStatus.fileName = FileName.File2;
    }
    public void Load3()
    {
        GameStatus.fileName = FileName.File3;
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    
}

