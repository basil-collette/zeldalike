using UnityEngine;

public class LogThinker : Bot
{
    ChaseBrain chaseBrain;
    SleeperBrain sleeperBrain;
    PursueBrain pursueBrain;
    //public AttackBrain attackBrain;

    new void Start()
    {
        base.Start();

        this.chaseBrain = GetComponent<ChaseBrain>();
        this.sleeperBrain = GetComponent<SleeperBrain>();
        this.pursueBrain = GetComponent<PursueBrain>();
        //this.attackBrain = GetComponent<AttackBrain>();

        SetState(EntityState.sleep);
    }

    void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        direction = chaseBrain.Think() ?? Vector3.zero;
        
        direction = sleeperBrain.Think(new SleeperThinkParam(direction != Vector3.zero)) ?? Vector3.zero;

        //On target not inside chaseRadius
        if (direction == Vector3.zero)
        {
            return;
        }

        if (currentEntityState != EntityState.sleep)
        {
            direction = pursueBrain.Think() ?? Vector3.zero;
            //On target not inside attackRadius
            if (direction != Vector3.zero)
            {
                SetState(EntityState.walk);
                return;
            }

            animator.SetBool("moving", false);
            SetState(EntityState.idle);
        }
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
                    //AttackBehaveParam params = new AttackBehaveParam();
                    //attackBrain.Behave(params);
                    animator.SetBool("attacking", true);
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
