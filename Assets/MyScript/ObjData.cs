//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ObjData : MonoBehaviour
{
    public enum Type
    {
        Small,
        Big,
        Long
    }
    public Type type;

    [Range(0.1f, 20f)]
    public float mass = 1;
    public float damage = 1;

    public Vector3 OffsetPosition;
    public Quaternion OffsetQuaternion=new Quaternion(0, 0, 0, 1);
    public bool CanEat;
    public int EXP;
    public float PlusHealthNum;

    public GameObject AfterCooking;
    //public int type;
    //public GameObject HoldPoint;
    [Header("Effect")]
    public GameObject TrailEffect;
    public Vector3 TrailEffectPosition;

    public GameObject CollisionEffect;

    [Header("Sound")]
    public AudioClip CollisionAudio1;
    public float volume1=100;
    public AudioClip CollisionAudio2;
    public float volume2=100;
    AudioSource CollisionAudioSource1;
    AudioSource CollisionAudioSource2;

    [Header("Icon")]
    public Sprite mySprite;

    [Header("")]
    public Rigidbody rb;
    public MeshCollider meshCollider;
   
    public bool InBag;
    
    public string MyPrefabPath;
#if UNITY_EDITOR
 
    public void OnValidate ()
    {
        if (!Application.isPlaying)
        {
            var Prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
            var Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            Path = Path.Substring(17, Path.Length - 24);
            MyPrefabPath = Path;
        }
    }
 
#endif

    void Awake()
    {
        AddAndSetRigidbody();
        AddAndSetMeshCollider();

    }

    void Start()
    {
        if (type == Type.Small || type == Type.Big)
        {
            //OffsetQuaternion = new Quaternion(0, 0, 0, 1);
        }

        AddAndSetTrailEffect();
        AddAndSetAudioSource();
        gameObject.tag = "Object";
    }
    public void Load(ObjectData o)
    {
        if (o != null)
        {
            AddAndSetRigidbody();
            AddAndSetMeshCollider();
            transform.position = o.position;
            transform.rotation = o.rotation;
            rb.velocity = o.velocity;
            rb.isKinematic = o.isKinematic;
            InBag = o.InBag;
            MyPrefabPath = o.MyPrefabPath;

        }
    }
    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
   
    void AddAndSetRigidbody()
    {
        if (GetComponent<Rigidbody>() == null)
            rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        else
            rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.isKinematic = true;
    }
    void AddAndSetMeshCollider()
    {
        if (GetComponent<MeshCollider>() == null)
            meshCollider = gameObject.AddComponent<MeshCollider>() as MeshCollider;
        else
            meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.convex = true;
        
    }
   
    void AddAndSetTrailEffect()
    {
        var tep = new Vector3(TrailEffectPosition.x * transform.localScale.x, TrailEffectPosition.y * transform.localScale.y, TrailEffectPosition.z * transform.localScale.z);
        GameObject Effect = Instantiate(TrailEffect, transform.position + transform.rotation * tep, new Quaternion(0, 0, 0, 0));
        Effect.transform.parent = gameObject.transform;
        Effect.transform.localScale = new Vector3(1, 1, 1);
    }
    void AddAndSetAudioSource()
    {
        CollisionAudioSource1 = gameObject.AddComponent<AudioSource>() as AudioSource;
        CollisionAudioSource1.clip = CollisionAudio1;
        CollisionAudioSource1.playOnAwake = false;
        CollisionAudioSource1.maxDistance = 10;
        CollisionAudioSource1.spatialBlend = 1;
        //CollisionAudioSource1.pitch = Mathf.Clamp((1 / mass),1f,0.2f);

        CollisionAudioSource2 = gameObject.AddComponent<AudioSource>() as AudioSource;
        CollisionAudioSource2.clip = CollisionAudio2;
        CollisionAudioSource2.playOnAwake = false;
        CollisionAudioSource2.maxDistance = 10;
        CollisionAudioSource2.spatialBlend = 1;

        ///CollisionAudioSource2.pitch = Mathf.Clamp((1 / mass), 1f, 0.2f);
    }

    void PlayCollisionEffect(Vector3 position)
    {
        GameObject Effect = Instantiate(CollisionEffect, position, Quaternion.identity) ;
        Destroy(Effect,5);
    }
    void OnCollisionEnter(Collision obj)
    {
        ContactPoint contact = obj.contacts[0];
        Vector3 position = contact.point;

        
        if(obj.gameObject.tag != "Player")
        {
            var p = 0.5f * rb.velocity.magnitude * rb.velocity.magnitude * mass;
            PlayCollisionAudio(p);

            if (p > 5) 
            {
                PlayCollisionEffect(position);
            }
        }
        
    }
    void PlayCollisionAudio(float power)
    {
        float proportion = Random.Range(0.8f , 0.2f);
        CollisionAudioSource1.PlayOneShot(CollisionAudioSource1.clip, (power) * proportion * volume1);
        CollisionAudioSource2.PlayOneShot(CollisionAudioSource2.clip, (power) * (1 - proportion) * volume2);

    }
     
}
