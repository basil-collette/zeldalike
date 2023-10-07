using UnityEngine;

public class DialogWhenCross : EventWhenCross
{
    [SerializeField] DialogueContainer dialog;

    protected override void TriggerEvent()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialog);
    }
}