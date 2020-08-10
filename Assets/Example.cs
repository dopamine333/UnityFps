using UnityEngine;

public class Example : MonoBehaviour
{
    Collider m_Collider;
    Vector3 m_Size;

    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider>();

        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;

        //Output to the console the size of the Collider volume
        Debug.Log("Collider Size : " + m_Size);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            m_Collider = GetComponent<Collider>();
            m_Size = m_Collider.bounds.size;

            //Debug.Log("Collider Size : " + m_Size);
        }
    }
}