using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 选择器，依次执行子节点，如果子节点返回：
// SUCCESS: 结束迭代
// FAILURE：下执行下一个节点
// RUNNING: 继续执行当前节点
// 如果直到所有子树都执行完毕，仍然没有SUCCESS，则返回FAILURE代表选择失败（因为没有符合条件的）
public class Selector : Sequence
{
    protected Selector() { }

    public Selector(string name)
    {
        this.name = $"[selector]{name}";
    }

    protected override Status OnUpdate()
    {
        while (true)
        {
            Status status = currentChild.Value.Tick();
            if (status != Status.Failure)
            {
                return status;
            }
            currentChild = currentChild.Next;
            if (currentChild == null)
            {
                return Status.Failure;
            }
        }
    }
}
