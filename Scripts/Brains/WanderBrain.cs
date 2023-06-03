using Assets.Scripts.Manager;
using UnityEngine;

public class WanderBrain : Brain
{
    public float wanderCooldown = 4;
    public float wanderDuration = 2;

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

    protected void SetAnimation(Vector3 direction)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

}
