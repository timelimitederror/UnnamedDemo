using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 并行器
// 会在每帧将子节点全部执行一次
// 可以选择全部子节点都成功才返回成功，还是只要有一个子节点返回成功就返回成功
// 可以选择全部子节点都失败才返回失败，还是只要有一个子节点返回失败就返回失败
public class Parallel : Composite
{
    protected Policy successPolicy;
    protected Policy failurePolicy;

    protected Parallel(){ }

    public Parallel(string name, Policy successPolicy, Policy failurePolicy)
    {
        this.name = $"[Parallel]{name}";
        this.successPolicy = successPolicy;
        this.failurePolicy = failurePolicy;
    }

    protected override Status OnUpdate()
    {
        int successCount = 0;
        int failureCount = 0;
        LinkedListNode<Node> linkedListNode = children.First;
        int size = children.Count;
        for (int i = 0; i < size; i++)
        {
            Node node = linkedListNode.Value;
            if (!node.IsTerminated())// 如果这个节点还没运行结束就运行它
            {
                node.Tick();
            }
            linkedListNode = linkedListNode.Next;
            if (node.IsSuccess())
            {
                successCount++;
                if (successPolicy == Policy.RequireOne)
                {
                    return Status.Success;
                }
            }
            if (node.IsFailure())
            {
                failureCount++;
                if (failurePolicy == Policy.RequireOne)
                {
                    return Status.Failure;
                }
            }
        }

        if (failurePolicy == Policy.RequireAll && failureCount == size)
        {
            return Status.Failure;
        }
        if (successPolicy == Policy.RequireAll && successCount == size)
        {
            return Status.Success;
        }

        return Status.Running;
        // 有呆瓜用成Success也是All，Failure也是All，但是执行结果一半Success一半Failure怎么办
    }

    // 结束函数，将所有子节点设为中断
    protected override void OnTerminate()
    {
        foreach (Node node in children)
        {
            if (node.IsRunning())
            {
                base.Abort();
            }
        }
    }

    public enum Policy
    {
        RequireOne, RequireAll
    }
}
