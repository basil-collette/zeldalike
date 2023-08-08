using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[KnownType(typeof(DialogueNodeData))]
[KnownType(typeof(EventNodeData))]
public class DialogueNodeData : BaseNodeData
{
    //public DialogueTypeEnum Type (talk, think)
    public string DialogueText;
    public string DialogueCode;
    //public DialogueNodeSide Side;
    public Pnj Pnj;
}
