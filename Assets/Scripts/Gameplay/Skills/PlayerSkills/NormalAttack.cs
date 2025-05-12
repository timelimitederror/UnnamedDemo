using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 普攻 当敌人与攻击同色时，造成10点伤害，获得10点与技能相同的颜料值
// 当敌人与攻击不同色时，造成5点伤害，获得5点与技能相同的颜料值
public class NormalAttack : PlayerSkill
{
    private const int DAMAGE_SUITABLE = 10;
    private const int DAMAGE_UNSUITABLE = 5;
    private const int COLOR_SUITABLE = 10;
    private const int COLOR_UNSUITABLE = 5;
    private const float COOLDOWN_TIME = 0.7f;
    private const float SPEED = 15f;
    private const float ACTIVE_TIME = 3f;

    private string redAmmAddress = "SpecialEffect/Skill/PlayerSkill/NormalAttack/NormalAttack_Red.prefab";
    private string greenAmmAddress = "SpecialEffect/Skill/PlayerSkill/NormalAttack/NormalAttack_Green.prefab";
    private string blueAmmAddress = "SpecialEffect/Skill/PlayerSkill/NormalAttack/NormalAttack_Blue.prefab";
    private string soundAddress = "Audio/SoundEffect/PlayerSound/Skill/normalAttack.wav";
    private ObjectPool<GameObject> redAmmPool;
    private ObjectPool<GameObject> greenAmmPool;
    private ObjectPool<GameObject> blueAmmPool;
    private AudioClip sound;

    private PlayerStateController playerController;
    private PlayerSpecialEffectController specialEffectController;
    private PlayerAnimationController playerAnimationController;

    public override bool enable()
    {
        if (playerController == null || redAmmPool == null || greenAmmPool == null || blueAmmPool == null || playerController.isFall())
        {
            return false;
        }
        if (playerController.cooldownTime > Time.fixedTime)
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

        AddressableAssetManager.Instance.loadAsset(redAmmAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.redAmmPool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    TriggerController triggerController = amm.GetComponent<TriggerController>();
                    if (triggerController != null)
                    {
                        //  放入方法
                        triggerController.setAction(otherCollider =>
                        {
                            EnemyControllerBase enemy = otherCollider.GetComponent<EnemyControllerBase>();
                            if (enemy != null)
                            {
                                if (enemy.getColor() == SkillColor.Red || enemy.getColor() == SkillColor.Mixing)
                                {
                                    enemy.damage(SkillColor.Red, DAMAGE_SUITABLE);
                                    playerController.redValue += COLOR_SUITABLE;
                                }
                                else
                                {
                                    enemy.damage(SkillColor.Red, DAMAGE_UNSUITABLE);
                                    playerController.redValue += COLOR_UNSUITABLE;

                                }
                                playerController.redValue = playerController.redValue > playerController.maxRed
                                        ? playerController.maxRed : playerController.redValue;
                                playerController.setColorValueUI();
                            }
                            redAmmPool.Release(amm);
                        });

                        triggerController.setReleaseAction(() =>
                        {
                            redAmmPool.Release(amm);
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

        AddressableAssetManager.Instance.loadAsset(greenAmmAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.greenAmmPool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    TriggerController triggerController = amm.GetComponent<TriggerController>();
                    if (triggerController != null)
                    {
                        //  放入方法
                        triggerController.setAction(otherCollider =>
                        {
                            EnemyControllerBase enemy = otherCollider.GetComponent<EnemyControllerBase>();
                            if (enemy != null)
                            {
                                if (enemy.getColor() == SkillColor.Green || enemy.getColor() == SkillColor.Mixing)
                                {
                                    enemy.damage(SkillColor.Green, DAMAGE_SUITABLE);
                                    playerController.greenValue += COLOR_SUITABLE;
                                }
                                else
                                {
                                    enemy.damage(SkillColor.Green, DAMAGE_UNSUITABLE);
                                    playerController.greenValue += COLOR_UNSUITABLE;
                                }
                                playerController.greenValue = playerController.greenValue > playerController.maxGreen
                                        ? playerController.maxGreen : playerController.greenValue;
                                playerController.setColorValueUI();
                            }
                            greenAmmPool.Release(amm);
                        });

                        triggerController.setReleaseAction(() =>
                        {
                            redAmmPool.Release(amm);
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

        AddressableAssetManager.Instance.loadAsset(blueAmmAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.blueAmmPool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    TriggerController triggerController = amm.GetComponent<TriggerController>();
                    if (triggerController != null)
                    {
                        //  放入方法
                        triggerController.setAction(otherCollider =>
                        {
                            EnemyControllerBase enemy = otherCollider.GetComponent<EnemyControllerBase>();
                            if (enemy != null)
                            {
                                if (enemy.getColor() == SkillColor.Blue || enemy.getColor() == SkillColor.Mixing)
                                {
                                    enemy.damage(SkillColor.Blue, DAMAGE_SUITABLE);
                                    playerController.blueValue += COLOR_SUITABLE;
                                }
                                else
                                {
                                    enemy.damage(SkillColor.Blue, DAMAGE_UNSUITABLE);
                                    playerController.blueValue += COLOR_UNSUITABLE;
                                }
                                playerController.blueValue = playerController.blueValue > playerController.maxBlue
                                        ? playerController.maxBlue : playerController.blueValue;
                                playerController.setColorValueUI();
                            }
                            blueAmmPool.Release(amm);
                        });

                        triggerController.setReleaseAction(() =>
                        {
                            redAmmPool.Release(amm);
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
        redAmmPool.Clear();
        greenAmmPool.Clear();
        blueAmmPool.Clear();
        AddressableAssetManager.Instance.releaseAsset(redAmmAddress);
        AddressableAssetManager.Instance.releaseAsset(greenAmmAddress);
        AddressableAssetManager.Instance.releaseAsset(blueAmmAddress);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
    }

    public override void useSkill()
    {
        SkillColor skillColor = playerController.color;
        GameObject amm = null;
        switch (skillColor)
        {
            case SkillColor.Red:
                amm = redAmmPool.Get();
                break;
            case SkillColor.Green:
                amm = greenAmmPool.Get();
                break;
            case SkillColor.Blue:
                amm = blueAmmPool.Get();
                break;
            default:
                break;
        }
        if (amm == null)
        {
            return;
        }

        Rigidbody rb = amm.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }


        Vector3 dir = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;
        playerController.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        Vector3 start = specialEffectController.getArmMark().transform.position;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
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
        playerAnimationController.skillLight();
        if (sound != null)
        {
            specialEffectController.playOneShot(sound);
        }
    }
}