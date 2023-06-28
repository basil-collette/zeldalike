using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    [SerializeReference] public List<Goal> Goals;
    public bool IsCompleted;
    public Rewards Rewards;
}