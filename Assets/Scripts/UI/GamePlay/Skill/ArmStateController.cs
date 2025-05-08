using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmStateController : MonoBehaviour
{
    private const float AVAILABLE_ALPHA = 1f;
    private const float UNAVAILABLE_ALPHA = 0.2f;

    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public TextMeshProUGUI tmp;
    public Image image;

    public void setColor(SkillColor color)
    {
        switch (color)
        {
            case SkillColor.Red:
                red.SetActive(true);
                blue.SetActive(false);
                green.SetActive(false);
                break;
            case SkillColor.Green:
                green.SetActive(true);
                blue.SetActive(false);
                red.SetActive(false);
                break;
            case SkillColor.Blue:
                blue.SetActive(true);
                red.SetActive(false);
                green.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void setKeyPosition(string keyPosition)
    {
        tmp.SetText(keyPosition);
    }

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
}
