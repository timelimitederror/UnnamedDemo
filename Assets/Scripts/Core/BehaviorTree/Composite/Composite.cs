using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 组合节点基类
public abstract class Composite : Node
{
    protected LinkedList<Node> children = new LinkedList<Node>();

    // 移除指定子节点
    public virtual void RemoveChild(Node node)
    {
        children.Remove(node);
    }

    public void ClearChildren()
    {
        children.Clear();
    }

    public override void AddChild(Node node)
    {
        children.AddLast(node);
    }

    public override void Reset()
    {
        base.Reset();
        foreach (Node node in children)
        {
            node.Reset();
        }
    }

    public override void Abort()
    {
        base.Abort();
        foreach (Node node in children)
        {
            node.Abort();
        }
    }

    public override void DebugTree(int level = 0)
    {
        base.DebugTree(level);
        level++;
        for (int i = 0; i < children.Count; i++)
        {
            children.ElementAt(i).DebugTree(level);
        }
    }
}
