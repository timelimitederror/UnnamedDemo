using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过滤器 可以添加条件和事件 相当于if
// 必须所有条件都SUCCESS，才会执行事件
public class Filter : Sequence
{
    public Filter(string name)
    {
        this.name = $"[Filter]{name}";
    }

    // 添加过滤条件
    public void AddCondition(Node condition)
    {
        children.AddFirst(condition);
    }

    // 添加事件
    public void AddAction(Node action)
    {
        children.AddLast(action);
    }
}
