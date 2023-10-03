[System.Serializable]
public class CollectGoal : Goal
{
    public CollectGoal() { Type = "Collect"; }

    protected override void ApplyGoalCheckObserver()
    {
        InventoryManager.OnObtain += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        InventoryManager.OnObtain -= GoalCheck;
    }

}