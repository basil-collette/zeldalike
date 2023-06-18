using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[KnownType(typeof(DialogueNodeData))]
[KnownType(typeof(EventNodeData))]
public class DialogueNodeData : BaseNodeData
{
    //public Enum Type (talk, think)
    public string DialogueText;
    public DialogueNodeSide Side;
    public Pnj Pnj;
}
