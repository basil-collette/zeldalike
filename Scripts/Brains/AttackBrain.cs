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

        DirectionHelper.LookAt(attackThinkParam.attackCollider.transform, attackThinkParam.targetPos);

        return (canAttack) ? attackThinkParam.targetPos : Vector3.zero;
    }

    public override short? Behave(BehaveParam param = null)
    {
        AttackBehaveParam attackBehaveParam = param as AttackBehaveParam;
        
        //RunCooldownCo(attackBehaveParam.attackCollider, attackBehaveParam.attackDuration, attackBehaveParam.cooldown);

        return null;
    }

    protected IEnumerator RunCooldownCo(Collider2D attackCollider, float attackDuration, float? cooldown = null)
    {
        animator.SetBool("attacking", true);
        attackCollider.enabled = true;
        canAttack = false;

        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;
        animator.SetBool("attacking", false);

        float finalCooldown = ((cooldown != null) ? (float)cooldown : this.Cooldown) - attackDuration;
        yield return new WaitForSeconds(finalCooldown);
        canAttack = true;
    }

}