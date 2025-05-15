using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 重复器 重复成功执行指定次数
public class Repeat : Decorator
{
    private int conunter; // 当前重复次数
    private int limit; // 重复总次数

    public Repeat(string name, int limit)
    {
        this.name = $"[Repeat]{name}";
        this.limit = limit;
    }

    protected override void OnInitialize()
    {
        conunter = 0;
    }

    protected override Status OnUpdate()
    {
        while (conunter < limit)
        {
            child.Tick();
            if (child.IsRunning())
            {
                return Status.Running;
            }
            if (child.IsFailure())
            {
                return Status.Failure;
            }
            conunter++;
        }
        return Status.Success;
    }
}
