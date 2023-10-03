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
    [SerializeField] Sprite spriteActionButton;

    //[HideInInspector] public List<string> dialogueCodeSaid = new List<string>();
    DialogueManager dialogueManager;
    GameObject ActionButtonTalk;

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    protected override void OnInteract()
    {
        //Talk();
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            ActionButtonTalk = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Talk);
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(ActionButtonTalk);
            ActionButtonTalk = null;
        }
    }

    public void Talk()
    {
        var dialogueRef = GetWantToSay();
        dialogueManager.StartDialogue(this, dialogueRef);
    }

    public DialogueReference GetWantToSay()
    {
        List<DialogueReference> dialogues = Dialogues.Dialogues.AsEnumerable()
            .Where(x => !x.Conditions.Exists(condition => condition.Verify() == false)).ToList();

        Priority maxPriority = dialogues.Max(x => x.Priority);

        dialogues = dialogues.Where(x => x.Priority == maxPriority).ToList();

        if (dialogues.Count() == 1)
        {
            return dialogues.FirstOrDefault();
        }

        int index = Random.Range(0, dialogues.Count());
        return dialogues.ElementAt(index);
    }

    public bool HaveSaid(string dialogueNameCode)
    {
        return MainGameManager._dialogStatesManager.HaveSaid(Name, dialogueNameCode);
    }

    public void AddSaid(string dialogueNameCode)
    {
        MainGameManager._dialogStatesManager.AddSaid(Name, dialogueNameCode);
    }

}
