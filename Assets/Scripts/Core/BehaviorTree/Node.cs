using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有节点的父节点
/*
    Composite组合节点
        Parallel并行器 会在每帧将子节点全部执行一次 可以选择成功或失败条件
            Monitor监视器 持续检查并行行为的条件
        Selector选择器，依次执行子节点，有一个成功就返回成功并停止继续
            ActiveSelector主动选择器 不按顺序选择，而是按优先级选择，每次选择最优的（默认高优先级的在List的前面 可以中断running变成最新的最优
        Sequence序列，依次执行子节点，有一个失败就返回失败并停止继续
            Filter过滤器 可以添加条件和事件 相当于if

    Decorator修饰节点
        Inverter取反器 SUCCESS改FAILURE，FAILURE改SUCCESS
        Repeat重复器 重复成功执行指定次数

    Leaf叶子节点
        SimpleLeaf 只执行一个方法的简单叶子节点
 */
public abstract class Node
{
    public string name;
    public Status status = Status.Invalid;

    // 当进入该节点时才会执行一次
    protected virtual void OnInitialize() { }

    // 该节点的运行逻辑，实时返回执行结果的状态
    protected abstract Status OnUpdate();

    // 运行结束时才会执行一次的函数
    protected virtual void OnTerminate() { }

    // 节点运行方法
    // 返回本次调用的结果
    public virtual Status Tick()
    {
        if (!IsRunning())
        {
            OnInitialize();
        }
        status = OnUpdate();
        if (!IsRunning())
        {
            OnTerminate();
        }
        return status;
    }

    // 添加子节点
    public virtual void AddChild(Node node) { }

    // 重置该节点的动作 这啥用啊
    public virtual void Reset()
    {
        status = Status.Invalid;
    }

    // 强行打断该节点的动作
    public virtual void Abort()
    {
        OnTerminate();
        status = Status.Aborted;
    }

    protected void DebugOutput(string str)
    {
        Debug.Log(str);
    }

    // 打印子树
    public virtual void DebugTree(int level = 0)
    {
        DebugOutput(new string('#', level) + name + "  status:" + status + "\n");
    }

    // 是否运行结束
    public bool IsTerminated()
    {
        return IsSuccess() || IsFailure();
    }

    public bool IsSuccess()
    {
        return status == Status.Success;
    }

    public bool IsFailure()
    {
        return status == Status.Failure;
    }

    public bool IsRunning()
    {
        return status == Status.Running;
    }

    public enum Status
    {
        Failure = 0,
        Success = 1,
        Running = 2,
        Aborted = 3,// 中断
        Invalid = 4// 无效
    }
}
