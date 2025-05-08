using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void setStaminaValue(float value, float maxValue)
    {
        slider.SetValueWithoutNotify(value / maxValue);
    }
}
