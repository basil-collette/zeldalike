using Type = Assets.Scripts.Enums.ConditionTypeEnum;
using System;

[Serializable]
public class Condition
{
    public Type Type;
    public string Code;
    public bool Not = false;

    public bool Verify()
    {
        bool result;

        switch (Type)
        {
            case Type.StartedQuest:
                result = MainGameManager._questManager.GetQuestByCode(Code) != null;
                break;

            case Type.EndQuest:
                var quest = MainGameManager._questManager.GetQuestByCode(Code);
                result = (quest == null) ? false : quest.IsCompleted;
                break;

            case Type.AnySaid:
                result = MainGameManager._dialogStatesManager.AnyHaveSaid(Code);
                break;

            case Type.PossessItem:
                result = MainGameManager._inventoryManager.HasByCode(Code);
                break;

            case Type.PossessMoney:
                result = MainGameManager._inventoryManager._money >= int.Parse(Code);
                break;

            case Type.Location:
                result = MainGameManager._storyEventManager._mapDiscovery.Exists(x => x == Code);
                break;

            case Type.Event:
                result = MainGameManager._storyEventManager._scenario.Exists(x => x == Code);
                break;

            default: throw new Exception("Type not added to enum ConditionType");
        }

        return (Not) ? !result : result;
    }

    public static bool VerifyAll(Condition[] conditions)
    {
        foreach (Condition con in conditions)
        {
            if (!con.Verify()) return false;
        }

        return true;
    }

}