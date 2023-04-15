using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

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

        if (currentEntityState == EntityState.attack)
        {
            Imobilize();
            return;
        }

        direction = Vector3.zero;

        if (Gamepad.current[GamepadButton.East].wasPressedThisFrame)
        {
            if (currentEntityState != EntityState.unavailable)
            {
                SetState(EntityState.attack);
            }
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

    void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        if (currentEntityState == EntityState.attack)
        {
            if (!animator.GetBool("attacking"))
            {
                animator.SetBool("attacking", true);
                StartCoroutine(AttackCo());
            }
            return;
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

    /*
    void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        if (currentEntityState != EntityState.walk
            || direction == Vector3.zero)
        {
            Imobilize();
        }

        if (currentEntityState == EntityState.attack
            && !animator.GetBool("attacking"))
        {
            StartCoroutine(AttackCo());
        }

        direction.Normalize();

        //rigidbody.MovePosition(transform.position + (direction.normalized * moveSpeed * Time.fixedDeltaTime));

        rigidbody.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetBool("moving", true);
    }
    */

    public IEnumerator AttackCo()
    {
        yield return new WaitForSecondsRealtime(.25f);

        animator.SetBool("attacking", false);

        if (currentEntityState != EntityState.unavailable)
            SetState(EntityState.walk);
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