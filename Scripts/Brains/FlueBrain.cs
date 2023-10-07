using Assets.Scripts.Manager;
using System;
using UnityEngine;

public class FlueBrain : Brain
{
    public float startingFlueDistance = 2;
    public float flueCooldown = 2;
    public float flueDuration = 1;

    protected Animator animator;
    protected float selfColliderSize;
    protected bool preferClockwise = true;
    protected CooldownManager cooldownManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cooldownManager = GetComponent<CooldownManager>();

        Vector2 collidArea = GetComponent<CapsuleCollider2D>().size;
        selfColliderSize = Math.Max(collidArea.x, collidArea.y) / 2;
    }

    public override Vector3? Think(ThinkParam param = null)
    {
        Vector3 targetPos = (param as TargetThinkParam).target.position;
        Vector3 direction = DirectionHelper.GetDirection(targetPos, transform.position);

        float distanceFromTarget = Vector2.Distance(transform.position, targetPos);

        //immobilize if target is far OR if flueCooldown is not finished
        if ((distanceFromTarget > startingFlueDistance || !cooldownManager.IsAvailable("flueCooldown"))
            && cooldownManager.IsAvailable("flueDuration"))
        {
            return Vector3.zero;
        }

        if (cooldownManager.IsAvailable("flueCooldown"))
        {
            cooldownManager.StartCooldown("flueDuration", flueDuration);
            cooldownManager.StartCooldown("flueCooldown", flueCooldown);
        }

        return GetFlueDirection(direction);
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
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, startingFlueDistance);
        if (hitInfo.collider == null)
        {
            return DirectionHelper.GetPosAfterDirection(transform.position, direction);
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
            clockwiseIsBetter = ColliderHelper.FirstDirectionHaveLessCollisions(clockwiseDirection, antiClockwiseDirection, transform.position, startingFlueDistance);
            preferClockwise = clockwiseIsBetter;
        }
        else
        {
            clockwiseIsBetter = ColliderHelper.FirstDirectionHaveLessCollisions(antiClockwiseDirection, clockwiseDirection, transform.position, startingFlueDistance);
            preferClockwise = !clockwiseIsBetter;
        }

        return ((preferClockwise) ? clockwiseDirection : antiClockwiseDirection) + transform.position;

        //Debug.DrawRay(transform.position, direction, Color.red, 0.01f);
    }

    protected float GetAngleDependingOfObstacleDistance(float colliderDistance)
    {
        float angle = 95f; // max angle degree of side rotation
        float colliderDistancePercentile = 100f - (Math.Max(0, colliderDistance - selfColliderSize) / startingFlueDistance) * 100f; // (100 - result) to invert the percentile
        return angle / 100f * colliderDistancePercentile;
    }

}
