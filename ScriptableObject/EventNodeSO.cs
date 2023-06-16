using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EventNodeSO")]
public class EventNodeSO : ScriptableObject
{
    public UnityEvent Event;

    public void Invoke()
    {
        Event.Invoke();
    }

}