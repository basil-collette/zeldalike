using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pnj : Interacting
{
    public string Name = "Anonymous";
    public AudioClip Voice;
    public Sprite Sprite;
    public PNJDialogues Dialogues;

    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    protected override void OnInteract()
    {
        Talk();
    }

    public void Talk()
    {
        var dialogueContainer = GetDialogue();
        dialogueManager.StartDialogue(dialogueContainer);
    }

    DialogueReference GetDialogue()
    {
        List<DialogueReference> dialogues = Dialogues.Dialogues.AsEnumerable()
            .Where(x => !x.Conditions.Exists(condition => condition.Verify() == false)) // Il n'existe pas de condition dont verify() return false
            .OrderBy(x => -(int)x.Priority)
            .ToList();

        if (dialogues.Count == 1)
        {
            return dialogues.FirstOrDefault();
        }

        int index = Random.Range(0, dialogues.Count);
        return dialogues[index];
    }

    /*
    bool HaveSaid()
    {
        return true;
    }

    bool HaveStartedQuest()
    {
        return true;
    }

    bool HaveEndedQuest()
    {
        return true;
    }

    bool HaveDo()
    {
        return true;
    }
    */
}
