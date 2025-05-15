using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ظ��� �ظ��ɹ�ִ��ָ������
public class Repeat : Decorator
{
    private int conunter; // ��ǰ�ظ�����
    private int limit; // �ظ��ܴ���

    public Repeat(string name, int limit)
    {
        this.name = $"[Repeat]{name}";
        this.limit = limit;
    }

    protected override void OnInitialize()
    {
        conunter = 0;
    }

    protected override Status OnUpdate()
    {
        while (conunter < limit)
        {
            child.Tick();
            if (child.IsRunning())
            {
                return Status.Running;
            }
            if (child.IsFailure())
            {
                return Status.Failure;
            }
            conunter++;
        }
        return Status.Success;
    }
}
