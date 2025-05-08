using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviourSinqletonBase<ManagerBase>
{
    // ������ģ��
    public List<MonoBehaviour> Monos = new List<MonoBehaviour>();

    // ����ģ��һ��ע�᷽��
    public void Register(MonoBehaviour mono)
    {
        // ���mono�����б���
        if (!Monos.Contains(mono))
        {
            Monos.Add(mono);
        }
    }

    public void UnRegister(MonoBehaviour mono)
    {
        if (Monos.Contains(mono))
        {
            Monos.Remove(mono);
        }
    }
}
