using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    [SerializeReference] public List<Goal> Goals;
    public bool IsCompleted;
    public Rewards Rewards;
    [HideInInspector] public Quest _Quest;

    public bool VerifyIfCompleted()
    {
        if (Goals.Exists(x => !x.IsCompleted))
        {
            IsCompleted = false;
        }
        else
        {
            IsCompleted = true;
            MainGameManager._inventoryManager.GetReward(Rewards);
            _Quest.VerifyIfCompleted();
        }

        return IsCompleted;
    }

}