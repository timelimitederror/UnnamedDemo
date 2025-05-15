using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private Action<Collider> action;
    protected Action releaseAction;
    private float activeTime = 0f;
    protected float lastActiveTime = 0f;

    void OnEnable()
    {
        lastActiveTime = Time.fixedTime;
    }

    void FixedUpdate()
    {
        if (activeTime > 0f && Time.fixedTime - lastActiveTime >= activeTime && releaseAction != null)
        {
            releaseAction.Invoke();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (action != null)
        {
            action.Invoke(other);
        }
    }

    public void setAction(Action<Collider> action)
    {
        this.action = action;
    }

    public void setActiveTime(float activeTime)
    {
        this.activeTime = activeTime;
    }

    public void setReleaseAction(Action action)
    {
        this.releaseAction = action;
    }
}
