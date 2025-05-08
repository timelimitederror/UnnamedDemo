using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    public GameObject bgImame;

    public GameObject panel;

    public void open()
    {
        bgImame.SetActive(true);
        panel.SetActive(true);
    }

    public void close()
    {
        bgImame.SetActive(false);
        panel.SetActive(false);
    }
}
