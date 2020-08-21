using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


  public enum gameStatus
    {
        MainMenu,
        Loading,
        Playing,
        Pause,
        GameOver
    }
    public enum FileName
    {
        File1,
        File2,
        File3
    }
public class GameStatus : MonoBehaviour
{
    public static gameStatus status = gameStatus.Playing;
    public static FileName fileName = FileName.File1;

}



