using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameStatus
{
    public enum gameStatus
    {
        MainMenu,
        Loading,
        Playing,
        Pause,
        GameOver
    }
    public class GameStatus : MonoBehaviour
    {
        public static gameStatus status = gameStatus.MainMenu;

        // Update is called once per frame
        void Update()
        {
           
            if (status == gameStatus.GameOver)
            {
                Debug.Log("now gamestatus is " + status);
                // 開頭選單處理......
            }
            if (status == gameStatus.Playing)
            {
                // 遊戲進行中處理......
            }
            if (status == gameStatus.Pause)
            {
                // 遊戲進行中處理......
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("now gamestatus is " + status);
            }
        }
    }

}

