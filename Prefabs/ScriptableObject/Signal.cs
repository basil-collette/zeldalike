using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Signal")]
public class Signal : ScriptableObject
{

    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        foreach (SignalListener sl in listeners)
        {
            sl.OnSignalRaised();
        }
    }

    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }

    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }

}
