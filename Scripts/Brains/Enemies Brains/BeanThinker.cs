using Assets.Scripts.Manager;
using System.Collections;
using UnityEngine;

public class BeanThinker : Bot
{
    protected DetectorBrain chaseBrain;
    protected FlueBrain flueBrain;
    protected ProjectileAttackBrain projectileAttackBrain;
    protected CooldownManager cooldownManager;

    new void Start()
    {
        base.Start();

        chaseBrain = GetComponent<DetectorBrain>();
        flueBrain = GetComponent<FlueBrain>();
        projectileAttackBrain = GetComponent<ProjectileAttackBrain>();
        cooldownManager = GetComponent<CooldownManager>();

        SetState(EntityState.sleep);
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable
            || currentEntityState == EntityState.attack)
            return;

        direction = chaseBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;
        if (direction == Vector3.zero)
        {
            // wandering !
            return;
        }

        direction = flueBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;
        //On target on flue needing range
        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
            return;
        }

        AttackThinkParam para = new AttackThinkParam()
        {
            targetPos = target.position,
            attackCollider = GetComponent<BoxCollider2D>()
        };
        if (projectileAttackBrain.Think(para) != Vector3.zero)
        {
            SetState(EntityState.attack);
            animator.SetBool("moving", false);
            return;
        }

        SetState(EntityState.idle);
    }

    new void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        switch (currentEntityState)
        {
            case EntityState.walk:
                flueBrain.Behave(new TargetPosBehaveParam() { targetPos = this.direction });
                break;

            case EntityState.attack:
                if (!animator.GetBool("attacking"))
                {
                    AttackBehaveParam param = new AttackBehaveParam()
                    {
                        targetTransform = target.transform
                    };
                    projectileAttackBrain.Behave(param);
                }
                break;

            default: break;
        }
    }

}
