using System;
using UnityEngine;

public class FlueBrain : Brain
{
    public float flueDistance = 2;

    protected Animator animator;
    protected float selfColliderSize;
    protected bool preferClockwise = true;

    private void Start()
    {
        animator = GetComponent<Animator>();

        Vector2 collidArea = GetComponent<BoxCollider2D>().size;
        selfColliderSize = Math.Max(collidArea.x, collidArea.y) / 2;
    }

    public override Vector3? Think(ThinkParam param = null)
    {
        Transform target = ((TargetThinkParam)param).target;
        Vector3 targetPos = target.position;

        float distanceFromTarget = Vector2.Distance(transform.position, targetPos);

        if (distanceFromTarget > flueDistance)
        {
            return Vector3.zero;
        }

        return GetFlueDirection(DirectionHelper.GetDirection(targetPos, transform.position));
    }

    public override short? Behave(BehaveParam param = null)
    {
        Vector3 targetPos = ((TargetPosBehaveParam)param).targetPos;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            GetComponent<AliveEntity>().moveSpeed * Time.fixedDeltaTime);

        SetAnimation(DirectionHelper.GetRelativeAxis(transform.position, targetPos).normalized);

        return null;
    }

    protected void SetAnimation(Vector3 direction)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    protected Vector3 GetFlueDirection(Vector3 direction)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, flueDistance);
        if (hitInfo.collider == null)
        {
            return direction;
        }

        float angle = GetAngleDependingOfObstacleDistance(hitInfo.distance);

        return GetAvoidingAngledDirection(direction, angle);
    }

    public Vector3 GetAvoidingAngledDirection(Vector3 direction, float angle)
    {
        Vector3 clockwiseDirection = DirectionHelper.RotateVector3DirectionByAngle(direction, angle);
        Vector3 antiClockwiseDirection = DirectionHelper.RotateVector3DirectionByAngle(direction, -angle);

        bool clockwiseIsBetter;
        if (preferClockwise)
        {
            clockwiseIsBetter = ColliderHelper.FirstDirectionHaveLessCollisions(clockwiseDirection, antiClockwiseDirection, transform.position, flueDistance);
            preferClockwise = clockwiseIsBetter;
        }
        else
        {
            clockwiseIsBetter = ColliderHelper.FirstDirectionHaveLessCollisions(antiClockwiseDirection, clockwiseDirection, transform.position, flueDistance);
            preferClockwise = !clockwiseIsBetter;
        }

        return ((preferClockwise) ? clockwiseDirection : antiClockwiseDirection) + transform.position;

        //Debug.DrawRay(transform.position, direction, Color.red, 0.01f);
    }

    protected float GetAngleDependingOfObstacleDistance(float colliderDistance)
    {
        float angle = 95f; // max angle degree of side rotation
        float colliderDistancePercentile = 100f - (Math.Max(0, colliderDistance - selfColliderSize) / flueDistance) * 100f; // (100 - result) to invert the percentile
        return angle / 100f * colliderDistancePercentile;
    }

}
