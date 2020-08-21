using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float FireDamage=2f;


    void OnTriggerEnter(Collider obj)
    {
        var o = obj.gameObject.GetComponent<ObjData>();
        if (o != null)
        {
            if (o.AfterCooking != null)
            {
                GameObject AfterCookingObj = Instantiate(o.AfterCooking, transform.position, new Quaternion(0, 0, 0, 0));
                AfterCookingObj.GetComponent<Rigidbody>().isKinematic = false;
                AfterCookingObj.GetComponent<Rigidbody>().velocity = Vector3.zero ;
                AfterCookingObj.GetComponent<Rigidbody>().AddForce(Vector3.up * Time.fixedDeltaTime * 300, ForceMode.VelocityChange);

                GameObject p = GameObject.FindWithTag("Player");
                int id = p.GetComponent<PlayerAttack>().InBag.IndexOf(obj.gameObject); // 這裡的1就是你要查詢的值
                if (id != -1) { p.GetComponent<PlayerAttack>().InBag.Remove(obj.gameObject); }

                Destroy(obj.gameObject);

            }
        }
        if (obj.gameObject.tag == "Player")
        {
            obj.gameObject.GetComponent<PlayerAttack>().TakeDamage(transform.position, FireDamage);

        }


    }
    void OnTriggerStay(Collider obj)
    {
        
    }
}
