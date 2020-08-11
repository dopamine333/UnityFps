using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider sli;
    public float smooth = 5.0F;
    float h;

    //[ColorUsage(true, true)]
    Color initalColor;
    public Color plusHealthColor = new Color32(255, 103, 16, 255);
    public Color lessHealthColor = new Color32(206, 233, 255, 255); 
    public Image HealthBarImage;

    public void SetMixHealth(float health)
    {
        sli.maxValue = health;
    }
    public void SetHealth(float health)
    {
        h = health;
        if(h > sli.value)
            HealthBarImage.color = plusHealthColor;
        else
            HealthBarImage.color = lessHealthColor;

    }

    //sli.value = health;

    void Update()
    {
        if ((h - 0.1f < sli.value) && (sli.value < h + 0.1f))
        {
            sli.value = h;
        }
        else
        {
            HealthBarImage.color = Color.Lerp(HealthBarImage.color, initalColor, Time.deltaTime * smooth);
            sli.value = Mathf.Lerp(sli.value, h, Time.deltaTime * smooth);
        }
        
    }
    void Awake()
    {
        initalColor = HealthBarImage.color;
        //sli.value = health;
        //h = health;
    }
}
