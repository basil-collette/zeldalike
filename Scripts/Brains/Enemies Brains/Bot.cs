using UnityEngine;

public abstract class Bot : AliveEntity
{
    public Transform target;
    public float chaseRadius = 6f;
    public float attackRadius = 1f;
    public float health = 1;

    protected bool targetting;

    protected new void Start()
    {
        base.Start();

        Health healthComp = GetComponent<Health>();
        if (healthComp != null)
        {
            healthComp.health = new FloatValue()
             {
                 initialValue = this.health,
                 RuntimeValue = this.health
             };
        }

        this.targetting = false;
        //target = FindObjectOfType<Player>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;

        this.nearRadius = this.attackRadius;
    }

    protected new void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool TargetIsOnAttackRadius()
    {
        return base.TargetIsNear(target.position, attackRadius);
    }

}
