using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5.0F;
 
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * smooth);
        
    }
   
}
