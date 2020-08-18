using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public static class SaveAndLoadGameData
{
    public static void SaveObject(List<GameObject> ObjectDataList)
    {
        ObjectDataList o = new ObjectDataList(ObjectDataList);
        string json = JsonUtility.ToJson(o);
        File.WriteAllText(Application.dataPath + "/saveObjectFile.json", json);
    }
    
    public static ObjectData LoadObject(int num)
    {
        
        string json = File.ReadAllText(Application.dataPath + "/saveObjectFile.json");
        if (json != null)
        {
            ObjectDataList o = JsonUtility.FromJson<ObjectDataList>(json);
            if (num < o.objectDataList.Count)
                return o.objectDataList[num];
            else
                return null;
        }
        else
        {
            return null;
        }
    }
    public static void SavePlayer(GameObject Player)
    {
        PlayerData p = new PlayerData(Player);
        string json = JsonUtility.ToJson(p);
        File.WriteAllText(Application.dataPath + "/savePlayerFile.json", json);
    }

    public static PlayerData LoadPlayer()
    {

        string json = File.ReadAllText(Application.dataPath + "/savePlayerFile.json");
        if (json != null)
        {
            PlayerData p = JsonUtility.FromJson<PlayerData>(json);
            return p;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class ObjectDataList
{
    public List<ObjectData> objectDataList = new List<ObjectData>();
    public ObjectDataList(List<GameObject> ObjectDataList)
    {
        for (int i = 0; i < ObjectDataList.Count; i++)
        {
            ObjectData o = new ObjectData(ObjectDataList[i]) ;
            objectDataList.Add(o);
        }
    }
}
[Serializable]
public class ObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public int Num;
    public ObjectData(GameObject Object)
    {
        Num = Object.GetComponent<ObjData>().MyNum;
        position = Object.transform.position;
        rotation = Object.transform.rotation;
        velocity = Object.GetComponent<Rigidbody>().velocity;
        
    }
}
[Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public int EXP;
    public float Health;
    public PlayerData(GameObject Player)
    {
        position = Player.transform.position;
        rotation = Player.transform.rotation;
        velocity = Player.GetComponent<Rigidbody>().velocity;
        EXP = Player.GetComponent<PlayerAttack>().EXP;
        Health = Player.GetComponent<PlayerAttack>().Health;

    }
}
    