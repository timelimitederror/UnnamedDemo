using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ѡ����������ִ���ӽڵ㣬����ӽڵ㷵�أ�
// SUCCESS: ��������
// FAILURE����ִ����һ���ڵ�
// RUNNING: ����ִ�е�ǰ�ڵ�
// ���ֱ������������ִ����ϣ���Ȼû��SUCCESS���򷵻�FAILURE����ѡ��ʧ�ܣ���Ϊû�з��������ģ�
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
