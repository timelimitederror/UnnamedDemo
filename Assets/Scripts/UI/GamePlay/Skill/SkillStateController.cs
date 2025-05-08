using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStateController : MonoBehaviour
{
    private const float AVAILABLE_ALPHA = 1f;
    private const float UNAVAILABLE_ALPHA = 0.2f;

    public Image image;
    public TextMeshProUGUI tmp;

    public void setState(bool available)
    {
        if (available)
        {
            Color color = image.color;
            color.a = AVAILABLE_ALPHA;
            image.color = color;
        }
        else
        {
            Color color = image.color;
            color.a = UNAVAILABLE_ALPHA;
            image.color = color;
        }
    }

    public void setKeyPosition(string keyPosition)
    {
        tmp.SetText(keyPosition);
    }
}
