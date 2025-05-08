using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    private static readonly Dictionary<Type, Action<object>> Events = new Dictionary<Type, Action<object>>();

    // 订阅带参数的事件
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

    // 取消订阅
    public static void UnSubscribe<T>(Action<T> handler) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var action))
        {
            action -= obj => handler(obj as T);
        }
    }

    // 发布事件并传递参数
    public static void Publish<T>(T eventData) where T : class
    {
        if (Events.TryGetValue(typeof(T), out var action))
        {
            action.Invoke(eventData);
        }
    }
}