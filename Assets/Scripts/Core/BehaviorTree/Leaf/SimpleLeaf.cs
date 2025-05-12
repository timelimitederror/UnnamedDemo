using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �򵥵�Ҷ�ӽڵ㣬ֻ�ô���ִ�з���
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
