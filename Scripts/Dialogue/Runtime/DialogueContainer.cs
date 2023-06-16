using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    [SerializeReference] public List<BaseNodeData> NodeDatas = new List<BaseNodeData>();
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
}
