using Assets.Scripts.Manager;
using System.Collections;
using UnityEngine;

public class ExplosiveBeanThinker : Bot
{
    [SerializeField] float sprintSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionEffect;

    protected DetectorBrain chaseBrain;
    protected PatrolBrain patrolBrain;
    protected PursueBrain pursueBrain;

    protected AliveEntity aliveEntity;
    protected float originMoveSpeed;

    protected CooldownManager cooldownManager;
    protected bool seePlayer;

    new void Start()
    {
        base.Start();

        chaseBrain = GetComponent<DetectorBrain>();
        patrolBrain = GetComponent<PatrolBrain>();
        pursueBrain = GetComponent<PursueBrain>();

        aliveEntity = GetComponent<AliveEntity>();
        originMoveSpeed = aliveEntity.moveSpeed;

        cooldownManager = GetComponent<CooldownManager>();

        SetState(EntityState.idle);
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable
            || currentEntityState == EntityState.attack)
            return;

        direction = chaseBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;
        if (direction == Vector3.zero)
        {
            aliveEntity.moveSpeed = originMoveSpeed;
            seePlayer = false;

            direction = patrolBrain.Think(null) ?? Vector3.zero;
            if (direction != Vector3.zero)
            {
                SetState(EntityState.walk);
                return;
            }
        }

        seePlayer = true;
        aliveEntity.moveSpeed = sprintSpeed;

        direction = pursueBrain.Think(new TargetThinkParam() { target = this.target }) ?? Vector3.zero;
        //On target not inside attackRadius
        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
            return;
        }

        if (!animator.GetBool("attacking"))
        {
            SetState(EntityState.attack);
            Imobilize();
            animator.SetTrigger("explode");
        }        
    }

    new void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        switch (currentEntityState)
        {
            case EntityState.walk:
                if (seePlayer)
                {
                    pursueBrain.Behave(new TargetPosBehaveParam() { targetPos = this.direction });
                }
                else
                {
                    patrolBrain.Behave(null);
                }
                break;

            case EntityState.attack:
                
                break;

            default: break;
        }
    }

    public void ExplodeAttackEnd()
    {
        FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>().PlayEffect("explosion");

        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
