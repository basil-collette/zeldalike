using UnityEngine;

public class LogThinker : Bot
{
    protected DetectorBrain chaseBrain;
    protected SleeperBrain sleeperBrain;
    protected PursueBrain pursueBrain;
    protected AttackBrain attackBrain;
    protected BoxCollider2D attackCollider;

    new void Start()
    {
        base.Start();

        attackCollider = transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();
        chaseBrain = GetComponent<DetectorBrain>();
        sleeperBrain = GetComponent<SleeperBrain>();
        pursueBrain = GetComponent<PursueBrain>();
        attackBrain = GetComponent<AttackBrain>();
        
        SetState(EntityState.sleep);
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable || currentEntityState == EntityState.attack)
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

        //GetComponent<Rigidbody2D>().velocity = Vector3.zero;

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
                    AttackBehaveParam param = new AttackBehaveParam()
                    {
                        attackDuration = 0.4f,
                        cooldown = 2,
                        attackCollider = this.attackCollider
                    };
                    attackBrain.Behave(param);
                }
                break;

            case EntityState.walk:
                pursueBrain.Behave(new TargetPosBehaveParam() { targetPos = this.direction });
                break;

            default: break;
        }
    }

}
