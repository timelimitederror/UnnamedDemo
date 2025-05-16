using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPanelController : MonoBehaviour
{
    public GameObject loadingPanel;

    void Start()
    {
        EventBus.Subscribe(new Action<SceneLoading>(sceneEvent =>
        {
            if (sceneEvent.isLoading)
            {
                loadingPanel.SetActive(true);
            }
            else
            {
                loadingPanel.SetActive(false);
            }
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
