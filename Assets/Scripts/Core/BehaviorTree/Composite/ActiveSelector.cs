using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主动选择器 不按顺序选择，而是按优先级选择，每次选择最优的（默认高优先级的在List的前面
// 用途是，当前情况下的最优子树变换了，中断当前正在执行的子树，执行最新的最优子树
public class ActiveSelector : Selector
{
    public ActiveSelector(string name)
    {
        this.name = $"[ActiveSelector]{name}";
    }

    protected override Status OnUpdate()
    {
        LinkedListNode<Node> prev = currentChild;
        base.OnInitialize();// currentChild = First
        Status result = base.OnUpdate();
        if (prev != null && currentChild != prev)// 最优子树变换，中断当前正在执行的子树，执行当前情况下的最优子树
        {
            prev.Value.Abort();
        }
        return result;
    }
}
