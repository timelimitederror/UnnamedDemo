using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPanelController : MonoBehaviour
{
    private const string UISOUND = "Audio/SoundEffect/UI/UI01.wav";
    private AudioClip audioClip;

    void Start()
    {
        AddressableAssetManager.Instance.loadAsset(UISOUND, new Action<object>(setAudioClip));
    }

    // 退出游戏
    public void quit()
    {
        SystemManager.Instance.closeGame();
    }

    // 设置
    public void openInstallPanel()
    {
        UIManager.Instance.openPanel("InstallPanel");
    }

    // 开始游戏（还没接入存档
    public void startGame()
    {
        UIManager.Instance.startGame();
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
}
