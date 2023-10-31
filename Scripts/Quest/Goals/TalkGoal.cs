using System;

[System.Serializable]
public class TalkGoal : Goal
{
    public TalkGoal() { Type = "Talk"; }

    protected override void ApplyGoalCheckObserver()
    {
        DialogueManager.OnDiscuss += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        DialogueManager.OnDiscuss -= GoalCheck;
    }

    protected override void GoalCheck(params object[] observerParams)
    {
        string[] discussParams = observerParams as string[];

        if (Array.Exists(discussParams, (x) => x == Target))
        {
            CurrentAmount++;

            if (CurrentAmount == RequiredAmount)
            {
                IsCompleted = true;
                DisableGoalCheckObserver();
            }
        }
    }

}