using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelColorController : MonoBehaviour
{
    public GameObject red;
    public GameObject green;
    public GameObject blue;

    public void SetColor(SkillColor color)
    {
        switch (color)
        {
            case SkillColor.Red:
                red.SetActive(true);
                green.SetActive(false);
                blue.SetActive(false);
                break;
            case SkillColor.Green:
                green.SetActive(true);
                blue.SetActive(false);
                red.SetActive(false);
                break;
            case SkillColor.Blue:
                blue.SetActive(true);
                green.SetActive(false);
                red.SetActive(false);
                break;
            default:
                break;
        }
    }
}
