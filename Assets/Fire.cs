using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float FireDamage=2f;


    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.GetComponent<ObjData>() != null)
        {
            if (obj.gameObject.GetComponent<ObjData>().AfterCooking != null)
            {
                GameObject AfterCookingObj = Instantiate(obj.gameObject.GetComponent<ObjData>().AfterCooking, obj.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
                AfterCookingObj.GetComponent<Rigidbody>().isKinematic = false;
                AfterCookingObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                AfterCookingObj.GetComponent<Rigidbody>().AddForce(Vector3.up * Time.fixedDeltaTime * 300, ForceMode.Impulse);

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
