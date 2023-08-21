[System.Serializable]
public class CollectGoal : Goal
{
    protected override void ApplyGoalCheckObserver()
    {
        Inventory.OnObtain += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        Inventory.OnObtain -= GoalCheck;
    }

}