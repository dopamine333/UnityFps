using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingMenu : MonoBehaviour
{
    public List<GameObject> ObjectDataList = new List<GameObject>();
    public List<GameObject> NpcDataList = new List<GameObject>();
    public GameObject Player;
   // public List<GameObject> ObjectDataList_test;
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        Player = p;
        LoadPlayer();
        LoadObject();
        LoadNpc();
    }
    
    // Update is called once per frame
    void LoadObject()
    {
        var odl = SaveAndLoadGameData.LoadObject().objectDataList;
        Debug.Log("loadObiect...");

        foreach (var od in odl)
        {
            Debug.Log(od.MyPrefabPath);

            var prefab = Resources.Load<GameObject>(od.MyPrefabPath);
            var o = Instantiate(prefab);
            o.GetComponent<ObjData>().Load(od);
            if (od.InBag)
            {
                Player.GetComponent<PlayerAttack>().InBag.Add(o);
                o.GetComponent<Rigidbody>().velocity = Vector3.zero;
                o.GetComponent<Rigidbody>().isKinematic = true;
                o.GetComponent<ObjData>().meshCollider.isTrigger = true;
                o.SetActive(false);
            }
        }
        

    }
    void LoadNpc()
    {
        var ndl = SaveAndLoadGameData.LoadNpc().npcDataList;
        Debug.Log("loadNpc...");

        foreach (var nd in ndl)
        {
            var prefab = Resources.Load<GameObject>(nd.MyPrefabPath);
            Debug.Log(nd.MyPrefabPath);
            var n =Instantiate(prefab);
            n.GetComponent<Npc>().Load(nd);
        }
    }
    void LoadPlayer()
    {
        Debug.Log("loadPlayer...");

        PlayerData p = SaveAndLoadGameData.LoadPlayer();
        Player.GetComponent<PlayerAttack>().Load(p);
    }


    void Update()
    {
       // ObjectDataList_test = ObjectDataList;
        if (GameStatus.status == gameStatus.GameOver)
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameStatus.status == gameStatus.Playing)
        {
            if (GameStatus.status != gameStatus.Pause)
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameStatus.status = gameStatus.Playing;

    }
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameStatus.status = gameStatus.Pause;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("menu");
        GameStatus.status = gameStatus.MainMenu;
    }
    public void SaveMainScene()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Object");
        foreach (var o in go) { ObjectDataList.Add(o); }
        SaveAndLoadGameData.SaveObject(ObjectDataList);

        GameObject[] gn = GameObject.FindGameObjectsWithTag("Npc");
        foreach (var n in gn) { NpcDataList.Add(n); }
        SaveAndLoadGameData.SaveNpc(NpcDataList);

        SaveAndLoadGameData.SavePlayer(Player);

    }
    public void Save1()
    {
        GameStatus.fileName = FileName.File1;
        SaveMainScene();
    }
    public void Save2()
    {
        GameStatus.fileName = FileName.File2;
        SaveMainScene();
    }
    public void Save3()
    {
        GameStatus.fileName = FileName.File3;
        SaveMainScene();
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
