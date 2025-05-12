using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������鲢����Ϊ������ �б�Ҫ���ֵܣ�����Ӹ�����������������
public class Monitor : Parallel
{
    public Monitor(string name, Policy successPolicy, Policy failurePolicy)
    {
        this.name = $"[Monitor]{name}";
        this.successPolicy = successPolicy;
        this.failurePolicy = failurePolicy;
    }

    public void AddCondition(Node condition)
    {
        children.AddFirst(condition);
    }

    public void AddAction(Node action)
    {
        children.AddLast(action);
    }
}
