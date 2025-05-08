using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestDebug : MonoBehaviourSinqletonBase<TestDebug>
{
    public TextMeshProUGUI tmp;

    public void log(string text)
    {
        tmp.gameObject.SetActive(true);
        tmp.SetText(text);
    }
}
