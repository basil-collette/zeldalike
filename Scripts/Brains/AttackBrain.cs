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

        //DirectionHelper.PointTo(attackThinkParam.attackCollider.transform, attackThinkParam.targetPos, -90);

        return (canAttack) ? attackThinkParam.targetPos : Vector3.zero;
    }

    public override short? Behave(BehaveParam param = null)
    {
        if (canAttack)
        {
            canAttack = false;

            AttackBehaveParam attackBehaveParam = param as AttackBehaveParam;

            StartCoroutine(AttackCooldownCo(attackBehaveParam.attackDuration, attackBehaveParam.cooldown));
        }
        return null;
    }

    protected IEnumerator AttackCooldownCo(float attackDuration, float attackCooldown)
    {
        
        animator.SetBool("attacking", true);

        Vector3 direction = DirectionHelper.GetDirection(transform.position, GetComponent<Bot>().target.position).normalized;

        yield return new WaitForSeconds(1);

        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * 10, direction.y * 10);

        yield return new WaitForSeconds(attackDuration);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetBool("attacking", false);
        GetComponent<AliveEntity>().SetState(EntityState.idle);

        yield return new WaitForSeconds(attackCooldown);
        
        canAttack = true;
    }

}