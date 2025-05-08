using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectController : MonoBehaviour
{
    private Action releaseAction;
    private float activeTime = 100f;
    private float lastActiveTime = 0f;
    private ParticleSystem thisParticleSystem;

    void Awake()
    {
        thisParticleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        lastActiveTime = Time.fixedTime;
    }

    void FixedUpdate()
    {
        if (Time.fixedTime - lastActiveTime >= activeTime && releaseAction != null)
        {
            releaseAction.Invoke();
        }
    }

    public void setActiveTime(float activeTime)
    {
        this.activeTime = activeTime;
    }

    public void setReleaseAction(Action action)
    {
        this.releaseAction = action;
    }

    public void playSE()
    {
        if(thisParticleSystem != null)
        {
            thisParticleSystem.Play();
        }
    }
}
