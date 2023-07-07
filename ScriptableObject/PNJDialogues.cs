using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PNJDialogues")]
public class PNJDialogues : ScriptableObject
{
    public List<DialogueReference> Dialogues;
}