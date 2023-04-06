using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : AliveEntity
{
    public int baseAttack = 1;
    public HoldableItem hand;
    public List<HoldableItem> hotbar = new List<HoldableItem>();
    public VectorValue startingPosition;

    PlayerInput playerInputs;

    void Start()
    {
        base.Start();

        transform.position = startingPosition.initalValue;

        SetState(EntityState.walk);

        playerInputs = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        direction = Vector3.zero;

        if (currentEntityState == EntityState.attack)
        {
            Imobilize();
            return;
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        Vector2 joystickInput = playerInputs.actions["Move"].ReadValue<Vector2>();
        if (joystickInput != Vector2.zero)
        {
            direction.x = joystickInput.x;
            direction.y = joystickInput.y;
        }

        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
        }
    }

    public void Attack()
    {
        if (currentEntityState != EntityState.attack
            && currentEntityState != EntityState.unavailable)
        {
            SetState(EntityState.attack);
        }
    }

    void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        if (currentEntityState == EntityState.attack)
        {
            if (!animator.GetBool("attacking"))
            {
                StartCoroutine(AttackCo());
            }
        }

        if (currentEntityState == EntityState.walk)
        {
            if (direction != Vector3.zero)
            {
                direction.Normalize();
                
                //rigidbody.MovePosition(transform.position + (direction.normalized * moveSpeed * Time.fixedDeltaTime));

                rigidbody.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
                                
                animator.SetFloat("moveX", direction.x);
                animator.SetFloat("moveY", direction.y);
                animator.SetBool("moving", true);
            }
            else
            {
                Imobilize();
            }
        }
        else
        {
            Imobilize();
        }
    }

    public IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);

        yield return new WaitForSeconds(.2f);

        SetState(EntityState.walk);
        animator.SetBool("attacking", false);

        /*
        animator.SetBool("attacking", true);
        

        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (IsAttackingAnimation(animStateInfo)
            && animStateInfo.normalizedTime == 1.0f)
        {
            SetState(EntityState.idle);
        }
        */
    }

    public bool IsAttackingAnimation(AnimatorStateInfo animStateInfo)
    {
        return animStateInfo.IsName("attackDown")
            || animStateInfo.IsName("attackLeftn")
            || animStateInfo.IsName("attackRight")
            || animStateInfo.IsName("attackUp");
    }

    void Imobilize()
    {
        direction = Vector3.zero;
        rigidbody.velocity = new Vector2(0, 0);
        animator.SetBool("moving", false);
    }

}