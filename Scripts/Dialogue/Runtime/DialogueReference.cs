using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueReference
{
    public string NameCode;
    public DialogueContainer DialogueContainer;
    [SerializeReference] public List<DialogueCondition> Conditions;
    public Priority Priority;
    public bool IsSaid;
}