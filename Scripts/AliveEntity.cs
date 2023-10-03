using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public abstract class AliveEntity : Entity
{
    public EntityState currentEntityState;
    public float moveSpeed = 1;
    public float nearRadius = 1;
    public List<SenseEnum> imperceptibles;

    [HideInInspector] public bool attacking;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 moveOrientation;
    protected Animator animator;

    protected new void Start()
    {
        base.Start();

        this.direction = Vector3.zero;

        this.animator = GetComponent<Animator>();
        animator?.SetFloat("moveX", 0);
        animator?.SetFloat("moveY", -1);

        SetState(EntityState.idle);
    }

    protected new void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    protected void FixedUpdate()
    {

    }

    public sealed override bool TargetIsNear(Vector3 targetPosition, float radius = 0)
    {
        radius = (radius == 0) ? nearRadius : radius;
        return base.TargetIsNear(targetPosition, radius);
    }

    public void SetState(EntityState state)
    {
        if (currentEntityState != state) currentEntityState = state;

        //Seams to unlock z rotation
        /*
        if (state == EntityState.unavailable)
        {
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }
        */
    }

}
