using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Specialized;

public static class SaveAndLoadGameData
{

    public static void SaveObject(List<GameObject> ObjectDataList)
    {

        ObjectDataList o = new ObjectDataList(ObjectDataList);
        string json = JsonUtility.ToJson(o);
        File.WriteAllText(Application.dataPath + "/MySaveFile/saveObject" + GameStatus.fileName.ToString() + ".json", json);
    }
    
    public static ObjectDataList LoadObject()
    {
        
        string json = File.ReadAllText(Application.dataPath + "/MySaveFile/saveObject" + GameStatus.fileName.ToString() + ".json");
        if (json != null)
        {
            ObjectDataList o = JsonUtility.FromJson<ObjectDataList>(json);
            return o;
        }
        else
        {
            return null;
        }
    }
    public static void SaveNpc(List<GameObject> NpcDataList)
    {
        NpcDataList o = new NpcDataList(NpcDataList);
        string json = JsonUtility.ToJson(o);
        File.WriteAllText(Application.dataPath + "/MySaveFile/saveNpc" + GameStatus.fileName.ToString() + ".json", json);
    }

    public static NpcDataList LoadNpc()
    {

        string json = File.ReadAllText(Application.dataPath + "/MySaveFile/saveNpc" + GameStatus.fileName.ToString() + ".json");
        if (json != null)
        {
            NpcDataList n = JsonUtility.FromJson<NpcDataList>(json);
            return n;
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
        File.WriteAllText(Application.dataPath + "/MySaveFile/savePlayer" + GameStatus.fileName.ToString() + ".json", json);
    }
    
    public static PlayerData LoadPlayer()
    {
        string json = File.ReadAllText(Application.dataPath + "/MySaveFile/savePlayer" + GameStatus.fileName.ToString() + ".json");
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
    public bool isKinematic;
    public bool InBag;
    public string MyPrefabPath;

    public ObjectData(GameObject Object)
    {
        var o = Object.GetComponent<ObjData>();
        var rb = Object.GetComponent<Rigidbody>();
        position = Object.transform.position;
        rotation = Object.transform.rotation;
        velocity = rb.velocity;
        isKinematic = rb.isKinematic;
        InBag = o.InBag;
        MyPrefabPath = o.MyPrefabPath;

    }
}
[Serializable]
public class NpcDataList
{
    public List<NpcData> npcDataList = new List<NpcData>();

    public NpcDataList(List<GameObject> NpcDataList)
    {
        for (int i = 0; i < NpcDataList.Count; i++)
        {
            NpcData n = new NpcData(NpcDataList[i]);
            npcDataList.Add(n);
        }
    }
}
[Serializable]
public class NpcData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public float Health;
    public string MyPrefabPath;
    public Vector3 wap1;
    public Vector3 wap2;

    public NpcData(GameObject Npc)
    {
        var n = Npc.GetComponent<Npc>();
        var rb = Npc.GetComponent<Rigidbody>();
        position = Npc.transform.position;
        rotation = Npc.transform.rotation;
        velocity = rb.velocity;
        Health = n.Health;
        MyPrefabPath = n.MyPrefabPath;
        wap1 = n.wap1;
        wap2 = n.wap2;
    }
}
[Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public float currentCameraRotationX;
    public int EXP;
    public float Health;
    //public List<GameObject> InBag = new List<GameObject>();
    public int BagObjIndex = 0;

    public PlayerData(GameObject Player)
    {
        var p = Player.GetComponent<PlayerAttack>();
        position = Player.transform.position;
        rotation = Player.transform.rotation;
        velocity = Player.GetComponent<Rigidbody>().velocity;
        EXP = p.EXP;
        Health = p.Health;
        currentCameraRotationX = Player.GetComponent<PlayerMove>().currentCameraRotationX;
        BagObjIndex = p.BagObjIndex;
        
    }
}
    