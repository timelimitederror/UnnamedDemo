using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// ���Ƽ��ܣ�����20��ɫֵ�ָ��������20%��Ѫ��
public class HealingSkill01 : PlayerSkill
{
    private const int MIXING_COST = 20;
    private const int RESTORE_HEALTH = 20;
    private const float COOLDOWN_TIME = 1.5f;

    private string specialEffectAddress = "SpecialEffect/Skill/PlayerSkill/HealingSkill01/HealingSkill01SE.prefab";
    private string soundAddress = "Audio/SoundEffect/PlayerSound/Skill/02_Heal_02.wav";
    private GameObject specialEffect;
    private AudioClip sound;
    private ParticleSystem specialEffectParticle;

    private PlayerStateController playerController;
    private PlayerSpecialEffectController specialEffectController;

    // ��ɫ���������ȴʱ�䲻��ʹ��
    public override bool enable()
    {
        if (playerController == null)
        {
            return false;
        }
        if (playerController.cooldownTime > Time.fixedTime)
        {
            return false;
        }
        if (playerController.mixingValue < MIXING_COST)
        {
            return false;
        }
        return true;
    }

    public override void installSkill(PlayerStateController playerController)
    {
        this.playerController = playerController;
        this.specialEffectController = playerController.gameObject.GetComponent<PlayerSpecialEffectController>();
        // ������ЧԤ����
        AddressableAssetManager.Instance.loadAsset(specialEffectAddress, new Action<object>(setSpecialEffect));
        AddressableAssetManager.Instance.loadAsset(soundAddress, new Action<object>(setSound));
    }

    private void setSpecialEffect(object obj)
    {
        this.specialEffect = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
        specialEffectParticle = specialEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
        specialEffectController.addToFootParticle(specialEffectAddress, specialEffect);
    }

    private void setSound(object obj)
    {
        this.sound = (AudioClip)obj;
    }

    public override void uninstallSkill()
    {
        // ж����ЧԤ������Դ
        AddressableAssetManager.Instance.destroyGameObject(specialEffect);
        AddressableAssetManager.Instance.releaseAsset(specialEffectAddress);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
    }

    public override void useSkill()
    {
        playerController.mixingValue -= MIXING_COST;
        playerController.health += (playerController.maxHealth * RESTORE_HEALTH) / 100;
        playerController.health = playerController.health > playerController.maxHealth ?
            playerController.maxHealth : playerController.health;

        // ������ȴʱ��
        playerController.cooldownTime = Time.fixedTime + COOLDOWN_TIME;

        playerController.setColorValueUI();
        playerController.setHealthUI();
        // ������Ч
        if (specialEffectParticle != null)
        {
            specialEffectParticle.Play();
        }
        if (sound != null)
        {
            specialEffectController.playOneShot(sound);

        }
    }
}
