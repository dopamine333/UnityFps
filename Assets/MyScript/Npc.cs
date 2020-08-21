using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEditor;

public class Npc : MonoBehaviour
{
    [Header("base")]
    public bool isBoss;
    public float MixHealth = 1000;
    public float Health;
    public float mass = 1;
    public float drag = 0;
    public GameObject[] DroppedItems;
    public float damage = 1;

    [Header("Effect")]
    public GameObject DeathEffect;
    public GameObject TakeDamageEffect;
    public GameObject JumpEffect;
    [Header("HealthBar")]
    public GameObject cam;
    public GameObject HealthBar;
    public Vector3 healthBarPosition;
    public Vector3 healthBarScale=new Vector3 (1,1,1);
    GameObject healthBar;

    [Header("WalkAround")]
    public GameObject WalkAroundPoint1;
    public Vector3 wap1;
    public GameObject WalkAroundPoint2;
    public Vector3 wap2;
    public Quaternion OffsetQuaternion=new Quaternion(0,0,0,1);
    public int MaxWalkAroundSpeed = 5;
    public int MinWalkAroundSpeed = 2;
    float WalkAroundSpeed;
    public float WalkAroundPower = 80;
    Vector3 WalkPoint;
    public Vector3 centerOfMass;
    float time = 1;
    GameObject tf;
    Rigidbody rb;
    public float smooth = 5.0F;
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
        tf = new GameObject();
        AddAndSetMeshCollider();
        AddAndSetRigidbody();
        AddAndSetHealthBar();
    }
    void Start()
    {
        Health = MixHealth;
        healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetMixHealth(MixHealth);
        healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetHealth(Health);
        WalkAroundSpeed = Random.Range(MinWalkAroundSpeed, MaxWalkAroundSpeed);
        rb.centerOfMass = centerOfMass;
        gameObject.tag = "Npc";
        if (WalkAroundPoint1 != null) { wap1 = WalkAroundPoint1.transform.position; }
        if (WalkAroundPoint2 != null) { wap2 = WalkAroundPoint2.transform.position; }

       
    }
    public void Load(NpcData n)
    {
        if (n != null)
        {
            AddAndSetRigidbody();
            AddAndSetMeshCollider();
            transform.position = n.position;
            transform.rotation = n.rotation;
            rb.velocity = n.velocity;
            if (n.Health <= MixHealth)
                Health = n.Health;
            wap1 = n.wap1;
            wap2 = n.wap2;
            MyPrefabPath = n.MyPrefabPath;

            healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetHealth(Health);

        }
    }
    
    void Update()
    {
        if (GameStatus.status == gameStatus.Playing)
        {
            
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameStatus.status == gameStatus.Playing)
        {
            if (!isBoss)
            {
                WalkAround();
            }
            
        }
        
    }
    void AddAndSetHealthBar()
    {
        if (healthBar == null)
        {
            GameObject pc = GameObject.FindWithTag("PlayerCam");
            cam = pc;
            healthBar = Instantiate(HealthBar, transform.position, new Quaternion(0, 0, 0, 0));
            healthBar.transform.localScale = healthBarScale;
            healthBar.transform.SetParent(gameObject.transform, true);
            //healthBar.transform.parent = gameObject.transform;
            healthBar.transform.GetChild(0).gameObject.transform.position = healthBarPosition + transform.position;
            healthBar.GetComponent<Billborard>().cam = cam;
        }
    }
    void AddAndSetRigidbody()
    {
        if (GetComponent<Rigidbody>() == null)
            rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        else
            rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;

    }
    void AddAndSetMeshCollider()
    {
        MeshCollider meshCollider;
        if (GetComponent<MeshCollider>() == null)
            meshCollider = gameObject.AddComponent<MeshCollider>() as MeshCollider;
        else
            meshCollider = gameObject.GetComponent<MeshCollider>();
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
            var o = col.gameObject.GetComponent<ObjData>();
            TakeDamage(position, 0.5f * o.mass * o.rb.velocity.magnitude * o.rb.velocity.magnitude * o.damage);
        }
    }
    void TakeDamage(Vector3 position,float damage)
    {
        if (damage > 1)
        {
            Health -= damage;
            healthBar.transform.GetChild(0).gameObject.GetComponent<HealthBar>().SetHealth(Health);
            if (Health < 0)
            {
                if (DeathEffect != null)
                {
                    GameObject Effect = Instantiate(DeathEffect, transform.position, new Quaternion(0, 0, 0, 0));
                    Destroy(Effect, 10);
                }
                Death();
            }
        }
        if (damage > 3 && TakeDamageEffect != null)
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
            rb.AddForce(tf.transform.forward * Time.fixedDeltaTime * WalkAroundSpeed * WalkAroundPower , ForceMode.Impulse);
            if(JumpEffect!=null)
                PlayJumpEffect();
            WalkPoint = new Vector3(
                       Random.Range(wap1.x, wap2.x),
                       Random.Range(wap1.y, wap2.y),
                       Random.Range(wap1.z, wap2.z));
            
            tf.transform.LookAt(WalkPoint);
            WalkAroundSpeed = Random.Range(MinWalkAroundSpeed, MaxWalkAroundSpeed);
        }
        tf.transform.position = transform.position;
        var q = Quaternion.Lerp(transform.rotation, tf.transform.rotation * OffsetQuaternion, Time.deltaTime * smooth);
        //q = (q * OffsetQuaternion).normalized;
        rb.MoveRotation(q);

    }
    void PlayJumpEffect()
    {
        GameObject Effect = Instantiate(JumpEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 5);
    }
    public void Death()
    {
        if (DroppedItems.Length != 0)
            for (int i = 0; i < DroppedItems.Length; i++)
            {

                GameObject obj = Instantiate(DroppedItems[i], transform.position + Vector3.up, new Quaternion(0, 0, 0, 0)) ;
                if (obj.GetComponent<ObjData>() != null)
                {
                    obj.GetComponent<Rigidbody>().isKinematic = false;
                }
            }

        GameObject s = GameObject.FindWithTag("SaveAndLoadGameData");
        s.GetComponent<PlayingMenu>().NpcDataList.Remove(gameObject);

        Destroy(gameObject);
    }
}
