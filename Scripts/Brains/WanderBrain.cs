using Assets.Scripts.Manager;
using UnityEngine;

public class WanderBrain : Brain
{
    public float wanderCooldown = 4;
    public float wanderDuration = 2;
    public float wanderRadius = 1;
    public float turnSpeed = 2;

    protected Animator animator;
    protected CooldownManager cooldownManager;
    protected PerlinNoise perlinNoise;
    protected Vector3 anchorPoisition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cooldownManager = GetComponent<CooldownManager>();
        perlinNoise = new PerlinNoise();

        anchorPoisition = GetComponent<AliveEntity>().anchorPosition;
    }

    public override Vector3? Think(ThinkParam param = null)
    {
        if (!cooldownManager.IsAvailable("wanderCooldown")
            && cooldownManager.IsAvailable("flueDuration"))
        {
            return Vector3.zero;
        }

        if (cooldownManager.IsAvailable("wanderCooldown"))
        {
            cooldownManager.StartCooldown("wanderDuration", wanderDuration);
            cooldownManager.StartCooldown("wanderCooldown", wanderCooldown);
        }

        float x = perlinNoise.Get1DNoiseAtX(Time.time);
        float y = perlinNoise.Get1DNoiseAtY(Time.time);
        Debug.Log(new Vector3(x, y, 0));
        return new Vector3(x, y, 0);
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

    public Vector3 GetWanderingDirection()
    {
        Vector3 wanderDirection = Vector3.zero;

        float noise = perlinNoise.Get1DNoiseAtX(Time.time);

        float wanderDelta = 1.0f * Mathf.PI * noise * Time.deltaTime;

        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            Vector3 to = transform.position + wanderDirection * 20;

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, to);
            if (hitInfo)
                wanderDirection = hitInfo.normal;
        }

        if (wanderDirection.magnitude == 0)
        {
            wanderDirection = new Vector2(Mathf.Cos(Random.Range(-Mathf.PI, Mathf.PI)), Mathf.Sin(Random.Range(-Mathf.PI, Mathf.PI)));
        }

        Vector3 spawnDisplacement = anchorPoisition - transform.position;

        float outerRadius = 2.0f * wanderRadius;
        float displacementWeight = DirectionHelper.MapValue(spawnDisplacement.magnitude, wanderRadius, outerRadius);
        float angleDiff = Vector2.Angle(wanderDirection, spawnDisplacement) * displacementWeight;

        angleDiff += wanderDelta;
        angleDiff = DirectionHelper.WrapAngle(angleDiff, -Mathf.PI, Mathf.PI);

        float rot = Mathf.Clamp(angleDiff, -turnSpeed * Time.deltaTime, turnSpeed * Time.deltaTime);

        return DirectionHelper.RotateVector3DirectionByAngle(wanderDirection, rot);
        //return wanderDirection * rotation;
    }

    protected void SetAnimation(Vector3 direction)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

}
