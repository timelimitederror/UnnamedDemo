using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 生成随机爆炸区域，4秒预警后爆炸 伤害致死
public class PolluroExplodeSkill : EnemySkillBase
{
    private const int DAMAGE = 1000000;
    private const float COOLDOWN_TIME = 10f;
    private const int BALL_LIMIT = 10;
    private const float ACTIVE_TIME = 5f;

    private string ammAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/Explode/ExplodeSE.prefab";
    //private string soundAddress = "";
    //private string explodeSoundAddress = "";
    private ObjectPool<GameObject> ammPool;
    //private AudioClip sound;
    //private AudioClip explodeSound;

    private PolluroController polluro;
    private PolluroSpecialEffectController seController;
    private PolluroAnimationController animationController;
    private GameObject footMark;

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
        footMark = seController.GetFootMark();

        AddressableAssetManager.Instance.loadAsset(ammAddress, obj =>
        {
            GameObject gameObject = obj as GameObject;
            this.ammPool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject amm = AddressableAssetManager.Instance.instantiateGameObject(gameObject);
                    PolluroExplodeTriggerController explodeController = amm.GetComponent<PolluroExplodeTriggerController>();
                    if (explodeController != null)
                    {
                        //  放入方法
                        explodeController.setAction(otherCollider =>
                        {
                            PlayerStateController player = otherCollider.gameObject.GetComponent<PlayerStateController>();
                            if (player != null)
                            {
                                player.health -= DAMAGE;
                                player.health = player.health < 0 ? 0 : player.health;
                                player.setHealthUI();
                                player.hitSound();
                            }
                        });

                        explodeController.setReleaseAction(() =>
                        {
                            ammPool.Release(amm);
                        });

                        explodeController.setActiveTime(ACTIVE_TIME);
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
    }

    public override void UninstallSkill()
    {
        ammPool.Clear();
        AddressableAssetManager.Instance.releaseAsset(ammAddress);
        //AddressableAssetManager.Instance.releaseAsset(soundAddress);
        //AddressableAssetManager.Instance.releaseAsset(explodeSoundAddress);
    }

    public override void UseSkill(PlayerStateController player)
    {
        //if (sound != null)
        //{
        //    seController.PlayOneShot(sound);
        //}
        animationController.Attack03();
        polluro.cooldownTime = Time.fixedTime + COOLDOWN_TIME;
        Vector3 dir = player.transform.position -= polluro.transform.position;
        polluro.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        List<Vector3> posList = new List<Vector3>();
        Vector3 mark = footMark.transform.position;
        for (int i = 0; i < BALL_LIMIT; i++) // 随机生成BALL_LIMIT个爆炸区位置，挨个塞上爆炸特效，每个爆炸区之间距离必须大于18
        {
            int limit = 2000;
            while (limit >= 0)
            {
                Vector3 random = new Vector3(
                    mark.x + MyMathUtils.IntRandom(-35, 35),
                    mark.y + MyMathUtils.IntRandom(-3, 5),
                    mark.z + MyMathUtils.IntRandom(-35, 35)
                    );
                // se R=8+
                int j = 0;
                for (; j < posList.Count; j++)
                {
                    if (Vector3.Distance(posList[j], random) < 18f)
                    {
                        break;
                    }
                }
                if (j == posList.Count)
                {
                    posList.Add(random);
                    break;
                }
                limit--;
            }
        }
        foreach (Vector3 pos in posList)
        {
            GameObject amm = ammPool.Get();
            if(amm == null)
            {
                continue;
            }
            amm.transform.position = pos;
        }
    }
}
