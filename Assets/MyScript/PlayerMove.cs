using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using GameStatus;


public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public float speed=4f;//移動速度
    public float lookSensitivity = 2.5f;//視野旋轉速度
    public float cameraRotationlimit = 85f;//上下視野範圍
    public float JumpHigh = 1000f;

    [SerializeField]
    private Camera cam;
    private float currentCameraRotationX = 0f;
    [SerializeField]
    private bool IsGround = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Test();

        CloseMouse();
    }
    void FixedUpdate()
    {
        if (GameStatus.GameStatus.status == gameStatus.Playing)
        {
            Movement();
            Rotation();
          

        }

    }
    void Movement()
    {

        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHor = transform.right * _xMov;
        Vector3 _movVer = transform.forward * _zMov;

        Vector3 _velocity = (_movHor + _movVer).normalized * speed;

        if (_velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
            //rb.AddForce( _velocity * Time.fixedDeltaTime, ForceMode.Impulse);

        }
    }
    void Rotation()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _Rot = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_Rot));

        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _camRotX = _xRot * lookSensitivity;
        if (cam != null)
        {
            currentCameraRotationX -= _camRotX * 0.9f;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationlimit, cameraRotationlimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGround)
        {
            IsGround = false;
            rb.AddForce(gameObject.transform.up * Time.fixedDeltaTime * JumpHigh, ForceMode.Impulse);
        }
    }

    void CloseMouse()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnTriggerStay(Collider obj)
    {
        IsGround = true;
    }
    void OnTriggerExit(Collider obj)
    {
        IsGround = false;
       
    }
    void Test()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            speed += 1;
            Debug.Log(rb.velocity.magnitude);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            speed -= 1;
        }

    }
}
