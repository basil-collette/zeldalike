using System;
using UnityEngine;

public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
{
    private static T _instance = null;

    private static readonly object _lockObj = new object();

    protected SingletonBase() {
        if (_instance != null)
        {
            throw new InvalidOperationException("Cannot create multiple instances of a Singleton.");
        }
    }

    public static T Instance
    {
        get {
            lock (_lockObj)
            {
                if (SingletonBase<T>._instance == null)
                    _instance = new T(); // Activator.CreateInstance(typeof(T), true) as T;
            }

            return SingletonBase<T>._instance;
        }
    }

}
