using UnityEngine;

[System.Serializable]
public abstract class Goal : MonoBehaviour
{
    public string Description;
    public int RequiredAmmount;
    public int CurrentAmmount;
    public bool IsCompleted;
    public Reward[] Rewards;

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

    protected abstract void GoalCheck(params object[] observerParams);
    protected abstract void ApplyGoalCheckObserver();
    protected abstract void DisableGoalCheckObserver();
}