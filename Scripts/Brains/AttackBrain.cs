using System.Collections;
using UnityEngine;

public class AttackBrain : Brain
{
    public float Cooldown = 2;

    protected Animator animator;
    protected bool canAttack = true;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public override Vector3? Think(ThinkParam param)
    {
        AttackThinkParam attackThinkParam = param as AttackThinkParam;

        DirectionHelper.PointTo(attackThinkParam.attackCollider.transform, attackThinkParam.targetPos);

        return (canAttack) ? attackThinkParam.targetPos : Vector3.zero;
    }

    public override short? Behave(BehaveParam param = null)
    {
        if (canAttack)
        {
            AttackBehaveParam attackBehaveParam = param as AttackBehaveParam;

            StartCoroutine(AttackCooldownCo(attackBehaveParam.attackDuration));
        }
        return null;
    }

    protected IEnumerator AttackCooldownCo(float attackDuration)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

}