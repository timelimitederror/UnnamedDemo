using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KPButtomController : MonoBehaviour
{
    public string keyName;
    public TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSkillKeyPositionChanged kp = InitManager.Instance.GetKeyPosition();
        switch (keyName)
        {
            case InitManager.SKILL_1:
                tmp.SetText(kp.skill_1);
                break;
            case InitManager.SKILL_2:
                tmp.SetText(kp.skill_2);
                break;
            case InitManager.SKILL_3:
                tmp.SetText(kp.skill_3);
                break;
            case InitManager.SKILL_4:
                tmp.SetText(kp.skill_4);
                break;
            case InitManager.MIXING_COLOR:
                tmp.SetText(kp.mixing);
                break;
            case InitManager.SWITCH_COLOR:
                tmp.SetText(kp.armColor);
                break;
            default:
                tmp.SetText("Error");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            DetectKeyPress();
        }
    }

    private void DetectKeyPress()
    {
        if (!Input.anyKeyDown) return;
        foreach (KeyCode keyCode in PlayerSkillKeyPositionChanged.mappingTable.Keys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                tmp.SetText(PlayerSkillKeyPositionChanged.mappingTable[keyCode]);
                InitManager.Instance.ReplaceKeyPosition(keyName, keyCode);
                EventSystem.current.SetSelectedGameObject(null);
                break;
            }
        }
    }
}
