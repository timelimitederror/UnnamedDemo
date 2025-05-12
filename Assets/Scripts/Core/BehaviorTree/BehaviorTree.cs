using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    private Node root;
    public BehaviorTree(Node root)
    {
        this.root = root;
    }

    public void Tick()
    {
        if (root != null)
        {
            root.Tick();
        }
    }

    public void DebugTree()
    {
        root.DebugTree();
    }

    public void SetRoot(Node root)
    {
        this.root = root;
    }

    public bool haveRoot()
    {
        return this.root != null;
    }
}
