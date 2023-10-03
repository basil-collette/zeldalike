using System;

[System.Serializable]
public class LocationGoal : Goal
{
    public LocationGoal() { Type = "Location"; }

    //public string Target;

    protected override void ApplyGoalCheckObserver()
    {
        //Health.OnDeath += GoalCheck;
    }

    protected override void DisableGoalCheckObserver()
    {
        //Health.OnDeath -= GoalCheck;
    }

    protected override void GoalCheck(params object[] observerParams)
    {
        string[] deathParams = observerParams as string[];

        /*if (Array.Exists(deathParams, (x) => x == Target))
        {
            CurrentAmount++;

            if (CurrentAmount == RequiredAmount)
            {
                IsCompleted = true;
                DisableGoalCheckObserver();
            }
        }*/
    }

}