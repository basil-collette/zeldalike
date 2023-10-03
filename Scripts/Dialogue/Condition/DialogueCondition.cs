using System;
using UnityEngine;

[Serializable]
public abstract class DialogueCondition
{
    public string TargetCode;
    #if UNITY_EDITOR
    [ShowOnly]
    #endif
    public string Type;
    public bool Not = false;

    public abstract bool Verify();
}

[Serializable]
public class DialogueConditionStartedQuest : DialogueCondition
{
    public DialogueConditionStartedQuest() { Type = "StartedQuest"; }

    public override bool Verify()
    {
        bool result = MainGameManager._questbookManager.GetQuestByCode(TargetCode) != null;
        return (Not) ? !result : result;
    }
}

[Serializable]
public class DialogueConditionEndQuest : DialogueCondition
{
    public DialogueConditionEndQuest() { Type = "EndQuest"; }

    public override bool Verify()
    {
        var quest = MainGameManager._questbookManager.GetQuestByCode(TargetCode);
        bool result = (quest == null) ? false : quest.IsCompleted;
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
        bool result = MainGameManager._dialogStatesManager.HaveSaid(PNJName, TargetCode);
        return (Not) ? !result : result;
    }
}

[Serializable]
public class DialogueConditionPossessItem : DialogueCondition
{
    public DialogueConditionPossessItem() { Type = "PossessItem"; }

    public override bool Verify()
    {
        InventoryManager inventory = MainGameManager._inventoryManager;

        bool result = (inventory._items.Exists(x => x.NameCode == TargetCode)
            || inventory._hotbars.Exists(x => x.NameCode == TargetCode)
            || inventory._weapon?.NameCode == TargetCode);

        return (Not) ? !result : result;
    }
}

[Serializable]
public class DialogueConditionPossessMoney : DialogueCondition
{
    public int amount;

    public DialogueConditionPossessMoney() { Type = "PossessMoney"; }

    public override bool Verify()
    {
        bool result = MainGameManager._inventoryManager._money < amount;

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