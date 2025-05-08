using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 需要挂载的脚本的单例基类，适用于跨场景存在的游戏物体上挂的脚本
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
        // 禁止销毁游戏对象
        DontDestroyOnLoad(gameObject);
    }
}

