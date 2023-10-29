using Assets.Scripts.Enums;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[KnownType(typeof(DialogueNodeData))]
[KnownType(typeof(EventNodeData))]
public class DialogueNodeData : BaseNodeData
{
    public DialogueTypeEnum Type;
    public string DialogueText;
    public string DialogueCode;
    public DialogueNodeSide Side;
    public Pnj Pnj;
    public string[] Outputs = new string[] { };
    public DialogEmotionType Emotion = DialogEmotionType.neutral;
}
