using System;
using UnityEngine;

public abstract class SignletonGameObject<T> : MonoBehaviour where T : SignletonGameObject<T>
{
    private static T _instance = null;

    private static readonly object _lockObj = new object();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
        }
        else
        {
            lock (_lockObj)
            {
                if (SignletonGameObject<T>._instance == null)
                {
                    new GameObject(typeof(T).Name, typeof(T));
                    _instance = Activator.CreateInstance(typeof(T), true) as T;
                }
            }

            DontDestroyOnLoad(gameObject);
        }
    }

    public static T Instance
    {
        get { return SignletonGameObject<T>._instance; }
    }

}
