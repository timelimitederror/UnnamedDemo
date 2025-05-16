using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene01Controller : MonoBehaviour
{
    public GameObject grayEnvironment;
    public GameObject colorEnvironment;
    public GameObject boss;

    private const string BGM01 = "Audio/BGM/BGM_01.mp3";
    private const string BGM03 = "Audio/BGM/BGM_03.mp3";
    private AudioClip bgm01;
    private AudioClip bgm03;
    private Action<GameScene01Victory> victoryAction;
    private bool isVictory = false;
    private float bossDisappearTime = 0f;

    void Awake()
    {
        UIManager.Instance.GameScene01UIActive();
        AddressableAssetManager.Instance.loadAsset(BGM01, new Action<object>(loadAndPlayBgm01));
        AddressableAssetManager.Instance.loadAsset(BGM03, obj =>
        {
            this.bgm03 = (AudioClip)obj;
        });
    }

    void Start()
    {
        if (bgm01 != null)
        {
            AudioManager.Instance.playBGM(bgm01);
        }
        grayEnvironment.SetActive(true);
        colorEnvironment.SetActive(false);

        victoryAction = new Action<GameScene01Victory>(sceneEvent =>
        {
            AudioManager.Instance.playBGM(bgm03);
            isVictory = true;
            bossDisappearTime = Time.fixedTime + 1.5f;
        });

        EventBus.Subscribe(victoryAction);
    }

    void Update()
    {
        if (isVictory && Time.fixedTime >= bossDisappearTime)
        {
            boss.SetActive(false);
            colorEnvironment.SetActive(true);
            grayEnvironment.SetActive(false);
        }
    }

    void OnDestroy()
    {
        AddressableAssetManager.Instance.releaseAsset(BGM01);
        AddressableAssetManager.Instance.releaseAsset(BGM03);
    }

    private void loadAndPlayBgm01(object obj)
    {
        bgm01 = (AudioClip)obj;
        AudioManager.Instance.playBGM(bgm01);
    }
}
