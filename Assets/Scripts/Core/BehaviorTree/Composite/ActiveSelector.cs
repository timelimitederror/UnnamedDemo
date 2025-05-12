using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ѡ���� ����˳��ѡ�񣬶��ǰ����ȼ�ѡ��ÿ��ѡ�����ŵģ�Ĭ�ϸ����ȼ�����List��ǰ��
// ��;�ǣ���ǰ����µ����������任�ˣ��жϵ�ǰ����ִ�е�������ִ�����µ���������
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
        if (prev != null && currentChild != prev)// ���������任���жϵ�ǰ����ִ�е�������ִ�е�ǰ����µ���������
        {
            prev.Value.Abort();
        }
        return result;
    }
}
