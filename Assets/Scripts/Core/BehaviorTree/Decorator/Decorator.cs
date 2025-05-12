using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 修饰节点的基类
public abstract class Decorator : Node
{
    protected Node child;

    public override void AddChild(Node node)
    {
        this.child = node;
    }

    public override void Reset()
    {
        base.Reset();
        child.Reset();
    }

    public override void Abort()
    {
        base.Abort();
        child.Abort();
    }

    public override void DebugTree(int level = 0)
    {
        base.DebugTree(level);
        level++;
        child.DebugTree(level);
    }
}
