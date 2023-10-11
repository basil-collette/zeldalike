using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObject/Brains/Itinirary")]
public class PatrolBrain : Brain
{
    public List<Transform> steps = new List<Transform>();
    public int currentStepIndex;
    public bool isLoop = true;

    protected AliveEntity entityComp;
    protected Animator animator;

    private void Start()
    {
        entityComp = GetComponent<AliveEntity>();
        animator = GetComponent<Animator>();
    }

    public override Vector3? Think(ThinkParam? param)
    {
        //On reach the step
        if (GetComponent<Rigidbody2D>().position == (Vector2)steps[currentStepIndex].position)
        {
            if (currentStepIndex + 1 < steps.Count)
            {
                currentStepIndex++;
                return null;
            }

            currentStepIndex = 0;

            if (!isLoop)
                steps.Reverse();
        }

        return steps[currentStepIndex].position;
    }

    public override short? Behave(BehaveParam? param)
    {
        Vector3 targetPos = steps[currentStepIndex].position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            entityComp.moveSpeed * Time.fixedDeltaTime);

        SetAnimation(DirectionHelper.GetDirection(transform.position, targetPos).normalized);

        return null;
    }
    protected void SetAnimation(Vector3 direction)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
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
            if (Vector3.Distance(steps[i].position, GetComponent<Rigidbody2D>().position) > closestDistance)
            {
                closestStepIndex = i;
            }
        }

        return closestStepIndex;
    }

}
