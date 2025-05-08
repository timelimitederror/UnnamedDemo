using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallPanelController : MonoBehaviour
{
    public GraphicsController graphicsController;
    public KeyPositionController keyPositionController;
    public VoiceController voiceController;

    private const string UISOUND = "Audio/SoundEffect/UI/UI01.wav";
    private AudioClip audioClip;

    void Start()
    {
        AddressableAssetManager.Instance.loadAsset(UISOUND, new Action<object>(setAudioClip));
    }

    private void setAudioClip(object obj)
    {
        audioClip = (AudioClip)obj;
    }

    public void playOneShot()
    {
        if (audioClip != null)
        {
            UIManager.Instance.playOneShot(audioClip);
        }
    }

    public void closeInstallPanel()
    {
        gameObject.SetActive(false);
    }

    public void openVoiceInit()
    {
        graphicsController.close();
        keyPositionController.close();
        voiceController.open();
    }

    public void openKeyPositionInit()
    {
        voiceController.close();
        keyPositionController.open();
        graphicsController.close();
    }

    public void openGraphicsInit()
    {
        voiceController.close();
        keyPositionController.close();
        graphicsController.open();
    }

}
