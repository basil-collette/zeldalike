using System;
using UnityEngine;

[Serializable]
public abstract class DialogueCondition
{
    public string TargetCode;
    public string Type;
    public bool Not = false;

    public abstract bool Verify();
}

[Serializable]
public class DialogueConditionEndQuest : DialogueCondition
{
    public DialogueConditionEndQuest() { Type = "EndQuest"; }

    public override bool Verify()
    {
        PlayerQuest playerQuest = Resources.Load<PlayerQuest>("ScriptableObjects/Player/Quest/PlayerQuest");
        bool result = playerQuest.GetQuestByName(TargetCode).IsCompleted;
        return (Not) ? !result : result;
    }
}

[Serializable]
public class DialogueConditionHaveTalk : DialogueCondition
{
    public string PNJName;

    public DialogueConditionHaveTalk() { Type = "HaveTalk"; }

    public override bool Verify()
    {
        bool result = DialogueStates.HaveSaid(PNJName, TargetCode);
        return (Not) ? !result : result;
    }
}

[Serializable]
public class DialogueConditionPossess : DialogueCondition
{
    public DialogueConditionPossess() { Type = "Possess"; }

    public override bool Verify()
    {
        Inventory inventory = Resources.Load<Inventory>("ScriptableObjects/Player/Inventory/Inventory");

        bool result = (inventory.Items.Exists(x => x.NameCode == TargetCode)
            || inventory.Hotbars.Exists(x => x.NameCode == TargetCode)
            || inventory.Weapon?.NameCode == TargetCode);

        return (Not) ? !result : result;
    }
}

/*
[Serializable]
public class DialogueConditionLocation : DialogueCondition
{
    public DialogueConditionLocation() { Type = "Location"; }
}
*/