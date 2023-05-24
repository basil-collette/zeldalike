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
            GetComponent<Animator>().SetBool("targeting", false);
            return Vector3.zero;
        }

        if (!needDirectSee)
        {
            GetComponent<Animator>().SetBool("targeting", true);
            return targetPos;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, (targetPos - transform.position), detectionRange);
        if (hitInfo == false
            || ReferenceEquals(hitInfo.transform.gameObject, target.gameObject))
        {
            lastPositionKnown = targetPos;

            GetComponent<Animator>().SetBool("targeting", true);
            return lastPositionKnown;
        }

        GetComponent<Animator>().SetBool("targeting", true);
        return lastPositionKnown;
    }

    public override short? Behave(BehaveParam param)
    {
        return null;
    }

}
