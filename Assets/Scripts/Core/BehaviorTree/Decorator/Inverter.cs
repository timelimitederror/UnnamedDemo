using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȡ���� SUCCESS��FAILURE��FAILURE��SUCCESS
public class Inverter : Decorator
{
    public Inverter(string name)
    {
        this.name = $"[Inverter]{name}";
    }

    protected override Status OnUpdate()
    {
        child.Tick();
        if (child.IsFailure())
        {
            return Status.Success;
        }
        if (child.IsSuccess())
        {
            return Status.Failure;
        }
        return Status.Running;
    }
}
