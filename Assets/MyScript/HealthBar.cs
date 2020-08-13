using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public bool HideInStandby;
    [Header("")]
    public Slider sli;
    public float smooth = 5.0F;
    public float h;
    public float lasth;
    RectTransform rectTransform;
    Vector2 initalSizeDelta;
    //[ColorUsage(true, true)]
    Color initalColor;
    public Color plusHealthColor = new Color32(255, 103, 16, 255);
    public Color lessHealthColor = new Color32(206, 233, 255, 255); 
    public Image HealthBarImage;

    public float time = 0;
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
       
        if (HideInStandby)
        {
            if (h == sli.value && time % 300 == 0)
            {
                time = 0;
                rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, new Vector2(rectTransform.sizeDelta.x, 0), Time.deltaTime * smooth);
            }
            else if (rectTransform.sizeDelta != initalSizeDelta)
            {
                
                time += 1;
                rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, initalSizeDelta, Time.deltaTime * smooth);
            }
            
        }
        else
        {
            if (sli.value == sli.maxValue)
            {
                rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, new Vector2(rectTransform.sizeDelta.x, 0), Time.deltaTime * smooth);
            }
            else if (rectTransform.sizeDelta != initalSizeDelta && sli.value != sli.maxValue)
            {
                rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, initalSizeDelta, Time.deltaTime * smooth);
            }
        }
    }
    
   
    void Awake()
    {
        initalColor = HealthBarImage.color;
        //sli.value = health;
        //h = health;
        rectTransform = GetComponent<RectTransform>();
        initalSizeDelta = rectTransform.sizeDelta;
        sli.value = 0;
    }
}
