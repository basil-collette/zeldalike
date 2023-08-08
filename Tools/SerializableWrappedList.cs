using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools
{
    [System.Serializable]
    public class SerializableWrappedList<T>
    {
        [SerializeField] public string Key;
        [SerializeField] public List<T> List;
    }
}
