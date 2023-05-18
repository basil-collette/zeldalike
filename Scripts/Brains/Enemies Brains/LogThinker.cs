using UnityEngine;

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

        attackCollider = transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();
        chaseBrain = GetComponent<ChaseBrain>();
        sleeperBrain = GetComponent<SleeperBrain>();
        pursueBrain = GetComponent<PursueBrain>();
        attackBrain = GetComponent<AttackBrain>();
        
        SetState(EntityState.sleep);
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        direction = chaseBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;

        //On target not inside chaseRadius
        if (sleeperBrain.Think(new SleeperThinkParam(direction != Vector3.zero)) == Vector3.zero)
        {
            return;
        }

        if (currentEntityState == EntityState.sleep)
        {
            return;
        }

        direction = pursueBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;
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

    new void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

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
                    attackBrain.Behave(para);
                }
                break;

            case EntityState.walk:
                pursueBrain.Behave(new TargetPosBehaveParam() { targetPos = this.direction });
                break;

            default: break;
        }
    }

}
