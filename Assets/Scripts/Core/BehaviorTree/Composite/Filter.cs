using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��������������¼� �൱��if
// ��������������SUCCESS���Ż�ִ���¼�
public class Filter : Sequence
{
    public Filter(string name)
    {
        this.name = $"[Filter]{name}";
    }

    // ��ӹ�������
    public void AddCondition(Node condition)
    {
        children.AddFirst(condition);
    }

    // ����¼�
    public void AddAction(Node action)
    {
        children.AddLast(action);
    }
}
