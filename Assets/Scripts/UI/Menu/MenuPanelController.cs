using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelController : MonoBehaviour
{
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

    void OnEnable()
    {
        TimeManager.Instance.stopTime();
    }

    public void closeMenuPanel()
    {
        TimeManager.Instance.startTime();
        gameObject.SetActive(false);
    }

    // 存档（还没做
    public void saveArchive()
    {
        Debug.Log(Time.timeScale);
    }

    // 设置
    public void openInstallPanel()
    {
        UIManager.Instance.openPanel("InstallPanel");
    }

    // 返回开始菜单
    public void returnLoginScene()
    {
        TimeManager.Instance.startTime();
        UIManager.Instance.returnLoginScene();
    }

    // 退出游戏
    public void closeGame()
    {
        SystemManager.Instance.closeGame();
    }
}
