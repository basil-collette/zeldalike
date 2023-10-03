[System.Serializable]
public class EventGoal : Goal
{
    public EventGoal() { Type = "Event"; }

    protected override void ApplyGoalCheckObserver()
    {
        //EventManager.OnEvent += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        //EventManager.OnEvent -= GoalCheck;
    }

}