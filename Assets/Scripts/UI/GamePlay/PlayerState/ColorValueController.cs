using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorValueController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI tmp;

    public void setColorValue(int value, int maxValue)
    {
        slider.SetValueWithoutNotify((float)value / (float)maxValue);
        tmp.SetText(value.ToString());
    }
}
