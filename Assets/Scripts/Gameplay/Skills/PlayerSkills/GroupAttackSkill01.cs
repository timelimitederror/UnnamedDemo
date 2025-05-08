using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 群攻技能，-30混色值，对范围内所有敌方造成伤害 
public class GroupAttackSkill01 : PlayerSkill
{
    private const int MIXING_COST = 30;
    private const int DAMAGE = 100;
    private const float RADIUS_P = 43f;
    private const float HORIZONTAL_DISTANCE = 30f;
    private const float VERTICAL_DISTANCE = 30f;
    private const float COOLDOWN_TIME = 5f;
    private const float ACTIVE_TIME = 7f;

    private LayerMask enemyLayer = LayerMask.GetMask("Enemy");// Enemy
    private string specialEffectAddress = "SpecialEffect/Skill/PlayerSkill/GroupAttack01/GroupAttackSkill01SE.prefab";
    private string soundAddress = "Audio/SoundEffect/PlayerSound/Skill/qungong.wav";
    private ObjectPool<GameObject> sePool;
    private AudioClip sound;

    private PlayerStateController playerController;
    private Vector3 lastUsedPosition;
    private PlayerSpecialEffectController specialEffectController;
    private PlayerAnimationController playerAnimationController;


    public override bool enable()
    {
        if (playerController == null || sePool == null)
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
        this.playerAnimationController = playerController.gameObject.AddComponent<PlayerAnimationController>();
        // 加载特效预制体
        AddressableAssetManager.Instance.loadAsset(specialEffectAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.sePool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    SpecialEffectController seController = amm.GetComponent<SpecialEffectController>();
                    if (seController != null)
                    {
                        //  放入方法
                        seController.setReleaseAction(() =>
                        {
                            sePool.Release(amm);
                        });

                        seController.setActiveTime(ACTIVE_TIME);
                    }

                    return amm;
                }, obj =>
                {
                    obj.SetActive(true);
                }, obj =>
                {
                    obj.SetActive(false);
                }, obj =>
                {
                    AddressableAssetManager.Instance.destroyGameObject(obj);
                }, true, 10, 1000);
        });
        AddressableAssetManager.Instance.loadAsset(soundAddress, new Action<object>(setSound));
    }

    private void setSound(object obj)
    {
        this.sound = (AudioClip)obj;
    }

    public override void uninstallSkill()
    {
        // 卸载特效预制体资源
        sePool.Clear();
        AddressableAssetManager.Instance.releaseAsset(specialEffectAddress);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
    }

    public override void useSkill()
    {
        playerController.mixingValue -= MIXING_COST;
        lastUsedPosition = playerController.transform.position;
        playerController.addScheduleTask(new PlayerStateController.ScheduleTask(4f, 1, new Action(damage)));

        // 更新冷却时间
        playerController.cooldownTime = Time.fixedTime + COOLDOWN_TIME;

        playerController.setColorValueUI();
        playerAnimationController.skillHeavy();

        // 播放特效
        GameObject gameObject = sePool.Get();
        SpecialEffectController seController = gameObject.GetComponent<SpecialEffectController>();
        if (seController != null)
        {
            seController.transform.position = lastUsedPosition;
            seController.playSE();
        }
        else
        {
            sePool.Release(gameObject);
        }
        if (sound != null)
        {
            specialEffectController.playOneShot(sound);

        }

    }

    private void damage()
    {
        Collider[] results = Physics.OverlapSphere(
            lastUsedPosition,
            RADIUS_P,
            enemyLayer);
        foreach (Collider coll in results)
        {
            EnemyControllerBase enemy = coll.gameObject.GetComponent<EnemyControllerBase>();
            if (enemy != null
                && MyMathUtils.horizontalDistance(lastUsedPosition, enemy.transform.position) <= HORIZONTAL_DISTANCE
                && MyMathUtils.verticalDistance(lastUsedPosition, enemy.transform.position) <= VERTICAL_DISTANCE)
            {
                enemy.damage(SkillColor.Mixing, DAMAGE);
            }
        }

    }
}
