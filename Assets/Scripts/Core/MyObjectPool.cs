using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MyObjectPool<T> where T : class
{
    private ObjectPool<T> objectPool;
    private List<T> objectList;
    public int CountActive
    {
        get
        {
            return objectPool.CountActive;
        }
    }

    public MyObjectPool(
        Func<T> createFunc,
        Action<T> actionOnGet = null,
        Action<T> actionOnRelease = null,
        Action<T> actionOnDestroy = null,
        bool collectionCheck = true,
        int defaultCapacity = 10,
        int maxSize = 1000
        )
    {
        objectList = new List<T>();

        objectPool = new ObjectPool<T>(
            createFunc: () =>
            {
                T instance = createFunc();
                objectList.Add(instance); // 创建时记录实例
                return instance;
            },
            actionOnGet: actionOnGet,
            actionOnRelease: actionOnRelease,
            actionOnDestroy: (obj) =>
            {
                objectList.Remove(obj); // 销毁时移除记录
                actionOnDestroy?.Invoke(obj);
            }, collectionCheck, defaultCapacity, maxSize);
    }

    // 获取对象
    public T Get() => objectPool.Get();

    // 归还对象
    public void Release(T obj) => objectPool.Release(obj);

    // 强制销毁所有对象（包括未归还的）
    public void Clear()
    {
        foreach(T obj in objectList)
        {
            try
            {
                objectPool.Release(obj);
            }
            catch (Exception ex)
            {

            }
        }
        objectPool.Clear();
        objectList.Clear();
    }

}
