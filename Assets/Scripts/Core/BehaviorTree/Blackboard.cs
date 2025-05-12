using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public T Get<T>(string key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        return default(T);
    }

    public void Add<T>(string key, T value)
    {
        data.Add(key, value);
    }

    public bool Remove<T>(string key)
    {
        if (data.ContainsKey(key))
        {
            data.Remove(key);
            return true;
        }
        return false;
    }
}
