using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有节点的父节点
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
