using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeBuilder
{
    private readonly Stack<Node> nodeStack; // 构建栈
    private readonly BehaviorTree behaviorTree;

    public BehaviorTreeBuilder()
    {
        nodeStack = new Stack<Node>();
        behaviorTree = new BehaviorTree(null);
    }

    public BehaviorTreeBuilder AddNode(Node node)
    {
        if (behaviorTree.haveRoot())// 有根节点入栈，没有就作为根节点
        {
            nodeStack.Peek().AddChild(node); // 将node作为构建栈最上一个节点的子节点
        }
        else
        {
            behaviorTree.SetRoot(node);
        }

        // 组合节点和修饰节点需要进构建栈
        if (node is Composite || node is Decorator)
        {
            nodeStack.Push(node);
        }
        return this;
    }

    public void TreeTick()
    {
        behaviorTree.Tick();
    }

    public void DebugTree()
    {
        behaviorTree.DebugTree();
    }

    public BehaviorTreeBuilder Back()
    {
        nodeStack.Pop(); // 弹出栈的最上层节点 相当于将构建环节回到父节点
        return this;
    }

    public BehaviorTree End()
    {
        nodeStack.Clear();
        return behaviorTree;
    }

    //---------包装各节点---------
    public BehaviorTreeBuilder Sequence(string name)
    {
        AddNode(new Sequence(name));
        return this;
    }

    public BehaviorTreeBuilder Seletctor(string name)
    {
        AddNode(new Selector(name));
        return this;
    }

    public BehaviorTreeBuilder Filter(string name)
    {
        AddNode(new Filter(name));
        return this;
    }

    public BehaviorTreeBuilder Parallel(string name, Parallel.Policy success, Parallel.Policy failure)
    {
        AddNode(new Parallel(name, success, failure));
        return this;
    }

    public BehaviorTreeBuilder Monitor(string name, Parallel.Policy success, Parallel.Policy failure)
    {
        AddNode(new Monitor(name, success, failure));
        return this;
    }

    public BehaviorTreeBuilder ActiveSelector(string name)
    {
        AddNode(new ActiveSelector(name));
        return this;
    }

    public BehaviorTreeBuilder Repeat(string name, int limit)
    {
        AddNode(new Repeat(name, limit));
        return this;
    }

    public BehaviorTreeBuilder Inverter(string name)
    {
        AddNode(new Inverter(name));
        return this;
    }

    public BehaviorTreeBuilder SimpleLeaf(string name, Func<Node.Status> func)
    {
        AddNode(new SimpleLeaf(name, func));
        return this;
    }
}
