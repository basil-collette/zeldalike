using System;

[Serializable]
public abstract class Goal
{
    public string Target;
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

    protected virtual void GoalCheck(params object[] observerParams)
    {
        string[] deathParams = observerParams as string[];

        if (Array.Exists(deathParams, (x) => x == Target))
        {
            CurrentAmmount++;

            if (CurrentAmmount == RequiredAmmount)
            {
                IsCompleted = true;
                DisableGoalCheckObserver();
            }
        }
    }

    protected abstract void ApplyGoalCheckObserver();
    protected abstract void DisableGoalCheckObserver();
}