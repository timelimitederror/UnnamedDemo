using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviourSinqletonBase<ManagerBase>
{
    // 管理功能模块
    public List<MonoBehaviour> Monos = new List<MonoBehaviour>();

    // 功能模块一个注册方法
    public void Register(MonoBehaviour mono)
    {
        // 如果mono不在列表中
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
