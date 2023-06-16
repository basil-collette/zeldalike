using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[KnownType(typeof(DialogueNodeData))]
[KnownType(typeof(EventNodeData))]
public class DialogueNodeData : BaseNodeData
{
    public string DialogueText;
    public DialogueNodeSide Side;
    public Sprite Sprite;
    public AudioClip AudioClip;
}
