using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuObj : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnMouseDown()
    {
        rb.velocity = new Vector3();
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        rb.velocity = new Vector3();
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
    void OnMouseUp()
    {
        //rb.useGravity = true;

    }
}