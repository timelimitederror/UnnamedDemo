using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI tmp;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void setHealthValue(int value, int maxValue)
    {
        if (value == 0)
        {
            slider.SetValueWithoutNotify(0f);
            tmp.SetText("0.0%");
        }
        else
        {
            float percentage = (float)value / (float)maxValue;
            //slider.SetValueWithoutNotify(percentage);
            slider.normalizedValue = percentage;
            percentage *= 100;
            float num = percentage + 0.1f >= 100f ? percentage : percentage + 0.1f;
            tmp.SetText(num.ToString("0.0") + "%");
        }
    }
}
