using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Itinirary")]
public class PatrolBrain : Brain
{
    public List<Vector2> steps = new List<Vector2>();
    public int currentStepIndex;
    public bool isLoop = true;

    protected Rigidbody2D rigidbody;
    protected AliveEntity entityComp;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.entityComp = GetComponent<AliveEntity>();
    }

    public override Vector3? Think(ThinkParam param)
    {
        //On reach the step
        if (rigidbody.position == steps[currentStepIndex])
        {
            if (currentStepIndex < steps.Count)
            {
                currentStepIndex++;
                return null;
            }

            currentStepIndex = 0;

            if (!isLoop)
                steps.Reverse();
        }

        return steps[currentStepIndex];
    }

    public override short? Behave(BehaveParam param)
    {
        transform.position = Vector2.MoveTowards(transform.position, steps[currentStepIndex], entityComp.moveSpeed * Time.fixedDeltaTime);

        return null;
    }

    public void SetStepIndex(int stepIndex)
    {
        this.currentStepIndex = stepIndex;
    }

    public void resetStepIndex()
    {
        SetStepIndex(getClosestItineraryIndex());
    }

    //loop through initeraries and find index of the closest step
    int getClosestItineraryIndex()
    {
        float closestDistance = 0f;
        int closestStepIndex = 0;

        for (int i = 0; i < steps.Count; i++)
        {
            if (Vector3.Distance(steps[i], rigidbody.position) > closestDistance)
            {
                closestStepIndex = i;
            }
        }

        return closestStepIndex;
    }

}
