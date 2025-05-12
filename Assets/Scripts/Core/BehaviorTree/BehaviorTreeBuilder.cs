using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeBuilder
{
    private readonly Stack<Node> nodeStack; // ����ջ
    private readonly BehaviorTree behaviorTree;

    public BehaviorTreeBuilder()
    {
        nodeStack = new Stack<Node>();
        behaviorTree = new BehaviorTree(null);
    }

    public BehaviorTreeBuilder AddNode(Node node)
    {
        if (behaviorTree.haveRoot())// �и��ڵ���ջ��û�о���Ϊ���ڵ�
        {
            nodeStack.Peek().AddChild(node); // ��node��Ϊ����ջ����һ���ڵ���ӽڵ�
        }
        else
        {
            behaviorTree.SetRoot(node);
        }

        // ��Ͻڵ�����νڵ���Ҫ������ջ
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
        nodeStack.Pop(); // ����ջ�����ϲ�ڵ� �൱�ڽ��������ڻص����ڵ�
        return this;
    }

    public BehaviorTree End()
    {
        nodeStack.Clear();
        return behaviorTree;
    }

    //---------��װ���ڵ�---------
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
