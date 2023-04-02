using UnityEngine;

public class SleeperBrain : Brain
{
    protected ChaseBrain chaseBrain;
    protected Animator animator;
    protected AliveEntity entityComp;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
        this.entityComp = GetComponent<AliveEntity>();
    }

    public override Vector3? Think(ThinkParam param = null)
    {
        SleeperThinkParam sleeperParam = param as SleeperThinkParam;

        if (!sleeperParam.wakeUp)
        {
            entityComp.SetState(EntityState.sleep);
            animator.SetBool("awake", false);
            animator.SetBool("moving", false);
            return Vector3.zero;
        }

        return new Vector3(1, 1);
    }

    public override short? Behave(BehaveParam param = null)
    {
        animator.SetBool("awake", true);

        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName("logSleeping")
            || animStateInfo.IsName("logWakeUp")
            && animStateInfo.normalizedTime < 1.0f)
        {
            return null;
        }

        entityComp.SetState(EntityState.idle);
        return 1;
    }

}