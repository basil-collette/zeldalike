using UnityEngine;

public abstract class SingletonGameObject<T> : MonoBehaviour
    where T : SingletonGameObject<T>
{
    private static T _instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this as T;

        //DontDestroyOnLoad(gameObject);
    }

    public static T Instance
    {
        get { return _instance; }
    }

}
