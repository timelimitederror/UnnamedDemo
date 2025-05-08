using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private Action<Collider> action;
    private Action releaseAction;
    private float activeTime = 100f;
    private float lastActiveTime = 0f;

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

    private void OnTriggerEnter(Collider other)
    {
        action.Invoke(other);
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
