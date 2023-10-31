[System.Serializable]
public class EventGoal : Goal
{
    public EventGoal() { Type = "Event"; }

    protected override void ApplyGoalCheckObserver()
    {
        StoryEventManager.OnEvent += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        StoryEventManager.OnEvent -= GoalCheck;
    }

    void GoalCheck(string eventName)
    {
        base.GoalCheck(new string[] { eventName });
    }

}