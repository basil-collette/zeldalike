using UnityEngine;

public class ChaseBrain : Brain
{
    public float detectionRange = 4;
    public bool needDirectSee = true;

    protected Vector3 lastPositionKnown;

    private void Start()
    {
        //
    }

    public override Vector3? Think(ThinkParam param)
    {
        Transform target = ((TargetThinkParam)param).target;
        Vector3 targetPos = target.position;

        if (Vector2.Distance(transform.position, targetPos) > detectionRange)
        {
            return Vector3.zero;
        }

        if (!needDirectSee)
        {
            return targetPos;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, (targetPos - transform.position), detectionRange);
        if (hitInfo == null
            || ReferenceEquals(hitInfo.transform.gameObject, target.gameObject))
        {
            lastPositionKnown = targetPos;

            return lastPositionKnown;
        }

        return lastPositionKnown;
    }

    public override short? Behave(BehaveParam param)
    {
        return null;
    }

}
