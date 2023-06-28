[System.Serializable]
public abstract class Goal // : MonoBehaviour
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

    /*
    private void Start()
    {
        if (!IsCompleted)
        {
            ApplyGoalCheckObserver();
        }
    }

    private void OnDestroy()
    {
        DisableGoalCheckObserver();
    }
    */

    protected abstract void GoalCheck(params object[] observerParams);
    protected abstract void ApplyGoalCheckObserver();
    protected abstract void DisableGoalCheckObserver();
}