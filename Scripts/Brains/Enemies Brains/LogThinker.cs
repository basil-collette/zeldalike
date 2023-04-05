using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LogThinker : Bot
{
    protected ChaseBrain chaseBrain;
    protected SleeperBrain sleeperBrain;
    protected PursueBrain pursueBrain;
    protected AttackBrain attackBrain;
    protected BoxCollider2D attackCollider;

    new void Start()
    {
        base.Start();

        this.chaseBrain = GetComponent<ChaseBrain>();
        this.sleeperBrain = GetComponent<SleeperBrain>();
        this.pursueBrain = GetComponent<PursueBrain>();
        this.attackBrain = GetComponent<AttackBrain>();

        this.attackCollider = transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();

        SetState(EntityState.sleep);
    }

    void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        direction = chaseBrain.Think() ?? Vector3.zero;

        //On target not inside chaseRadius
        if (sleeperBrain.Think(new SleeperThinkParam(direction != Vector3.zero)) == Vector3.zero)
        {
            return;
        }

        if (currentEntityState == EntityState.sleep)
        {
            return;
        }

        direction = pursueBrain.Think() ?? Vector3.zero;
        //On target not inside attackRadius
        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
            return;
        }

        AttackThinkParam para = new AttackThinkParam()
        {
            targetPos = target.position,
            attackCollider = this.attackCollider
        };
        if (attackBrain.Think(para) != Vector3.zero)
        {
            SetState(EntityState.attack);
            animator.SetBool("moving", false);
            return;
        }
        
        SetState(EntityState.idle);
    }

    void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        rigidbody.velocity = Vector3.zero;

        switch (currentEntityState)
        {
            case EntityState.sleep:
                if (direction != Vector3.zero)
                {
                    sleeperBrain.Behave();
                }
                break;

            case EntityState.attack:
                if (!animator.GetBool("attacking"))
                {
                    AttackBehaveParam para = new AttackBehaveParam()
                    {
                        attackDuration = 1,
                        cooldown = 1,
                        attackCollider = this.attackCollider
                    };
                    attackBrain.Behave();
                }
                break;

            case EntityState.walk:
                pursueBrain.Behave(new PursueBehaveParam()
                    { direction = this.direction });
                break;

            default: break;
        }
    }

}
