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

    // �浵����û��
    public void saveArchive()
    {
        Debug.Log(Time.timeScale);
    }

    // ����
    public void openInstallPanel()
    {
        UIManager.Instance.openPanel("InstallPanel");
    }

    // ���ؿ�ʼ�˵�
    public void returnLoginScene()
    {
        TimeManager.Instance.startTime();
        UIManager.Instance.returnLoginScene();
    }

    // �˳���Ϸ
    public void closeGame()
    {
        SystemManager.Instance.closeGame();
    }
}
