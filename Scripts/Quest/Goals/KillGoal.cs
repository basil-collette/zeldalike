using UnityEngine;

[System.Serializable]
public class KillGoal : Goal
{
    public KillGoal() { Type = "Kill"; }

    protected override void ApplyGoalCheckObserver()
    {
        Health.OnDeath += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        Health.OnDeath -= GoalCheck;
    }
}