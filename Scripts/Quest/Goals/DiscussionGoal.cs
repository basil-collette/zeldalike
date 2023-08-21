using System;

[Serializable]
public class DiscussionGoal : Goal
{
    protected override void ApplyGoalCheckObserver()
    {
        DialogueManager.OnDiscuss += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        DialogueManager.OnDiscuss -= GoalCheck;
    }

}