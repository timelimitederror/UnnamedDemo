using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroChangeColorSkill : EnemySkillBase
{
    private string soundAddress = "Audio/SoundEffect/EnemySound/PolluroSound/ChangeColor.wav";
    private string redSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/ChangeColor/ChangeColorRedSE.prefab";
    private string greenSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/ChangeColor/ChangeColorGreenSE.prefab";
    private string blueSEAddress = "SpecialEffect/Skill/EnemySkill/PolluroSkill/ChangeColor/ChangeColorBlueSE.prefab";
    private AudioClip sound;
    private GameObject redSE;
    private GameObject greenSE;
    private GameObject blueSE;
    private ParticleSystem redParticle;
    private ParticleSystem greenParticle;
    private ParticleSystem blueParticle;

    private PolluroController polluro;
    private PolluroSpecialEffectController seController;

    public override bool Enable()
    {
        if (polluro == null)
        {
            return false;
        }
        if (polluro.redHealth <= 0 && polluro.greenHealth <= 0 && polluro.blueHealth <= 0)// 死了不能用
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
            seController = polluro.gameObject.GetComponent<PolluroSpecialEffectController>();
        }
        else
        {
            return;
        }
        AddressableAssetManager.Instance.loadAsset(soundAddress, (obj) =>
        {
            this.sound = (AudioClip)obj;
        });
        AddressableAssetManager.Instance.loadAsset(redSEAddress, (obj) =>
        {
            this.redSE = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
            redParticle = redSE.transform.GetChild(0).GetComponent<ParticleSystem>();
            seController.AddToFootParticle(redSEAddress, redSE);
        });
        AddressableAssetManager.Instance.loadAsset(greenSEAddress, (obj) =>
        {
            this.greenSE = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
            greenParticle = greenSE.transform.GetChild(0).GetComponent<ParticleSystem>();
            seController.AddToFootParticle(greenSEAddress, greenSE);
        });
        AddressableAssetManager.Instance.loadAsset(blueSEAddress, (obj) =>
        {
            this.blueSE = AddressableAssetManager.Instance.instantiateGameObject((GameObject)obj);
            blueParticle = blueSE.transform.GetChild(0).GetComponent<ParticleSystem>();
            seController.AddToFootParticle(blueSEAddress, blueSE);
        });
    }

    public override void UninstallSkill()
    {
        seController.Remove(redSEAddress);
        seController.Remove(greenSEAddress);
        seController.Remove(blueSEAddress);
        AddressableAssetManager.Instance.destroyGameObject(redSE);
        AddressableAssetManager.Instance.destroyGameObject(greenSE);
        AddressableAssetManager.Instance.destroyGameObject(blueSE);
        AddressableAssetManager.Instance.releaseAsset(soundAddress);
        AddressableAssetManager.Instance.releaseAsset(redSEAddress);
        AddressableAssetManager.Instance.releaseAsset(greenSEAddress);
        AddressableAssetManager.Instance.releaseAsset(blueSEAddress);
    }

    public override void UseSkill(PlayerStateController player)
    {
        if (polluro.redHealth >= polluro.greenHealth && polluro.redHealth >= polluro.blueHealth)
        {
            polluro.ChangeColor(SkillColor.Red);
            if (redParticle != null)
            {
                redParticle.Play();
            }
        }
        else if (polluro.greenHealth >= polluro.blueHealth)
        {
            polluro.ChangeColor(SkillColor.Green);
            if (greenParticle != null)
            {
                greenParticle.Play();
            }
        }
        else
        {
            polluro.ChangeColor(SkillColor.Blue);
            if (blueParticle != null)
            {
                blueParticle.Play();
            }
        }
        if (sound != null)
        {
            seController.PlayOneShot(sound);
        }
    }
}
