using Assets.Scripts.Manager;
using System.Collections;
using UnityEngine;

public class ProjectileAttackBrain : Brain
{
    public float Cooldown = 5;
    public AnimationClip attackAnim;
    public Projectile projectile;
    public string attackClipName;

    protected Animator animator;
    protected CooldownManager cooldownManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cooldownManager = GetComponent<CooldownManager>();
    }

    public override Vector3? Think(ThinkParam param)
    {
        AttackThinkParam attackThinkParam = param as AttackThinkParam;

        //DirectionHelper.GetDirection(transform.position, attackThinkParam.targetPos);

        return (cooldownManager.IsAvailable("attackCooldown"))
            ? attackThinkParam.targetPos
            : Vector3.zero;
    }

    public override short? Behave(BehaveParam param = null)
    {
        if (!cooldownManager.IsAvailable("attackCooldown"))
        {
            return null;
        }

        cooldownManager.StartCooldown("attackCooldown", Cooldown);

        AttackBehaveParam attackBehaveParam = param as AttackBehaveParam;

        StartCoroutine(AttackCooldownCo(attackBehaveParam));

        return null;
    }

    void ThrowProjectile(Vector3 targetPos)
    {
        Vector2 collidArea = GetComponent<BoxCollider2D>().size;

        Vector3 axis = DirectionHelper.GetAxis(new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0));

        Vector3 instantiatePos = transform.position;
        if (axis == Vector3.left) { instantiatePos.x -= collidArea.x / 2; instantiatePos.y -= collidArea.y / 2; }
        else if (axis == Vector3.right) { instantiatePos.x += collidArea.x / 2; instantiatePos.y -= collidArea.y / 2; }
        else if(axis == Vector3.up) { instantiatePos.y += collidArea.y / 2; }
        else if(axis == Vector3.down) { instantiatePos.y -= collidArea.y / 2; }

        Projectile instanciateProjectile = Instantiate(projectile, instantiatePos, Quaternion.identity);
        instanciateProjectile.direction = DirectionHelper.GetDirection(transform.position, targetPos);

        FindGameObjectHelper.FindInactiveObjectByName("SoundManager").GetComponent<SoundManager>().PlayEffect(attackClipName);
    }

    IEnumerator AttackCooldownCo(AttackBehaveParam attackBehaveParam)
    {
        animator.SetBool("attacking", true);

        //float durationAnim = attackAnim.length;

        yield return new WaitForSeconds(attackAnim.length);

        Vector3 finalPos = (attackBehaveParam.targetTransform != null)
            ? attackBehaveParam.targetTransform.position
            : attackBehaveParam.targetPos;

        ThrowProjectile(finalPos);

        animator.SetBool("attacking", false);
        GetComponent<AliveEntity>().SetState(EntityState.idle);
    }

}