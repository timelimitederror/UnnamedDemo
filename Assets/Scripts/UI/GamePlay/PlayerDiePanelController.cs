using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiePanelController : MonoBehaviour
{
    private const string UISOUND = "Audio/SoundEffect/UI/UI01.wav";
    private AudioClip audioClip;

    private GameObject panel;
    private Action<PlayerDie> action;

    void Start()
    {
        AddressableAssetManager.Instance.loadAsset(UISOUND, new Action<object>(obj =>
        {
            this.audioClip = obj as AudioClip;
        }));

        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);

        action = new Action<PlayerDie>(playerEvent =>
        {
            panel.SetActive(true);
        });
        EventBus.Subscribe(action);
    }

    void OnEnable()
    {
        panel?.SetActive(false);
    }

    void OnDisable()
    {
        panel?.SetActive(false);
    }

    public void ReturnLoginScene()
    {
        panel?.SetActive(false);
        UIManager.Instance.returnLoginScene();
    }

    public void PlayOneShot()
    {
        if (audioClip != null)
        {
            UIManager.Instance.playOneShot(audioClip);
        }
    }
}
