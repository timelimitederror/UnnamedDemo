using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 监视器 持续检查并行行为的条件 有必要吗兄弟，上面加个过滤器不就完事了
public class Monitor : Parallel
{
    public Monitor(string name, Policy successPolicy, Policy failurePolicy)
    {
        this.name = $"[Monitor]{name}";
        this.successPolicy = successPolicy;
        this.failurePolicy = failurePolicy;
    }

    public void AddCondition(Node condition)
    {
        children.AddFirst(condition);
    }

    public void AddAction(Node action)
    {
        children.AddLast(action);
    }
}
