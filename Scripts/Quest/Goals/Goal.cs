using System;
using UnityEngine;

[Serializable]
public abstract class Goal
{
    public string Target;
    public string Objective;
    public bool IsCompleted;
    public int RequiredAmount;
    public int CurrentAmount;
    [HideInInspector] public QuestStep _QuestStep;

    #if UNITY_EDITOR
        [ShowOnly]
    #endif
    public string Type;

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
        string[] goalsParams = observerParams as string[];

        if (Array.Exists(goalsParams, (x) => x == Target))
        {
            CurrentAmount++;

            if (CurrentAmount >= RequiredAmount)
            {
                IsCompleted = true;
                _QuestStep.VerifyIfCompleted();
                DisableGoalCheckObserver();
            }
        }
    }

    protected abstract void ApplyGoalCheckObserver();
    protected abstract void DisableGoalCheckObserver();

}