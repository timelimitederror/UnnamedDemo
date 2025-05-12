using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 简单的叶子节点，只用传入执行方法
public class SimpleLeaf : Node
{
    private Func<Status> processMethod;

    public SimpleLeaf(string name, Func<Status> processMethod)
    {
        this.name = $"[SimpleLeaf]{name}";
        this.processMethod = processMethod;
    }

    protected override Status OnUpdate()
    {
        return processMethod();
    }
}
