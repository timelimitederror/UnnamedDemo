using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������
// ����ÿ֡���ӽڵ�ȫ��ִ��һ��
// ����ѡ��ȫ���ӽڵ㶼�ɹ��ŷ��سɹ�������ֻҪ��һ���ӽڵ㷵�سɹ��ͷ��سɹ�
// ����ѡ��ȫ���ӽڵ㶼ʧ�ܲŷ���ʧ�ܣ�����ֻҪ��һ���ӽڵ㷵��ʧ�ܾͷ���ʧ��
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
            if (!node.IsTerminated())// �������ڵ㻹û���н�����������
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
        // �д����ó�SuccessҲ��All��FailureҲ��All������ִ�н��һ��Successһ��Failure��ô��
    }

    // �����������������ӽڵ���Ϊ�ж�
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
