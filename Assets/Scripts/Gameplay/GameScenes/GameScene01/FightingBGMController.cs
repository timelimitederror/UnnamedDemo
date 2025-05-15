using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingBGMController : MonoBehaviour
{
    private string bgmAddress = "Audio/BGM/BGM_02.mp3";
    private AudioClip bgm;

    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;

        AddressableAssetManager.Instance.loadAsset(bgmAddress, obj =>
        {
            bgm = obj as AudioClip;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bgm != null)
        {
            AudioManager.Instance.playBGM(bgm);
        }
        boxCollider.enabled = false;
    }

    private void OnDestroy()
    {
        AddressableAssetManager.Instance.releaseAsset(bgmAddress);
    }
}
