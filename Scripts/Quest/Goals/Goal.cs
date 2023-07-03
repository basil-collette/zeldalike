[System.Serializable]
public abstract class Goal
{
    public string Objective;
    public bool IsCompleted;
    public int RequiredAmmount;
    public int CurrentAmmount;

    public Goal()
    {
        if (!IsCompleted)
        {
            ApplyGoalCheckObserver();
        }
    }

    ~Goal()
    {
        DisableGoalCheckObserver();
    }

    protected abstract void GoalCheck(params object[] observerParams);
    protected abstract void ApplyGoalCheckObserver();
    protected abstract void DisableGoalCheckObserver();
}