using Assets.Database.Model.Design;
using Assets.Scripts.Items.Equipments.Weapons;
using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Player : AliveEntity
{
    public Inventory inventory;
    public PlayerQuest playerQuest;

    [Header("Position Settings")]
    public VectorValue startingPosition;
    public VectorValue startingDirection;

    [Header("Dash Settings")]
    public float dashSpeed = 20;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public GameObject rollSmokeEffect;
    public AudioClip rollSound;

    public AudioClip walkSound;

    PlayerInput playerInputs;
    CooldownManager cooldownManager;
    WeaponMonoBehaviour _weapon;
    AudioSource audioSource;
    Animator animatorLegs;
    Animator animatorTop;

    new void Start()
    {
        base.Start();

        animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        animatorTop = transform.GetChild(1).GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>(); audioSource.volume = 0.5f;
        playerInputs = GetComponent<PlayerInput>();
        cooldownManager = GetComponent<CooldownManager>();

        transform.position = startingPosition.initalValue;
        //animator.SetFloat("moveX", startingDirection.initalValue.x);
        //animator.SetFloat("moveY", startingDirection.initalValue.y);

        SetState(EntityState.walk);

        if (inventory.Weapon != null && inventory.Weapon.Id != 0)
        {
            EquipWeapon(inventory.Weapon);
        }

        Health health = GetComponent<Health>();
        health._dieOverride = () =>
        {
            //Animation death
            //coroutine wait seconds

            Vector3 pos = Resources.Load<VectorValue>("ScriptableObjects/Player/position/PlayerPosition").initalValue;
            transform.position = pos;

            FloatValue playerHealth = Resources.Load<FloatValue>("ScriptableObjects/Player/Health/PlayerHealth");
            playerHealth.RuntimeValue = playerHealth.initialValue;

            health._healthSignal.Raise();
        };
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        SetState(EntityState.idle);

        /*
        if (currentEntityState == EntityState.attack)
        {
            Imobilize();
            return;
        }
        */

        direction = Vector3.zero;

        if (Gamepad.current != null && Gamepad.current[GamepadButton.South].wasPressedThisFrame)
        {
            Dash();
            return;
        }

        //Grab
        /*
        if (Gamepad.current != null && Gamepad.current[GamepadButton.East].isPressed)
        {
            /*
            if (currentEntityState != EntityState.unavailable)
            {
                SetState(EntityState.attack);
            }
            return;
            *//*
        }
        */

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
        } else
        {
            Imobilize();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        WeaponMonoBehaviour weaponMonobehaviour = Resources.Load<WeaponMonoBehaviour>($"Prefabs/Weapons/{weapon.weaponType}");
        weaponMonobehaviour.weapon = weapon;
        WeaponMonoBehaviour weaponMonoBehaviour = Instantiate(weaponMonobehaviour, transform.position, Quaternion.identity);
        weaponMonoBehaviour.transform.parent = gameObject.transform;
        weaponMonoBehaviour.GetComponentInChildren<TriggerHit>(true).attackerTag = "Player";

        var attackJoystickImage = FindGameObjectHelper.FindByName("WeaponSprite")?.GetComponent<Image>();
        if (attackJoystickImage != null)
        {
            attackJoystickImage.sprite = weaponMonobehaviour.weapon.Sprite;
            attackJoystickImage.preserveAspect = true;
        }

        _weapon = weaponMonoBehaviour;
    }

    public void UnequipWeapon()
    {
        Destroy(_weapon);
    }

    new void FixedUpdate()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        /*
        if (currentEntityState == EntityState.attack)
        {
            if (!animator.GetBool("attacking"))
            {
                animator.SetBool("attacking", true);
                StartCoroutine(AttackCo());
            }
            return;
        }
        */

        if (currentEntityState == EntityState.walk)
        {
            if (direction != Vector3.zero)
            {
                direction.Normalize();

                //rigidbody.MovePosition(transform.position + (direction.normalized * moveSpeed * Time.fixedDeltaTime));

                float finalSpeed = (cooldownManager.IsAvailable("dashDuration"))
                    ? moveSpeed
                    : dashSpeed;

                if (cooldownManager.IsAvailable("dashDuration") && cooldownManager.IsAvailable("walkSoundCooldown"))
                {
                    audioSource.PlayOneShot(walkSound);
                    cooldownManager.StartCooldown("walkSoundCooldown", 0.4f);
                }

                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * finalSpeed, direction.y * finalSpeed);

                animatorTop.SetBool("moving", true);
                animatorLegs.SetBool("moving", true);
            }
            else
            {
                Imobilize();
            }
        }

        if (direction != Vector3.zero)
        {
            animatorLegs.SetFloat("moveX", direction.x);
            animatorLegs.SetFloat("moveY", direction.y);
        }

        SetAnimatorTopDirection();
    }

    void SetAnimatorTopDirection()
    {
        var weapon = GetComponentInChildren<WeaponMonoBehaviour>();

        if (weapon != null && weapon.attacking && weapon.direction != Vector2.zero)
        {
            animatorTop.SetFloat("lookX", weapon.direction.x);
            animatorTop.SetFloat("lookY", weapon.direction.y);

            if (direction == Vector3.zero)
            {
                animatorLegs.SetFloat("moveX", weapon.direction.x);
                animatorLegs.SetFloat("moveY", weapon.direction.y);
            }
        }
        else
        {
            if (direction != Vector3.zero)
            {
                animatorTop.SetFloat("lookX", direction.x);
                animatorTop.SetFloat("lookY", direction.y);
            }
        }
    }

    public void SetOrientation()
    {
        this.moveOrientation = this.direction.normalized;
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
            GetComponent<Health>().enabled = false;
            animatorTop.SetTrigger("roll");
            animatorLegs.SetBool("rolling", true);
            audioSource.clip = rollSound;
            audioSource.Play();

            Coroutine rollEffectCoroutine = StartCoroutine(DashEffectCo());

            Action OnLoop = () => { /* CreateDashEffect() */ };
            Action OnEnd = () => {
                /*SetState(EntityState.walk);*/
                StopCoroutine(rollEffectCoroutine);
                GetComponent<Health>().enabled = true;
                animatorLegs.SetBool("rolling", false);
            };

            cooldownManager.StartCooldown("dashDuration", dashDuration, OnLoop, OnEnd);
            cooldownManager.StartCooldown("dashCooldown", dashCooldown);
        }
    }

    IEnumerator DashEffectCo()
    {
        while(true)
        {
            CreateDashEffect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(.25f);

        //animator.SetBool("attacking", false);
        animatorTop.SetTrigger("attack");
        animatorLegs.SetTrigger("attack");

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
        animatorTop.SetBool("moving", false);
        animatorLegs.SetBool("moving", false);
    }

    void CreateDashEffect()
    {
        Instantiate(rollSmokeEffect, transform.position, Quaternion.identity);
    }

    public void RaiseItem()
    {
        animatorTop.SetBool("receivingItem", true);
        SetState(EntityState.unavailable);
    }

    public void CloseRaiseItem()
    {
        animatorTop.SetBool("receivingItem", false);
        SetState(EntityState.walk);
    }

}