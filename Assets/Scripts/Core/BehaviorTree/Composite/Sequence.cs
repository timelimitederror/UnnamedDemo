using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
// һ����һ����ִ���ӽڵ㣬������ɺ��ٴ�ͷ��ʼ
// ����ӽڵ㷵�أ�
// SUCCESS������һִ֡����һ���ӽڵ�
// FAILURE: ����ʧ�ܣ��������ж����ĵ�����
// RUNNING: ����һ֡�ٴ�ִ�иýڵ�
// �������ӽڵ㷵��SUCCESS�򷵻�SUCCESS
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
            // ����ӽڵ����У���û�гɹ�����ֱ�ӷ��ظý����
            // �ǡ������С��Ǿͱ������ڵ�Ҳ�������У��м�¼��ǰ�ڵ㣬�´λ������ִ�У�
            // �ǡ�ʧ�ܡ��ͱ������ڵ�Ҳ����ʧ���ˣ��´λ��پ���OnInitialize����ͷ��ʼ��
            if (status != Status.Success)
            {
                return status;
            }
            // ������гɹ���ִ����һ���ӽڵ�
            currentChild = currentChild.Next;
            // ȫ�����
            if (currentChild == null)
            {
                return Status.Success;
            }
        }
    }
}
