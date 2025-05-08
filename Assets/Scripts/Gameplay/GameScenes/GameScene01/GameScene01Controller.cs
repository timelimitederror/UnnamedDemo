using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene01Controller : MonoBehaviour
{
    private const string BGM01 = "Audio/BGM/BGM_01.mp3";
    private AudioClip bgm01;

    void Awake()
    {
        AddressableAssetManager.Instance.loadAsset(BGM01, new Action<object>(loadAndPlayBgm01));
    }

    void Start()
    {
        if (bgm01 != null)
        {
            AudioManager.Instance.playBGM(bgm01);
        }
    }

    private void loadAndPlayBgm01(object obj)
    {
        bgm01 = (AudioClip)obj;
        AudioManager.Instance.playBGM(bgm01);
    }
}
