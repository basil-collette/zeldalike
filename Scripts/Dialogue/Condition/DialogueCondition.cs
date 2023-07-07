using System;
using UnityEngine;

[Serializable]
public abstract class DialogueCondition
{
    public string TargetCode;
    public abstract bool Verify();
}

[Serializable]
public class DialogueConditionEndQuest : DialogueCondition
{
    public override bool Verify()
    {
        PlayerQuest playerQuest = Resources.Load<PlayerQuest>("ScriptableObjects/Player/Quest/PlayerQuest");
        return playerQuest.GetQuestByName(TargetCode).IsCompleted;
    }
}

[Serializable]
public class DialogueConditionHaveTalk : DialogueCondition
{
    public string PNJName;

    public override bool Verify()
    {
        PNJDialogues PnjDialogues = Resources.Load<PNJDialogues>($"ScriptableObjects/Dialogues/PNJ Dialogues/{PNJName}");

        DialogueReference dialogueRef = PnjDialogues.Dialogues.Find(x => x.NameCode == TargetCode);

        return dialogueRef.IsSaid;
    }
}

[Serializable]
public class DialogueConditionPossess : DialogueCondition
{
    public override bool Verify()
    {
        Inventory inventory = Resources.Load<Inventory>("ScriptableObjects/Player/Inventory/Inventory");

        return (inventory.Items.Exists(x => x.NameCode == TargetCode)
            || inventory.Hotbars.Exists(x => x.NameCode == TargetCode)
            || inventory.Weapon?.NameCode == TargetCode);
    }
}

/*
[Serializable]
public class DialogueConditionLocation : DialogueCondition
{

}
*/