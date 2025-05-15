using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 石头撞击造成500伤害 撞到人就销毁，没撞到就等会继续撞
public class PolluroGenerateRockSkill : EnemySkillBase
{
    private const int DAMAGE = 500;
    private const float COOLDOWN_TIME = 5f;
    private const int ROCK_LIMIT = 5;

    private string redRockAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/SmallRockRed.prefab";
    private string greenRockAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/SmallRockGreen.prefab";
    private string blueRockAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/SmallRockBlue.prefab";
    private string redSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/RedRockDieSE.prefab";
    private string greenSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/GreenRockDieSE.prefab";
    private string blueSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Rock/BlueRockDieSE.prefab";
    private string soundAddress = "Audio/SoundEffect/EnemySound/PolluroSound/NewRock.wav";
    //private string rockSoundAddress = "";
    private MyObjectPool<GameObject> redPool;
    private MyObjectPool<GameObject> greenPool;
    private MyObjectPool<GameObject> bluePool;
    private GameObject redSEObj;
    private GameObject greenSEObj;
    private GameObject blueSEObj;
    private AudioClip sound;
    //private AudioClip rockSound;

    private PolluroController polluro;
    private PolluroSpecialEffectController seController;
    private PolluroAnimationController animationController;
    private GameObject armMark;

    public override bool Enable()
    {
        if (polluro == null || redPool == null || greenPool == null || bluePool == null)
        {
            return false;
        }
        if (polluro.cooldownTime > Time.fixedTime)
        {
            return false;
        }
        if (redPool.CountActive + greenPool.CountActive + bluePool.CountActive >= ROCK_LIMIT)
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

        AddressableAssetManager.Instance.loadAsset(redRockAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.redPool = new MyObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    PolluroRockController rockController = amm.GetComponent<PolluroRockController>();
                    if (rockController != null)
                    {
                        //  放入方法
                        rockController.setAction(otherCollider =>
                        {
                            if (otherCollider.gameObject.layer != LayerMask.NameToLayer("Player"))
                            {
                                return;
                            }
                            Damage(otherCollider.gameObject);
                            if (redSEObj != null)
                            {
                                redSEObj.transform.position = new Vector3(
                                    amm.transform.position.x,
                                    amm.transform.position.y - 0.5f,
                                    amm.transform.position.z);
                                redSEObj.GetComponent<ParticleSystem>().Play();
                            }
                            redPool.Release(amm);
                        });

                        rockController.setReleaseAction(() =>
                        {
                            redPool.Release(amm);
                        });

                        //rockController.SetMoveAction(() =>
                        //{
                        //    seController.PlayOneShot(rockSound);
                        //});
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

        AddressableAssetManager.Instance.loadAsset(greenRockAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.greenPool = new MyObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    PolluroRockController rockController = amm.GetComponent<PolluroRockController>();
                    if (rockController != null)
                    {
                        //  放入方法
                        rockController.setAction(otherCollider =>
                        {
                            if (otherCollider.gameObject.layer != LayerMask.NameToLayer("Player"))
                            {
                                return;
                            }
                            Damage(otherCollider.gameObject);
                            if (greenSEObj != null)
                            {
                                greenSEObj.transform.position = new Vector3(
                                    amm.transform.position.x,
                                    amm.transform.position.y - 0.5f,
                                    amm.transform.position.z);
                                greenSEObj.GetComponent<ParticleSystem>().Play();
                            }
                            greenPool.Release(amm);
                        });

                        rockController.setReleaseAction(() =>
                        {
                            greenPool.Release(amm);
                        });

                        //rockController.SetMoveAction(() =>
                        //{
                        //    seController.PlayOneShot(rockSound);
                        //});
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

        AddressableAssetManager.Instance.loadAsset(blueRockAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.bluePool = new MyObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    PolluroRockController rockController = amm.GetComponent<PolluroRockController>();
                    if (rockController != null)
                    {
                        //  放入方法
                        rockController.setAction(otherCollider =>
                        {
                            if (otherCollider.gameObject.layer != LayerMask.NameToLayer("Player"))
                            {
                                return;
                            }
                            Damage(otherCollider.gameObject);
                            if (blueSEObj != null)
                            {
                                blueSEObj.transform.position = new Vector3(
                                    amm.transform.position.x,
                                    amm.transform.position.y - 0.5f,
                                    amm.transform.position.z);
                                blueSEObj.GetComponent<ParticleSystem>().Play();
                            }
                            bluePool.Release(amm);
                        });

                        rockController.setReleaseAction(() =>
                        {
                            bluePool.Release(amm);
                        });

                        //rockController.SetMoveAction(() =>
                        //{
                        //    seController.PlayOneShot(rockSound);
                        //});
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

        AddressableAssetManager.Instance.loadAsset(redSEAddress, obj =>
        {
            redSEObj = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
        });

        AddressableAssetManager.Instance.loadAsset(greenSEAddress, obj =>
        {
            greenSEObj = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
        });

        AddressableAssetManager.Instance.loadAsset(blueSEAddress, obj =>
        {
            blueSEObj = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
        });

        AddressableAssetManager.Instance.loadAsset(soundAddress, obj =>
        {
            this.sound = (AudioClip)obj;
        });

        //AddressableAssetManager.Instance.loadAsset(rockSoundAddress, obj =>
        //{
        //    this.rockSound = (AudioClip)obj;
        //});
    }

    private void Damage(GameObject gameObject)
    {
        PlayerStateController player = gameObject.GetComponent<PlayerStateController>();
        if (player != null)
        {
            player.health -= DAMAGE;
            player.health = player.health < 0 ? 0 : player.health;
            player.setHealthUI();
            player.hitSound();
        }
    }

    public override void UninstallSkill()
    {
        redPool.Clear();
        greenPool.Clear();
        bluePool.Clear();
        AddressableAssetManager.Instance.destroyGameObject(redSEObj);
        AddressableAssetManager.Instance.destroyGameObject(greenSEObj);
        AddressableAssetManager.Instance.destroyGameObject(blueSEObj);

        AddressableAssetManager.Instance.releaseAsset(redRockAddress);
        AddressableAssetManager.Instance.releaseAsset(greenRockAddress);
        AddressableAssetManager.Instance.releaseAsset(blueRockAddress);
        AddressableAssetManager.Instance.releaseAsset(redSEAddress);
        AddressableAssetManager.Instance.releaseAsset(greenSEAddress);
        AddressableAssetManager.Instance.releaseAsset(blueSEAddress);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
        //AddressableAssetManager.Instance.releaseAsset(rockSoundAddress);
    }

    public override void UseSkill(PlayerStateController player)
    {
        if (redPool.CountActive + greenPool.CountActive + bluePool.CountActive >= ROCK_LIMIT)
        {
            return;
        }
        if (sound != null)
        {
            seController.PlayOneShot(sound);
        }
        animationController.Attack02();
        polluro.cooldownTime = Time.fixedTime + COOLDOWN_TIME;
        Vector3 dir = player.transform.position -= polluro.transform.position;
        polluro.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        Vector3 pos = randomPosition();
        if (pos == Vector3.zero)
        {
            return;
        }
        GameObject amm = null;
        switch (polluro.color)
        {
            case SkillColor.Red:
                amm = redPool.Get();
                break;
            case SkillColor.Green:
                amm = greenPool.Get();
                break;
            case SkillColor.Blue:
                amm = bluePool.Get();
                break;
            default:
                break;
        }
        if (amm == null)
        {
            return;
        }
        amm.transform.position = pos;
    }

    private Vector3 randomPosition()
    {
        int limit = 2000;
        Vector3 mark = armMark.transform.position;
        while (limit >= 0)
        {
            Vector3 random = new Vector3(
                mark.x + MyMathUtils.IntRandom(-10, 10),
                mark.y + MyMathUtils.IntRandom(-10, 10),
                mark.z + MyMathUtils.IntRandom(-10, 10));
            Collider[] results = Physics.OverlapSphere(random, 3f);
            if (results == null || results.Length == 0)
            {
                return random;
            }
            limit--;
        }
        return Vector3.zero;
    }
}
