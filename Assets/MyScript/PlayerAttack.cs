﻿
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public int BagObjIndex = 0;
    public GameObject OnHand;
    public List<GameObject> InBag;
    //public int NowBagLength = 0;

    public int EXP = 0;
    public int level = 0;
    public float MixHealth = 20;
    public float Health;

    float InitialPower;
    float MixAccumulateTime;
    float MixPower;
    float AccumulateTime;
    bool InAccumulate;

    public float power;

    [Header("UI")]

    public GameObject HealthBar;
    public GameObject LevelBar;
    public Text LevelText;


    public GameObject InBagIcon5;
    public GameObject InBagIcon4;
    public GameObject InBagIcon3;
    public GameObject InBagIcon2;
    public GameObject InBagIcon1;

    [Header("")]
    //Ray ray;
    RaycastHit hit;
    public GameObject BeCheckObj;
    //public Vector3 BeCheckPoint;

    
    [SerializeField]
    Camera cam;
    public float MixCameraField=20f;
    public float InitialCameraField = 60f;
    //public float CameraField = 60f;


    public GameObject testPoint;
    // Start is called before the first frame update
    void Start()
    {
        InitialUI();
        Health = MixHealth;
        HealthBar.GetComponent<HealthBar>().SetMixHealth(MixHealth);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            CheckUp();
            SetBagObjIndex();
            LevelUp();
            AccumulateAttack();

            //Attack();
            Eat();
            PickUp();
            OpenOrCloseDoor();
            SetUI();

            Test();

        }

    }
   
    void CheckUp()
    {
        Vector3 position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
        Ray ray = cam.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            if (BeCheckObj != null && BeCheckObj.tag == "CanThrow")
            {   
                if (BeCheckObj.GetComponent<Renderer>() != null)
                    {
                        if (BeCheckObj != hit.collider.gameObject)
                        {
                            BeCheckObj.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                            BeCheckObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);


                        }
                        else
                        {
                            BeCheckObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.blue);
                            BeCheckObj.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

                        }
                    }
            }
            BeCheckObj = hit.collider.gameObject;
            //BeCheckPoint = hit.point;

        }
        else
        {
            if (BeCheckObj != null)
            {

                if (BeCheckObj.GetComponent<Renderer>() != null)
                {
                    BeCheckObj.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    BeCheckObj.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                }
            }
            
            BeCheckObj = null;
        }
        
        
    }

    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.E) && BeCheckObj != null && BeCheckObj.GetComponent<ObjData>() != null)
        {
            BeCheckObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            BeCheckObj.SetActive(false);
            InBag.Insert(BagObjIndex, BeCheckObj);
            
            //InBag.Add(BeCheckObj);
              
        }
          
    }

    void OpenOrCloseDoor()
    {
        if (Input.GetKeyDown(KeyCode.E) && BeCheckObj != null && BeCheckObj.tag == "Door")
        {
            BeCheckObj.GetComponent<DoorToggle>().OpenOrCloseDoor();
        }

    }

    void SetBagObjIndex()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            BagObjIndex++;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            BagObjIndex--;
            
        }
        if (BagObjIndex > InBag.Count - 1)
            BagObjIndex = 0;

        if (BagObjIndex < 0)
            BagObjIndex = InBag.Count - 1;

        if (BagObjIndex < 0 && InBag.Count == 0)
            BagObjIndex = 0;

        if (InBag.Count != 0)
        {
            OnHand = InBag[BagObjIndex];
        }
        else
        {
            OnHand = null;
        }
        
    }

    void Eat()
    {
        if (OnHand != null&&OnHand.GetComponent<ObjData>().CanEat)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Eat!!!");
                EXP += OnHand.GetComponent<ObjData>().EXP;
                Health += OnHand.GetComponent<ObjData>().RecoveryNum;
                HealthBar.GetComponent<HealthBar>().SetHealth(Health);

                if (InBag.Count != 0)
                {
                    InBag.Remove(InBag[BagObjIndex]);
                }
                SetBagObjIndex();
            }
        }
    }

    void Attack(float power)
    {

        if (90f > cam.transform.eulerAngles.x && cam.transform.eulerAngles.x > 23f)
        {
            //Debug.Log(cam.transform.eulerAngles.x);
            OnHand.transform.position = cam.transform.position + transform.forward;
            OnHand.transform.LookAt(OnHand.transform.position + cam.transform.right);

        }
        else
        {
            //Debug.Log(cam.transform.eulerAngles.x);
            OnHand.transform.position = cam.transform.position + cam.transform.forward;
            OnHand.transform.LookAt(OnHand.transform.position + cam.transform.right);
            Vector3 vec = cam.transform.eulerAngles;
            OnHand.transform.eulerAngles += new Vector3(0, 0, vec.x);
        }

        
        OnHand.GetComponent<Rigidbody>().isKinematic = false;
        OnHand.SetActive(true);

        OnHand.GetComponent<Rigidbody>().AddForce(cam.transform.forward * Time.fixedDeltaTime * power, ForceMode.Impulse);
        //OnHand.GetComponent<ObjData>().speed = 0;

        //OnHand.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Time.fixedDeltaTime;

        if (InBag.Count != 0)
        {
            InBag.Remove(InBag[BagObjIndex]);
        }
        SetBagObjIndex();


    }

    void AccumulateAttack()
    {


        if (Input.GetMouseButtonDown(0))
        {
            InAccumulate = true;
        }
        if (Input.GetMouseButton(0) && InAccumulate)
        {
            power = ((InitialPower - MixPower) / MixAccumulateTime / MixAccumulateTime) * (AccumulateTime / 60 - MixAccumulateTime) * (AccumulateTime / 60 - MixAccumulateTime) + MixPower;
            AccumulateTime += 1;
            //cam.fieldOfView = MixPower / power * MixCameraField;
            cam.fieldOfView = (MixCameraField - InitialCameraField) / (MixPower - InitialPower) * (power - InitialPower) + InitialCameraField;
        }

        if (AccumulateTime / 60 < MixAccumulateTime)
        {
            if (Input.GetMouseButtonUp(0) && InAccumulate)
            {
                if (OnHand != null)
                    Attack(power);

                power = InitialPower;
                AccumulateTime = 0;
                //cam.fieldOfView = MixPower / power * MixCameraField;
                cam.fieldOfView = (MixCameraField - InitialCameraField) / (MixPower - InitialPower) * (power - InitialPower) + InitialCameraField;

            }
        }
        else
        {
            InAccumulate = false;

            power = InitialPower;
            AccumulateTime = 0;
            //cam.fieldOfView = MixPower / power * MixCameraField;
            cam.fieldOfView = (MixCameraField - InitialCameraField) / (MixPower - InitialPower) * (power - InitialPower) + InitialCameraField;


        }



    }

    public void TakeDamage(Vector3 position,float damage)
    {
        if (transform.position.y < position.y)
        {
            GetComponent<Rigidbody>().AddForce((transform.position - position)-new Vector3(0, (transform.position - position).y,0) * Time.fixedDeltaTime * 100f, ForceMode.Impulse);

        }
        else
        {
            GetComponent<Rigidbody>().AddForce((transform.position-position) * Time.fixedDeltaTime * 1300f, ForceMode.Impulse);
        }
        Health -= damage;
        HealthBar.GetComponent<HealthBar>().SetHealth(Health);
    }

    void LevelUp()
    {
        if (EXP > 10000)
        {
            level = 6;
            Debug.Log("我的天啊");
        }
        else if (EXP > 1000)
        {
            level = 5;
        }
        else if (EXP > 400)
        {
            level = 4;
            LevelBar.GetComponent<HealthBar>().SetMixHealth(1000);

        }
        else if (EXP > 150)
        {
            level = 3;
            LevelBar.GetComponent<HealthBar>().SetMixHealth(400);

        }
        else if (EXP > 50)
        {
            level = 2;
            LevelBar.GetComponent<HealthBar>().SetMixHealth(150);

        }
        else if (EXP > 20)
        {
            level = 1;
            LevelBar.GetComponent<HealthBar>().SetMixHealth(50);

        }
        else
        {
            level = 0;
            LevelBar.GetComponent<HealthBar>().SetMixHealth(20);
        }

        LevelBar.GetComponent<HealthBar>().SetHealth(EXP);
        LevelText.text = "Lv." + level;

        if (level == 0)
        {
            MixPower = 900f;
            InitialPower = 200f;
            MixAccumulateTime = 3f;
        }
        if (level == 1)
        {
            MixPower = 1200f;
            InitialPower = 250f;
            MixAccumulateTime = 4f;
        }
        if (level == 2)
        {
            MixPower = 1600f;
            InitialPower = 270f;
            MixAccumulateTime = 4.5f;
        }
        if (level == 3)
        {
            MixPower = 2000f;
            InitialPower = 300f;
            MixAccumulateTime = 5f;
        }
        if (level == 4)
        {
            MixPower = 2300f;
            InitialPower = 400f;
            MixAccumulateTime = 5.5f;
        }
        if (level == 5)
        {
            MixPower = 2500f;
            InitialPower = 500f;
            MixAccumulateTime = 6f;
        }
        if (level == 6)
        {
            MixPower = 100000f;
            InitialPower = 400f;
            MixAccumulateTime = 10f;
        }

    }

    void InitialUI()
    {
        InBagIcon1.SetActive(false);
        InBagIcon2.SetActive(false);
        InBagIcon3.SetActive(false);
        InBagIcon4.SetActive(false);
        InBagIcon5.SetActive(false);
    }
    void SetUI()
    {
        if (InBag.Count == 0)
        {
            InBagIcon1.SetActive(false);
            InBagIcon2.SetActive(false);
            InBagIcon3.SetActive(false);
            InBagIcon4.SetActive(false);
            InBagIcon5.SetActive(false);

            
        }
        else if (InBag.Count == 1)
        {
            InBagIcon1.SetActive(false);
            InBagIcon2.SetActive(false);
            InBagIcon3.SetActive(true);
            InBagIcon4.SetActive(false);
            InBagIcon5.SetActive(false);

            InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
        }
        else if (InBag.Count == 2)
        {
            InBagIcon1.SetActive(false);
            InBagIcon2.SetActive(false);
            InBagIcon3.SetActive(true);
            InBagIcon4.SetActive(true);
            InBagIcon5.SetActive(false);

            if (BagObjIndex == 0)
            {
                //InBagIcon2.GetComponent<Image>().sprite = InBag[BagObjIndex+1].GetComponent<ObjData>().mySprite;
                InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
                InBagIcon4.GetComponent<Image>().sprite = InBag[BagObjIndex+1].GetComponent<ObjData>().mySprite;
            }
            else
            {
                //InBagIcon2.GetComponent<Image>().sprite = InBag[BagObjIndex-1].GetComponent<ObjData>().mySprite;
                InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
                InBagIcon4.GetComponent<Image>().sprite = InBag[BagObjIndex-1].GetComponent<ObjData>().mySprite;
            }
            
        }
        else
        {
            InBagIcon1.SetActive(false);
            InBagIcon2.SetActive(true);
            InBagIcon3.SetActive(true);
            InBagIcon4.SetActive(true);
            InBagIcon5.SetActive(false);

            if (BagObjIndex == 0)
            {
                InBagIcon2.GetComponent<Image>().sprite = InBag[InBag.Count - 1].GetComponent<ObjData>().mySprite;
                InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
                InBagIcon4.GetComponent<Image>().sprite = InBag[BagObjIndex + 1].GetComponent<ObjData>().mySprite;
            }
            else if (BagObjIndex == InBag.Count - 1)
            {
                InBagIcon2.GetComponent<Image>().sprite = InBag[BagObjIndex - 1].GetComponent<ObjData>().mySprite;
                InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
                InBagIcon4.GetComponent<Image>().sprite = InBag[0].GetComponent<ObjData>().mySprite;
            }
            else
            {
                InBagIcon2.GetComponent<Image>().sprite = InBag[BagObjIndex - 1].GetComponent<ObjData>().mySprite;
                InBagIcon3.GetComponent<Image>().sprite = InBag[BagObjIndex].GetComponent<ObjData>().mySprite;
                InBagIcon4.GetComponent<Image>().sprite = InBag[BagObjIndex + 1].GetComponent<ObjData>().mySprite;
            }
            
        }


    }

    void Test()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            transform.position = testPoint.transform.position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            power += 200;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            power -= 200;
        }
    }

}