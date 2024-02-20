using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using System.Collections;
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

    public virtual void Imobilize()
    {
        direction = Vector3.zero;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        animator.SetBool("moving", false);
    }

    public void Fall(Vector3 fallPos, Vector3 respawnPos)
    {
        Vector3 dir = DirectionHelper.GetDirection(transform.position, fallPos).normalized;

        GetComponent<Rigidbody2D>().velocity = dir;

        StartCoroutine(FallCo(fallPos, respawnPos));
    }

    protected virtual IEnumerator FallCo(Vector3 fallPos, Vector3 respawnPos)
    {
        SetState(EntityState.unavailable);
        Imobilize();

        MainGameManager._soundManager.PlayEffect("falling");
        
        animator.SetTrigger("fall");

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fall")) yield return null;

        transform.position = fallPos;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        transform.position = respawnPos;
        SetState(EntityState.idle);

        GetComponent<Health>().Hit(gameObject, new List<Effect> { new Effect(EffectTypeEnum.neutral, 0.25f) }, "Player");
    }

}
