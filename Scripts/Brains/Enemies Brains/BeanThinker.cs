using UnityEngine;

public class BeanThinker : Bot
{
    protected ChaseBrain chaseBrain;
    protected PursueBrain pursueBrain;
    protected AttackBrain attackBrain;

    new void Start()
    {
        base.Start();

        chaseBrain = GetComponent<ChaseBrain>();
        pursueBrain = GetComponent<PursueBrain>();
        attackBrain = GetComponent<AttackBrain>();
        
        SetState(EntityState.sleep);
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        direction = chaseBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;

        direction = pursueBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;

        //On target not inside attackRadius
        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
            return;
        }

        /*
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
        */
        
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
                pursueBrain.Behave(new TargetPosBehaveParam() { targetPos = this.direction });
                break;

            default: break;
        }
    }

}
