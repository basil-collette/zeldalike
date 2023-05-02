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

    public float dashSpeed = 20;
    public float dashTimeLength = 0.2f;
    public float dashCooldown = 0.5f;
    float dashCounter = 0;
    float dashCooldownCounter = 0;

    void Start()
    {
        base.Start();

        playerInputs = GetComponent<PlayerInput>();

        transform.position = startingPosition.initalValue;

        SetState(EntityState.walk);
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

        if (Gamepad.current[GamepadButton.South].wasPressedThisFrame
            && dashCounter <= 0)
        {
            StartCoroutine(DashCo());
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

                if (dashCounter <= 0)
                {
                    rigidbody.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
                }
                                
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

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(.25f);

        animator.SetBool("attacking", false);

        if (currentEntityState != EntityState.unavailable)
            SetState(EntityState.walk);
    }

    void Imobilize()
    {
        direction = Vector3.zero;
        rigidbody.velocity = new Vector2(0, 0);
        animator.SetBool("moving", false);
    }

    void DashUpdate()
    {
        if (dashCounter > 0)
        {
            rigidbody.velocity = new Vector3(orientation.x * dashSpeed, orientation.y * dashSpeed);
        }

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }

        if (Gamepad.current[GamepadButton.South].wasPressedThisFrame
            && dashCounter <= 0)
        {
            //set collision active false
            dashCounter = dashTimeLength;
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                //set colission active true
                dashCounter = 0;
                dashCooldownCounter = dashCooldown;
            }
        }
    }

    IEnumerator DashCo()
    {
        SetState(EntityState.unavailable);
        //set colission active false GetComponent<BoxCollider2D>()

        dashCounter = dashTimeLength;

        while (dashCounter > 0)
        {
            rigidbody.velocity = new Vector3(orientation.x * dashSpeed, orientation.y * dashSpeed);

            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                //set colission active true
                dashCounter = 0;
            }

            yield return null;
        }

        SetState(EntityState.walk);
    }

    void Teleport()
    {
        bool isDashButtonDown = false;
        if (isDashButtonDown)
        {
            float dashAmount = 50f;
            Vector3 dashPosition = transform.position + orientation * dashAmount;

            /*
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, dashAmount);
            if (raycast.collider != null)
            {
                dashPosition = raycast.point;
            }
            */

            rigidbody.MovePosition(dashPosition);
            isDashButtonDown = false;

            CreateDashEffect(dashPosition, Vector2.Distance(rigidbody.position, dashPosition));
        }
    }

    void CreateDashEffect(Vector3 dashPosition, float dashSize)
    {
        /*
        Transform dashTransform = Instantiate(GameAssets.i.pfDashEffect, dashPosition, Quaternion.identity);
        dashTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.getAngleFromVector(direction));
        dashTransform.localScale = new Vector3(dashSize / 35f, 1, 1);
        */
    }

}