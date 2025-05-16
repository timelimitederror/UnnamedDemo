using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI tmp;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void setHealthValue(int value, int maxValue)
    {
        
        slider.normalizedValue = (float)value / (float)maxValue;
        tmp.SetText(value + " / " + maxValue);
    }
}
