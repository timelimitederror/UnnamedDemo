using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���нڵ�ĸ��ڵ�
/*
    Composite��Ͻڵ�
        Parallel������ ����ÿ֡���ӽڵ�ȫ��ִ��һ�� ����ѡ��ɹ���ʧ������
            Monitor������ ������鲢����Ϊ������
        Selectorѡ����������ִ���ӽڵ㣬��һ���ɹ��ͷ��سɹ���ֹͣ����
            ActiveSelector����ѡ���� ����˳��ѡ�񣬶��ǰ����ȼ�ѡ��ÿ��ѡ�����ŵģ�Ĭ�ϸ����ȼ�����List��ǰ�� �����ж�running������µ�����
        Sequence���У�����ִ���ӽڵ㣬��һ��ʧ�ܾͷ���ʧ�ܲ�ֹͣ����
            Filter������ ��������������¼� �൱��if

    Decorator���νڵ�
        Inverterȡ���� SUCCESS��FAILURE��FAILURE��SUCCESS
        Repeat�ظ��� �ظ��ɹ�ִ��ָ������

    LeafҶ�ӽڵ�
        SimpleLeaf ִֻ��һ�������ļ�Ҷ�ӽڵ�
 */
public abstract class Node
{
    public string name;
    public Status status = Status.Invalid;

    // ������ýڵ�ʱ�Ż�ִ��һ��
    protected virtual void OnInitialize() { }

    // �ýڵ�������߼���ʵʱ����ִ�н����״̬
    protected abstract Status OnUpdate();

    // ���н���ʱ�Ż�ִ��һ�εĺ���
    protected virtual void OnTerminate() { }

    // �ڵ����з���
    // ���ر��ε��õĽ��
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

    // ����ӽڵ�
    public virtual void AddChild(Node node) { }

    // ���øýڵ�Ķ��� ��ɶ�ð�
    public virtual void Reset()
    {
        status = Status.Invalid;
    }

    // ǿ�д�ϸýڵ�Ķ���
    public virtual void Abort()
    {
        OnTerminate();
        status = Status.Aborted;
    }

    protected void DebugOutput(string str)
    {
        Debug.Log(str);
    }

    // ��ӡ����
    public virtual void DebugTree(int level = 0)
    {
        DebugOutput(new string('#', level) + name + "  status:" + status + "\n");
    }

    // �Ƿ����н���
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
        Aborted = 3,// �ж�
        Invalid = 4// ��Ч
    }
}
