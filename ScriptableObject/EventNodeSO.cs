using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EventNodeSO")]
public class EventNodeSO : ScriptableObject
{
    public UnityEvent<string> Event;
}