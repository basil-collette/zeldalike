using System;

[Serializable]
public class DiscussionGoal : Goal
{
    public DiscussionGoal() { Type = "Discussion"; }

    protected override void ApplyGoalCheckObserver()
    {
        DialogueManager.OnDiscuss += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        DialogueManager.OnDiscuss -= GoalCheck;
    }

}