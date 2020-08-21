
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PlayerAttack : MonoBehaviour
{
    public int BagObjIndex = 0;
    public GameObject OnHand;
    public List<GameObject> InBag = new List<GameObject>();
    public float smooth = 12f;

    //public int NowBagLength = 0;

    public int EXP = 0;
    public int level = 0;
    public float MixHealth = 20;
    public float Health;
    public float AttackRange = 2;

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
    RaycastHit hit2;
    public GameObject BeCheckObj;
    public Vector3 BeCheckPoint;

    
    [SerializeField]
    Camera cam;
    public float MixCameraField=20f;
    public float InitialCameraField = 80f;
    //public float CameraField = 60f;

    GameObject Hand;
    public GameObject HandPoint;
    public GameObject HandPoint_Small;
    public GameObject HandPoint_Big;
    public GameObject HandPoint_Long;
    public GameObject testPoint;

    void Awake()
    {

    }
    void Start()
    {
        
        Hand = new GameObject("Hand");
        Health = MixHealth;
        cam.fieldOfView = InitialCameraField;
        InitialUI();
        HealthBar.GetComponent<HealthBar>().SetMixHealth(MixHealth);
        HealthBar.GetComponent<HealthBar>().SetHealth(Health);
        LevelUp();
        gameObject.tag = "Player";

    }
    public void Load(PlayerData p)
    {
        if (p != null)
        {
            transform.position = p.position;
            transform.rotation = p.rotation;
            GetComponent<Rigidbody>().velocity = p.velocity;
            EXP = p.EXP;
            Health = p.Health;
            GetComponent<PlayerMove>().currentCameraRotationX = p.currentCameraRotationX;
            BagObjIndex = p.BagObjIndex;
           
            LevelUp();
            HealthBar.GetComponent<HealthBar>().SetHealth(Health);
            SetBagObjIndex();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (GameStatus.status == gameStatus.Playing)
        {
            PickUp();
            OpenOrCloseDoor();
            SetUI();
            SetBagObjIndex();

            Eat();
            AccumulateAttackUpdate();

            Test();

        }

    }
    void FixedUpdate()
    {
        if (GameStatus.status == gameStatus.Playing)
        {
            CheckUp();
            SetHandPosition();
            AccumulateAttackFixedUpdate();

        }
        else if(GameStatus.status == gameStatus.Pause)
        {
            SetHandPosition();

        }
    }
    void CheckUp()
    {
        Vector3 position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);

        //if(!InAccumulate)
        
            Ray ray = cam.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, AttackRange, -5, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);

                if (BeCheckObj != null && BeCheckObj.tag == "Object")
                {
                    if (BeCheckObj.GetComponent<Renderer>() != null)
                    {
                        var BeCheckObjMaterial = BeCheckObj.GetComponent<Renderer>().material;
                        if (BeCheckObj != hit.collider.gameObject)
                        {
                            BeCheckObjMaterial.DisableKeyword("_EMISSION");
                            BeCheckObjMaterial.SetColor("_EmissionColor", Color.black);

                        }
                        else
                        {
                            BeCheckObjMaterial.SetColor("_EmissionColor", Color.blue);
                            BeCheckObjMaterial.EnableKeyword("_EMISSION");

                        }
                    }
                }
                BeCheckObj = hit.collider.gameObject;
           

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
        
        Ray ray2 = cam.ScreenPointToRay(position);
        if (Physics.Raycast(ray2, out hit2,100,-5,QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(ray2.origin, hit2.point, Color.blue);
            BeCheckPoint = hit2.point;
        }
        else
        {
            BeCheckPoint = cam.transform.forward*100 + cam.transform.position;
        }
        
    }
    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.E) && BeCheckObj != null && BeCheckObj.GetComponent<ObjData>() != null)
        {
            var rb = BeCheckObj.GetComponent<Rigidbody>();
            var o = BeCheckObj.GetComponent<ObjData>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            o.meshCollider.isTrigger = true;
            o.InBag = true;



            InBag.Insert(BagObjIndex, BeCheckObj);

            SetBagObjIndex();

        }

    }
    public GameObject GetPrefabAsset(GameObject go)
    {
        string pathToPrefab = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
        return PrefabUtility.GetCorrespondingObjectFromSourceAtPath(go, pathToPrefab);
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
        for (int i = 0; i < InBag.Count; i++)
        {
            InBag[i].SetActive(false);
        }
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
            OnHand.SetActive(true);

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
                var o = OnHand.GetComponent<ObjData>();

                EXP += o.EXP;
                Health += o.PlusHealthNum;
                if (Health >= MixHealth)
                {
                    Health = MixHealth;
                }
                HealthBar.GetComponent<HealthBar>().SetHealth(Health);
                LevelUp();

                o.InBag = false;

                Destroy(OnHand);
                InBag.Remove(OnHand);
                
                SetBagObjIndex();
            }
        }
    }
    void SetHandPosition()
    {
        if (OnHand != null)
        {
            var OnHandObjType = OnHand.GetComponent<ObjData>().type;
            if (OnHandObjType == ObjData.Type.Small)
            {
                //Debug.Log("Small");
                HandPoint = HandPoint_Small;
            }
            else if (OnHandObjType == ObjData.Type.Big)
            {
                //Debug.Log("Big");
                HandPoint = HandPoint_Big;

            }
            else if (OnHandObjType == ObjData.Type.Long)
            {
                //Debug.Log("Long");
                HandPoint = HandPoint_Long;

            }
            Vector3 OffsetPosition = OnHand.GetComponent<ObjData>().OffsetPosition;
            var OffsetQuaternion = OnHand.GetComponent<ObjData>().OffsetQuaternion;
            Transform HandFt = Hand.transform;

            var rb = OnHand.GetComponent<Rigidbody>();

            //rb.MoveRotation(HandFt.rotation * OnHand.GetComponent<ObjData>().OffsetQuaternion);
            OnHand.transform.rotation = HandFt.rotation * OffsetQuaternion;

            //rb.MovePosition(HandFt.position + OnHand.transform.rotation * -OffsetPosition);
            OnHand.transform.position = HandFt.position + OnHand.transform.rotation * Quaternion.Inverse(OffsetQuaternion) * OffsetPosition;
        }
        else
        {
            HandPoint = HandPoint_Small;
        }

        Hand.transform.position = Vector3.Lerp(Hand.transform.position, HandPoint.transform.position, Time.deltaTime * smooth);
        Hand.transform.rotation = Quaternion.Slerp(Hand.transform.rotation, HandPoint.transform.rotation, Time.deltaTime * smooth);

    }
    void Attack(float power)
    {

        if (90f > cam.transform.eulerAngles.x && cam.transform.eulerAngles.x > 90f)
        {
            //Debug.Log(cam.transform.eulerAngles.x);
           ////OnHand.transform.position = cam.transform.position + transform.forward;
            //OnHand.transform.LookAt(OnHand.transform.position + cam.transform.right);

        }
        else
        {
            //Debug.Log(cam.transform.eulerAngles.x);
            ////OnHand.transform.position = cam.transform.position + cam.transform.forward;
            //OnHand.transform.LookAt(OnHand.transform.position + cam.transform.right);
            //Vector3 vec = cam.transform.eulerAngles;
            //OnHand.transform.eulerAngles += new Vector3(0, 0, vec.x);
        }

        var rb = OnHand.GetComponent<Rigidbody>();
        var o = OnHand.GetComponent<ObjData>();
        rb.isKinematic = false;
        o.meshCollider.isTrigger = false;
        o.InBag = false;
        //OnHand.GetComponent<Rigidbody>().AddForce(cam.transform.forward * Time.fixedDeltaTime * power, ForceMode.Impulse);
        rb.AddForce((BeCheckPoint - HandPoint.transform.position).normalized * Time.fixedDeltaTime * power, ForceMode.Impulse);

        GetComponent<Rigidbody>().AddForce(-cam.transform.forward * Time.fixedDeltaTime * power, ForceMode.Impulse);
        //OnHand.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Time.fixedDeltaTime;


        InBag.Remove(InBag[BagObjIndex]);
        SetBagObjIndex();

    }

    void AccumulateAttackUpdate()
    {


        if (Input.GetMouseButtonDown(0))
        {
            InAccumulate = true;
        }
        
        if (AccumulateTime / 60 < MixAccumulateTime)
        {
            if (Input.GetMouseButtonUp(0) && InAccumulate)
            {
                if (OnHand != null)
                    Attack(power);

                power = InitialPower;
                AccumulateTime = 0;
                InAccumulate = false;

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
    void AccumulateAttackFixedUpdate()
    {
        if (Input.GetMouseButton(0) && InAccumulate)
        {
            power = ((InitialPower - MixPower) / MixAccumulateTime / MixAccumulateTime) * (AccumulateTime / 60 - MixAccumulateTime) * (AccumulateTime / 60 - MixAccumulateTime) + MixPower;
            AccumulateTime += 1;
            //cam.fieldOfView = MixPower / power * MixCameraField;
            cam.fieldOfView = (MixCameraField - InitialCameraField) / (MixPower - InitialPower) * (power - InitialPower) + InitialCameraField;
        }
    }
    public void TakeDamage(Vector3 position,float damage)
    {
        if (transform.position.y < position.y)
        {
            GetComponent<Rigidbody>().AddForce((transform.position - position)-new Vector3(0, (transform.position - position).y,0) * Time.fixedDeltaTime * 100, ForceMode.VelocityChange);

        }
        else
        {
            GetComponent<Rigidbody>().AddForce((transform.position-position) * Time.fixedDeltaTime * 100, ForceMode.VelocityChange);
        }
        Health -= damage;
        if (Health <= 0)
        {
            GameOver();
        }
        HealthBar.GetComponent<HealthBar>().SetHealth(Health);
    }
    void GameOver()
    {
        GameStatus.status = gameStatus.GameOver;

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
            MixPower = 2000f;
            InitialPower = 200f;
            MixAccumulateTime = 4f;
            MixCameraField = 60f;
        }
        if (level == 1)
        {
            MixPower = 1200f;
            InitialPower = 250f;
            MixAccumulateTime = 4.2f;
            MixCameraField = 50f;

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
