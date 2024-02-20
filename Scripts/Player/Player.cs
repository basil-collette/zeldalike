using Assets.Database.Model.Design;
using Assets.Scripts.Items.Equipments.Weapons;
using Assets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Player : AliveEntity
{
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
    Animator animatorLegs;
    Animator animatorTop;

    new void Start()
    {
        base.Start();

        animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        animatorTop = transform.GetChild(1).GetComponent<Animator>();

        playerInputs = FindAnyObjectByType<PlayerInput>();
        cooldownManager = GetComponent<CooldownManager>();

        transform.position = startingPosition.initalValue;
        direction = startingDirection.initalValue;
        animatorLegs.SetFloat("moveX", direction.x);
        animatorLegs.SetFloat("moveY", direction.y);
        animatorTop.SetFloat("lookX", direction.x);
        animatorTop.SetFloat("lookY", direction.y);

        animatorLegs.SetFloat("moveX", startingDirection.initalValue.x);
        animatorLegs.SetFloat("moveY", startingDirection.initalValue.y);
        animatorTop.SetFloat("lookX", startingDirection.initalValue.x);
        animatorTop.SetFloat("lookY", startingDirection.initalValue.y);

        SetState(EntityState.walk);

        EquipWeapon(MainGameManager._inventoryManager._weapon);

        //Override la fonction OnDeath qui détruit normalement l'entité, dans cette Démo on souhaite que le joueur revive à l'infini
        GetComponent<Health>()._dieOverride = OnDeathHealthOverride;
    }

    new void Update()
    {
        if (currentEntityState == EntityState.unavailable)
            return;

        SetState(EntityState.idle);

        if (Gamepad.current != null && Gamepad.current[GamepadButton.South].wasPressedThisFrame)
        {
            Dash();
            return;
        }

        if (cooldownManager.IsAvailable("dashDuration"))
        {
            direction = Vector3.zero;

            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");

            Vector2 joystickInput = playerInputs.actions["Move"].ReadValue<Vector2>();
            if (joystickInput != Vector2.zero)
            {
                direction.x = joystickInput.x;
                direction.y = joystickInput.y;
            }
        }

        if (direction != Vector3.zero)
        {
            SetState(EntityState.walk);
            SetOrientation();
        }
        else
        {
            Imobilize();
        }
    }

    void OnDeathHealthOverride()
    {
        //Animation death
        //coroutine wait seconds

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Vector3 pos = Resources.Load<VectorValue>("ScriptableObjects/Player/position/PlayerPosition").initalValue;
        transform.position = pos;

        FloatValue playerHealth = Resources.Load<FloatValue>("ScriptableObjects/Player/Health/PlayerHealth");
        playerHealth.RuntimeValue = playerHealth.initialValue;

        GetComponent<Health>()._healthSignal.Raise();
    }

    public void EquipWeapon(Weapon weapon)
    {
        bool hasWeapon = weapon != null && weapon.Id != 0;

        var attackJoystickImage = FindGameObjectHelper.FindByName("WeaponSprite")?.GetComponent<Image>();

        FindGameObjectHelper.FindByName("Attack joystick container")?.SetActive(hasWeapon);

        if (!hasWeapon)
        {
            attackJoystickImage.color = new Color(1, 1, 1, 0);
            return;
        }

        WeaponMonoBehaviour weaponMonobehaviour = Resources.Load<WeaponMonoBehaviour>($"Prefabs/Weapons/{weapon.weaponType}");
        weaponMonobehaviour._weapon = weapon;
        WeaponMonoBehaviour weaponMonoBehaviour = Instantiate(weaponMonobehaviour, transform);
        weaponMonoBehaviour.GetComponentInChildren<TriggerHit>(true).attackerTag = "Player";

        attackJoystickImage.sprite = weaponMonobehaviour._weapon.Sprite;
        attackJoystickImage.color = new Color(1, 1, 1, 1);
        attackJoystickImage.preserveAspect = true;

        _weapon = weaponMonoBehaviour;

        FindGameObjectHelper.FindByName("Attack joystick container").SetActive(true);
    }

    public void UnequipWeapon()
    {
        Destroy(_weapon);

        var attackJoystickImage = FindGameObjectHelper.FindByName("WeaponSprite")?.GetComponent<Image>();
        attackJoystickImage.color = new Color(1, 1, 1, 0);

        FindGameObjectHelper.FindByName("Attack joystick container").SetActive(false);
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

        #region WALK & DASH

        if (currentEntityState == EntityState.walk)
        {
            if (direction != Vector3.zero)
            {
                direction.Normalize();

                float finalSpeed = dashSpeed;

                if (cooldownManager.IsAvailable("dashDuration"))
                {
                    finalSpeed = moveSpeed;

                    if (cooldownManager.IsAvailable("walkSoundCooldown"))
                    {
                        MainGameManager._soundManager.PlaySoundIndependently(walkSound,
                            UnityEngine.Random.Range(0.8f, 1.2f),
                            UnityEngine.Random.Range(0.7f, 1.3f));

                        cooldownManager.StartCooldown("walkSoundCooldown", 0.4f);
                    }
                }

                //rigidbody.MovePosition(transform.position + (direction.normalized * finalSpeed * Time.fixedDeltaTime));
                GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * finalSpeed, direction.y * finalSpeed);

                animatorTop.SetBool("moving", true);
                animatorLegs.SetBool("moving", true);
            }
            else
            {
                Imobilize();
            }
        }

        #endregion

        #region ANIMATION

        if (direction != Vector3.zero)
        {
            animatorLegs.SetFloat("moveX", direction.x);
            animatorLegs.SetFloat("moveY", direction.y);

            if (MainGameManager._inventoryManager._weapon != null && MainGameManager._inventoryManager._weapon.Id != 0)
            {
                var weapon = GetComponentInChildren<WeaponMonoBehaviour>();
                if (weapon.attacking) return;
            }

            animatorTop.SetFloat("lookX", direction.x);
            animatorTop.SetFloat("lookY", direction.y);
        }

        #endregion
    }

    public void AttackAnimation(Vector3 weaponDirection)
    {
        animatorTop.SetBool("attacking", true);

        animatorTop.SetFloat("lookX", weaponDirection.x);
        animatorTop.SetFloat("lookY", weaponDirection.y);

        animatorLegs.SetBool("attacking", true);

        if (direction == Vector3.zero)
        {
            animatorLegs.SetFloat("moveX", weaponDirection.x);
            animatorLegs.SetFloat("moveY", weaponDirection.y);
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
            _weapon.EndAttack();

            if (direction == Vector3.zero)
                direction = moveOrientation;

            GetComponent<Health>().enabled = false;
            animatorTop.SetTrigger("roll");
            animatorLegs.SetBool("rolling", true);

            MainGameManager._soundManager.PlayEffect(rollSound);

            CooldownButton.buttons.Find(x => x._name == "dash")?.Cooldwon(dashCooldown);

            Action OnEndDashAnimation = () => {
                GetComponent<Health>().enabled = true;
                animatorLegs.SetBool("rolling", false);
            };

            cooldownManager.StartCooldown("dashDuration", dashDuration, OnEndDashAnimation);
            cooldownManager.StartCooldown("dashCooldown", dashCooldown);
        }
    }

    public void RaiseItem()
    {
        _weapon.EndAttack();

        animatorTop.SetBool("receivingItem", true);
        SetState(EntityState.unavailable);
    }

    public void CloseRaiseItem()
    {
        animatorTop.SetBool("receivingItem", false);
        SetState(EntityState.walk);
    }

    public override sealed void Imobilize()
    {
        direction = Vector3.zero;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        animatorTop.SetBool("moving", false);
        animatorLegs.SetBool("moving", false);
    }

    protected override sealed IEnumerator FallCo(Vector3 fallPos, Vector3 respawnPos)
    {
        SetState(EntityState.unavailable);
        Imobilize();

        _weapon.EndAttack();

        MainGameManager._soundManager.PlayEffect("falling");

        animatorTop.SetTrigger("fall");

        animatorLegs.SetBool("hidden", true);

        while (!animatorTop.GetCurrentAnimatorStateInfo(0).IsName("Fall")) yield return null;

        transform.position = fallPos;

        while (animatorTop.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        animatorLegs.SetBool("hidden", false);

        transform.position = respawnPos;
        SetState(EntityState.idle);

        GetComponent<Health>().Hit(gameObject, new List<Effect> { new Effect(EffectTypeEnum.neutral, 0.25f) }, "Player");
    }

}