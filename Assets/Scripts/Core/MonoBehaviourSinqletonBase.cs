using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ҫ���صĽű��ĵ������࣬�����ڿ糡�����ڵ���Ϸ�����ϹҵĽű�
public class MonoBehaviourSinqletonBase<T> : MonoBehaviour where T : MonoBehaviourSinqletonBase<T>
{
    public static T Instance;

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        // ��ֹ������Ϸ����
        DontDestroyOnLoad(gameObject);
    }
}

