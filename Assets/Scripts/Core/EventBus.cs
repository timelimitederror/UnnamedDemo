using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    public static readonly Dictionary<Type, List<Action<object>>> Events = new Dictionary<Type, List<Action<object>>>();

    // ���Ĵ��������¼�
    public static void Subscribe<T>(Action<T> handler) where T : class
    {
        Type type = typeof(T);
        if (!Events.TryGetValue(type, out var actionList))
        {
            Events[type] = new List<Action<object>>();
            Events[type].Add(obj => handler(obj as T));
        }
        else
        {
            actionList.Add(obj => handler(obj as T));
        }
    }

    // ȡ������
    public static void UnSubscribe<T>(Action<T> handler) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var actionList))
        {
            actionList.Remove(obj => handler(obj as T));
        }
    }

    // �����¼������ݲ���
    public static void Publish<T>(T eventData) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var actionList))
        {
            foreach (Action<object> action in actionList)
            {
                action.Invoke(eventData);
            }
        }
    }
}