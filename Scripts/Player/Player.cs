using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player : AliveEntity
{
    public int baseAttack = 1;
    public Inventory inventory;

    [Header("Position Settings")]
    public VectorValue startingPosition;
    public VectorValue startingDirection;

    [Header("Dash Settings")]
    public float dashSpeed = 20;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    PlayerInput playerInputs;
    CooldownManager cooldownManager;

    new void Start()
    {
        base.Start();

        playerInputs = GetComponent<PlayerInput>();
        cooldownManager = GetComponent<CooldownManager>();

        transform.position = startingPosition.initalValue;
        animator.SetFloat("moveX", startingDirection.initalValue.x);
        animator.SetFloat("moveY", startingDirection.initalValue.y);

        SetState(EntityState.walk);

        if (inventory.holdedItem != null)
        {
            HoldableItem holded = Instantiate(inventory.holdedItem, transform.position, Quaternion.identity);
            holded.transform.parent = gameObject.transform;
        }
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        if (currentEntityState == EntityState.attack)
        {
            Imobilize();
            return;
        }

        direction = Vector3.zero;

        if (Gamepad.current[GamepadButton.South].wasPressedThisFrame)
        {
            Dash();
            return;
        }

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
            SetOrientation();
        }
    }

    new void FixedUpdate()
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

                float finalSpeed = (cooldownManager.IsAvailable("dashDuration"))
                    ? moveSpeed
                    : dashSpeed;

                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * finalSpeed, direction.y * finalSpeed);

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

    public void SetOrientation()
    {
        this.orientation = this.direction.normalized;
    }

    void Dash()
    {
        if (!cooldownManager.IsAvailable("dashCooldown")
            && cooldownManager.IsAvailable("dashDuration"))
        {
            return;
        }

        if (cooldownManager.IsAvailable("dashCooldown"))
        {
            Action OnLoop = () => { /* CreateDashEffect() */ };
            Action OnEnd = () => { SetState(EntityState.walk); };

            cooldownManager.StartCooldown("dashDuration", dashDuration, OnLoop, OnEnd);
            cooldownManager.StartCooldown("dashCooldown", dashCooldown);
        }
    }

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(.25f);

        animator.SetBool("attacking", false);

        while (currentEntityState == EntityState.unavailable)
        {
            yield return null;
        }
        SetState(EntityState.walk);
    }

    void Imobilize()
    {
        direction = Vector3.zero;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        animator.SetBool("moving", false);
    }

    void CreateDashEffect()
    {
        /*
        Transform dashTransform = Instantiate(GameAssets.i.pfDashEffect, dashPosition, Quaternion.identity);
        dashTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.getAngleFromVector(direction));
        dashTransform.localScale = new Vector3(dashSize / 35f, 1, 1);
        */
    }

    public void RaiseItem()
    {
        if (!animator.GetBool("receivingItem"))
        {
            animator.SetBool("receivingItem", true);
            SetState(EntityState.unavailable);
        }
        else
        {
            animator.SetBool("receivingItem", false);
            SetState(EntityState.walk);
        }
    }

    /*
    void Teleport()
    {
        bool isDashButtonDown = false;
        if (isDashButtonDown)
        {
            float dashAmount = 50f;
            Vector3 dashPosition = transform.position + orientation * dashAmount;

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, dashAmount);
            if (raycast.collider != null)
            {
                dashPosition = raycast.point;
            }

            GetComponent<Rigidbody2D>().MovePosition(dashPosition);
            isDashButtonDown = false;

            CreateDashEffect(dashPosition, Vector2.Distance(GetComponent<Rigidbody2D>().position, dashPosition));
        }
    }
    */

}