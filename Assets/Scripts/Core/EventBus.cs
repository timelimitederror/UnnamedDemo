using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    private static readonly Dictionary<Type, Action<object>> Events = new Dictionary<Type, Action<object>>();

    // ���Ĵ��������¼�
    public static void Subscribe<T>(Action<T> handler) where T : class
    {
        Type type = typeof(T);
        if (!Events.TryGetValue(type, out var action))
        {
            Events[type] = obj => handler(obj as T);
        }
        else
        {
            action += obj => handler(obj as T);
        }
    }

    // ȡ������
    public static void UnSubscribe<T>(Action<T> handler) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var action))
        {
            action -= obj => handler(obj as T);
        }
    }

    // �����¼������ݲ���
    public static void Publish<T>(T eventData) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var action))
        {
            action.Invoke(eventData);
        }
    }
}