using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // 上半身：技能>跳跃=掉落>跑步>待机
    // 下半身：跳跃=掉落>跑步>技能>待机
    private Animator animator;
    private bool isFall = false;
    private PlayerSpecialEffectController specialEffectController;

    private string runSoundAddress = "Audio/SoundEffect/PlayerSound/Movement/08_Step_rock_02.wav";
    private AudioClip runSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        specialEffectController = GetComponent<PlayerSpecialEffectController>();

        AddressableAssetManager.Instance.loadAsset(runSoundAddress, obj =>
        {
            this.runSound = (AudioClip)obj;
        });
    }

    void OnDestory()
    {
        AddressableAssetManager.Instance.releaseAsset(runSoundAddress);
    }

    public void run()
    {
        animator.SetBool("isRun", true);
    }

    public void idle()
    {
        animator.SetBool("isRun", false);
    }

    public void jump()
    {
        animator.SetTrigger("jump");
        isFall = true;
    }

    public void fall()
    {
        isFall = true;
        animator.SetBool("isFall", true);
    }

    public void jumpEnd()
    {
        if (isFall)
        {
            isFall = false;
            animator.SetBool("isFall", false);
            animator.SetBool("isFly", false);
            animator.SetTrigger("jumpEnd");
        }
    }

    public void startFly()
    {
        animator.SetBool("isFly", true);
    }

    public void stopFly()
    {
        animator.SetBool("isFly", false);
    }

    public void skillLight()
    {
        animator.SetTrigger("skillLight");
    }

    public void skillMedium()
    {
        animator.SetTrigger("skillMedium");
    }

    public void skillHeavy()
    {
        animator.SetTrigger("skillHeavy");
    }

    public void Die()
    {
        animator.SetBool("isFall", false);
        animator.SetBool("isDie", true);
    }

    public void LeftFoot()
    {
        if (runSound != null)
        {
            specialEffectController.playOneShot(runSound, 0.5f);
        }
    }

    public void RightFoot()
    {
        if (runSound != null)
        {
            specialEffectController.playOneShot(runSound, 0.5f);
        }
    }
}
