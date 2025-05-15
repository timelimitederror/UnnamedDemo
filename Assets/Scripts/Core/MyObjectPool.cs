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
                objectList.Add(instance); // ����ʱ��¼ʵ��
                return instance;
            },
            actionOnGet: actionOnGet,
            actionOnRelease: actionOnRelease,
            actionOnDestroy: (obj) =>
            {
                objectList.Remove(obj); // ����ʱ�Ƴ���¼
                actionOnDestroy?.Invoke(obj);
            }, collectionCheck, defaultCapacity, maxSize);
    }

    // ��ȡ����
    public T Get() => objectPool.Get();

    // �黹����
    public void Release(T obj) => objectPool.Release(obj);

    // ǿ���������ж��󣨰���δ�黹�ģ�
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
