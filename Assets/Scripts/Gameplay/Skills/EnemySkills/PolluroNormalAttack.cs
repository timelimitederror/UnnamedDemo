using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 普攻，造成200点伤害
public class PolluroNormalAttack : EnemySkillBase
{
    private const int DAMAGE = 200;
    private const float COOLDOWN_TIME = 5f;
    private const float SPEED = 30f;
    private const float ACTIVE_TIME = 3f;

    private string ammAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/NormalAttack/PolluroNA.prefab";
    private string soundAddress = "Audio/SoundEffect/EnemySound/PolluroSound/NA.wav";
    private ObjectPool<GameObject> ammPool;
    private AudioClip sound;

    private PolluroController polluro;
    private PolluroSpecialEffectController seController;
    private PolluroAnimationController animationController;
    private GameObject armMark;

    public override bool Enable()
    {
        if (polluro == null || ammPool == null)
        {
            return false;
        }
        if (polluro.cooldownTime > Time.fixedTime)
        {
            return false;
        }
        return true;
    }

    public override void InstallSkill(EnemyControllerBase enemyController)
    {
        if (enemyController is PolluroController)
        {
            polluro = (PolluroController)enemyController;
        }
        else
        {
            return;
        }
        seController = polluro.gameObject.GetComponent<PolluroSpecialEffectController>();
        animationController = polluro.gameObject.GetComponent<PolluroAnimationController>();
        armMark = seController.GetArmMark();

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
                            PlayerStateController player = otherCollider.GetComponent<PlayerStateController>();
                            if (player != null)
                            {
                                player.health -= DAMAGE;
                                player.health = player.health < 0 ? 0 : player.health;
                                player.setHealthUI();
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

        AddressableAssetManager.Instance.loadAsset(soundAddress, obj =>
        {
            this.sound = (AudioClip)obj;
        });
    }

    public override void UninstallSkill()
    {
        ammPool.Clear();
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
        AddressableAssetManager.Instance.releaseAsset(ammAddress);
    }

    public override void UseSkill(PlayerStateController player)
    {
        GameObject amm = ammPool.Get();
        Rigidbody rb = amm.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }

        Vector3 playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z);
        Vector3 start = armMark.transform.position;
        Vector3 dir = (playerPosition - start).normalized;
        polluro.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        amm.transform.position = start;
        amm.transform.rotation = Quaternion.LookRotation(dir);
        rb.velocity = dir * SPEED;

        polluro.cooldownTime = Time.fixedTime + COOLDOWN_TIME;

        animationController.Attack01();
        if (sound != null)
        {
            seController.PlayOneShot(sound);
        }
    }
}
