using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneController : MonoBehaviour
{
    private const string BGM03 = "Audio/BGM/BGM_03.mp3";
    private AudioClip bgm03;

    void Awake()
    {
        AddressableAssetManager.Instance.loadAsset(BGM03, new Action<object>(loadAndPlayBgm03));
    }

    void Start()
    {
        if (bgm03 != null)
        {
            AudioManager.Instance.playBGM(bgm03);
        }
    }

    private void OnDestroy()
    {
        AddressableAssetManager.Instance.releaseAsset(BGM03);
    }

    private void loadAndPlayBgm03(object obj)
    {
        bgm03 = (AudioClip)obj;
        AudioManager.Instance.playBGM(bgm03);
    }
}
