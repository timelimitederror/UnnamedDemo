using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 单体，召唤大型混色炮弹，直接摧毁一个单体有色岩石（对非岩石敌人伤害很低）
public class SingleAttackSkill01 : PlayerSkill
{
    private const int MIXING_COST = 20;
    private const int DAMAGE_ROCK = 1000000;
    private const int DAMAGE_LIFE = 100;
    private const float COOLDOWN_TIME = 3f;
    private const float SPEED = 15f;
    private const float ACTIVE_TIME = 3f;

    private string ammAddress = "SpecialEffect/Skill/PlayerSkill/SingleAttack01/SingleAttackSkill01SE.prefab";
    private string soundAddress = "Audio/SoundEffect/PlayerSound/Skill/danti01.wav";
    private AudioClip sound;
    private ObjectPool<GameObject> ammPool;

    private PlayerStateController playerController;
    private PlayerSpecialEffectController specialEffectController;
    private PlayerAnimationController playerAnimationController;

    public override bool enable()
    {
        if (playerController == null || ammPool == null || playerController.isFall())
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
        this.playerAnimationController = playerController.gameObject.GetComponent<PlayerAnimationController>();

        AddressableAssetManager.Instance.loadAsset(ammAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.ammPool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    TriggerController triggerController = amm.GetComponent<TriggerController>();
                    if (triggerController != null)
                    {
                        //  放入方法
                        triggerController.setAction(otherCollider =>
                        {
                            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
                            {
                                return;
                            }
                            EnemyControllerBase enemy = otherCollider.GetComponent<EnemyControllerBase>();
                            if (enemy != null)
                            {
                                if (enemy.getEnemyType() == EnemyControllerBase.EnemyType.Rock)
                                {
                                    enemy.damage(SkillColor.Mixing, DAMAGE_ROCK);
                                }
                                else
                                {
                                    enemy.damage(SkillColor.Mixing, DAMAGE_LIFE);
                                }
                            }
                            ammPool.Release(amm);
                        });

                        triggerController.setReleaseAction(() =>
                        {
                            ammPool.Release(amm);
                        });

                        triggerController.setActiveTime(ACTIVE_TIME);
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
        ammPool.Clear();
        AddressableAssetManager.Instance.releaseAsset(ammAddress);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
    }

    public override void useSkill()
    {
        GameObject amm = ammPool.Get();

        Rigidbody rb = amm.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        playerController.mixingValue -= MIXING_COST;

        Vector3 dir = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;
        playerController.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        Vector3 start = specialEffectController.getArmMark().transform.position;

        RaycastHit[] hitInfos = Physics.RaycastAll(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
        foreach (RaycastHit hitInfo in hitInfos)
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                dir = new Vector3(hitInfo.point.x - start.x, hitInfo.point.y - start.y, hitInfo.point.z - start.z).normalized;
                break;
            }
        }

        amm.transform.position = start;


        // Debug.DrawRay(start, dir, Color.red, 10f);
        rb.velocity = dir * SPEED;

        playerController.cooldownTime = Time.fixedTime + COOLDOWN_TIME;
        playerController.setColorValueUI();
        playerAnimationController.skillMedium();
        if (sound != null)
        {
            specialEffectController.playOneShot(sound);
        }
    }
}
