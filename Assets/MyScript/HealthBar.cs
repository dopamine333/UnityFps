using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider sli;
   
    public void SetMixHealth(float health)
    {
        sli.maxValue = health;
        sli.value = health;
    }
    public void SetHealth(float health)
    {
        sli.value = health;
    }
}
