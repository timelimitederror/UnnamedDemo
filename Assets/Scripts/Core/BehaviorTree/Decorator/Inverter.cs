using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 取反器 SUCCESS改FAILURE，FAILURE改SUCCESS
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
