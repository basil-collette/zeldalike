using UnityEngine;
using UnityEngine.Events;

public class DoOnEvent : MonoBehaviour
{
    [SerializeField] string _eventName;
    [SerializeField] UnityEvent _event;

    private void OnEnable()
    {
        StoryEventManager.OnEvent += Invoke;
    }

    private void OnDisable()
    {
        StoryEventManager.OnEvent += Invoke;
    }

    void Invoke(string eventName)
    {
        if (eventName == _eventName)
        {
            _event.Invoke();
        }
    }

}
