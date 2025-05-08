using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 混色，消耗10R,10G,10B获得10混色值
public class MixingColor : PlayerSkill
{
    private const int RED_COST = 10;
    private const int GREEN_COST = 10;
    private const int BLUE_COST = 10;
    private const int MIXING_VALUE = 10;

    private string soundAddress = "Audio/SoundEffect/PlayerSound/Skill/mixing.wav";
    private AudioClip sound;


    private PlayerStateController playerController;
    private PlayerSpecialEffectController specialEffectController;

    // RGB不足或mixing达到上限则无法使用
    public override bool enable()
    {
        if (playerController == null)
        {
            return false;
        }
        if (playerController.redValue < RED_COST)
        {
            return false;
        }
        if (playerController.greenValue < GREEN_COST)
        {
            return false;
        }
        if (playerController.blueValue < BLUE_COST)
        {
            return false;
        }
        if (playerController.mixingValue >= playerController.maxMixing)
        {
            return false;
        }
        return true;
    }

    public override void installSkill(PlayerStateController playerController)
    {
        this.playerController = playerController;
        this.specialEffectController = playerController.gameObject.GetComponent<PlayerSpecialEffectController>();
        // 加载特效预制体
        AddressableAssetManager.Instance.loadAsset(soundAddress, new Action<object>(setSound));
    }

    private void setSound(object obj)
    {
        this.sound = (AudioClip)obj;
    }

    public override void uninstallSkill()
    {
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
    }

    // R-10 G-10 B-10 Mixing+10
    public override void useSkill()
    {
        playerController.redValue -= RED_COST;
        playerController.greenValue -= GREEN_COST;
        playerController.blueValue -= BLUE_COST;
        playerController.mixingValue = playerController.mixingValue + MIXING_VALUE;
        playerController.mixingValue = playerController.mixingValue > playerController.maxMixing ?
            playerController.maxMixing : playerController.mixingValue;

        playerController.setColorValueUI();

        if (sound != null)
        {
            specialEffectController.playOneShot(sound);

        }
    }
}
