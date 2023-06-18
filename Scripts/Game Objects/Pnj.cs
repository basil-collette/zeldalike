using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pnj : Interacting
{
    public string Name = "Anonymous";
    public AudioClip Voice;
    public Sprite Sprite;
    public List<DialogueReference> Dialogues;

    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = GameObject.Find("PauseManager").GetComponent<DialogueManager>();
    }


    void FixedUpdate()
    {
        
    }

    public void Talk()
    {
        var dialogueContainer = Dialogues.Find(x => x.Name == "hello").DialogueContainer;
        dialogueManager.StartDialogue(dialogueContainer);
    }

    protected override void OnInterfact()
    {
        Talk();
    }

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

}
