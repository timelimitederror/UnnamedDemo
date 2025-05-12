using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 序列
// 一个接一个地执行子节点，迭代完成后再从头开始
// 如果子节点返回：
// SUCCESS：在下一帧执行下一个子节点
// FAILURE: 返回失败（可用于中断树的迭代）
// RUNNING: 在下一帧再次执行该节点
// 当所有子节点返回SUCCESS则返回SUCCESS
public class Sequence : Composite
{
    protected LinkedListNode<Node> currentChild;

    protected Sequence() { }

    public Sequence(string name)
    {
        this.name = $"[sequence]{name}";
    }

    protected override void OnInitialize()
    {
        currentChild = children.First;
    }

    protected override Status OnUpdate()
    {
        while (true)
        {
            Status status = currentChild.Value.Tick();
            // 如果子节点运行，还没有成功，就直接返回该结果。
            // 是「运行中」那就表明本节点也是运行中，有记录当前节点，下次还会继续执行；
            // 是「失败」就表明本节点也运行失败了，下次会再经历OnInitialize，从头开始。
            if (status != Status.Success)
            {
                return status;
            }
            // 如果运行成功，执行下一个子节点
            currentChild = currentChild.Next;
            // 全部完成
            if (currentChild == null)
            {
                return Status.Success;
            }
        }
    }
}
