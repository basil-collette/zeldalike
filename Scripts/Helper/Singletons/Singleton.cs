using System;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T _instance = default(T);

    private static readonly object _lockObj = new object();

    protected Singleton() {
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
                if (Singleton<T>._instance == null)
                    _instance = new T(); // Activator.CreateInstance(typeof(T), true) as T;
            }

            return Singleton<T>._instance;
        }
    }

}
