﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;
using GameStatus;

public class Npc : MonoBehaviour
{
    [Header("base")]
    public bool isBoss;
    public float MixHealth = 1000;
    public float Health;
    public float mass = 1;
    public GameObject[] DroppedItems;
    public float damage = 1;

    [Header("Effect")]
    public GameObject DeathEffect;
    public GameObject TakeDamageEffect;

    [Header("HealthBar")]
    public GameObject cam;
    public GameObject HealthBar;
    public Vector3 healthBarPosition;
    GameObject healthBar;

    [Header("isNotBoss")]
    public GameObject WalkAroundPoint1;
    public GameObject WalkAroundPoint2;
    public float WalkAroundSpeed=1;
    public float WalkAroundPower = 80;
    public Vector3 WalkPoint;
    float time = 1;
    GameObject tf;
    public float smooth = 5.0F;
    // Start is called before the first frame update
    void Awake()
    {
        tf = new GameObject();
        AddAndSetMeshCollider();
        AddAndSetRigidbody();
        AddAndSetHealthBar();
        
        if (!isBoss)
        {
            WalkAroundSpeed = Random.Range(2, 5);
        }
    }
    void Start()
    {
        Health = MixHealth;
        healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetMixHealth(MixHealth);
        healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetHealth(Health);
       
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0.126f, -0.06f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(GameStatus.GameStatus.status == gameStatus.Playing)
        {
            if (!isBoss)
            {
                WalkAround();
            }
            if (Health < 0)
            {
                Death();
            }
        }
        
    }
    void AddAndSetHealthBar()
    {
        healthBar = Instantiate(HealthBar, transform.position, new Quaternion(0, 0, 0, 0));
        healthBar.transform.SetParent(gameObject.transform, true);
        //healthBar.transform.parent = gameObject.transform;
        healthBar.transform.localScale = new Vector3(1, 1, 1);
        
        healthBar.transform.GetChild(0).gameObject.transform.position = healthBarPosition + transform.position;
        healthBar.GetComponent<Billborard>().cam = cam;
    }
    void AddAndSetRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        rb.mass = mass;
    }
    void AddAndSetMeshCollider()
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>() as MeshCollider;
        meshCollider.convex = true;
    }
    
    
    void OnCollisionEnter(Collision col)
    {
        ContactPoint contact = col.contacts[0];
        Vector3 position = contact.point;
        if (!isBoss)
        {
            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<PlayerAttack>().TakeDamage(transform.position,damage);
            }
        }
        if (col.gameObject.GetComponent<ObjData>() != null)
        {
            //Debug.Log(col.gameObject.GetComponent<ObjData>().speed);
            TakeDamage(position, 0.5f * col.gameObject.GetComponent<ObjData>().mass * col.gameObject.GetComponent<ObjData>().speed * col.gameObject.GetComponent<ObjData>().speed);
        }
    }
    void TakeDamage(Vector3 position,float damage)
    {
        if (damage > 1)
        {
            Health -= damage;
            healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetHealth(Health);
        }
        if( damage > 3 )
            PlayTakeDamageEffect(position);
    }
    void PlayTakeDamageEffect(Vector3 position)
    {
        GameObject Effect = Instantiate(TakeDamageEffect, position, Quaternion.identity);
        Destroy(Effect, 5);
    }
    void WalkAround()
    {
        time += 1;
        if ((time % (WalkAroundSpeed * 60) == 0)  )
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * Time.fixedDeltaTime * WalkAroundSpeed * WalkAroundPower, ForceMode.Impulse);

            WalkPoint = new Vector3(
                       Random.Range(WalkAroundPoint1.transform.position.x, WalkAroundPoint2.transform.position.x),
                       Random.Range(WalkAroundPoint1.transform.position.y, WalkAroundPoint2.transform.position.y),
                       Random.Range(WalkAroundPoint1.transform.position.z, WalkAroundPoint2.transform.position.z));
            
            tf.transform.LookAt(WalkPoint);
        }
        tf.transform.position = transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, tf.transform.rotation, Time.deltaTime * smooth);
        //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(tf.transform.rotation.x, tf.transform.rotation.y, tf.transform.rotation.z, tf.transform.rotation.w), Time.deltaTime * smooth);

    }
    void Death()
    {
        if(DeathEffect != null)
        {
            GameObject Effect = Instantiate(DeathEffect, transform.position, new Quaternion(0, 0, 0, 0));

            Destroy(Effect, 10);
        }
        if (DroppedItems.Length != 0)
            for (int i = 0; i < DroppedItems.Length; i++)
            {
                
                GameObject obj=Instantiate(DroppedItems[i],transform.position, new Quaternion(0, 0, 0, 0));
                
            }
        Destroy(gameObject);
    }
}
