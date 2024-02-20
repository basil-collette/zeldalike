using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Items.Equipments.Weapons
{
    [Serializable]
    public abstract class WeaponMonoBehaviour : MonoBehaviour
    {
        public Weapon _weapon;
        [HideInInspector] public Vector2 direction;
        [HideInInspector] public Animator anim;
        [HideInInspector] public bool attacking;

        protected Animator animPlayerTop;
        protected Animator animPlayerLegs;
        protected PlayerInput playerInputs;
        protected CooldownManager cooldownManager;

        Vector3 tempDirection;

        protected void Start()
        {
            anim = GetComponentInChildren<Animator>();
            animPlayerTop = transform.parent.GetChild(1).GetComponent<Animator>();
            animPlayerLegs = transform.parent.GetChild(0).GetComponent<Animator>();

            anim.speed = AttackSpeedModifier;
            animPlayerTop.SetFloat("attackSpeed", AttackSpeedModifier);

            playerInputs = FindAnyObjectByType<PlayerInput>();

            cooldownManager = GetComponent<CooldownManager>();

            direction = Vector2.zero;
        }

        protected void Update()
        {
            direction = playerInputs.actions["Aim"].ReadValue<Vector2>();
            if (direction != Vector2.zero)
            {
                if (!attacking)
                {
                    attacking = true;

                    Attack(direction);

                    anim.SetBool("attacking", true);

                    tempDirection = direction;

                    cooldownManager.StartCooldown("attackCooldown", _weapon.attackDelay, EndAttack);

                    GetComponentInParent<Player>().AttackAnimation(direction);
                }
            }
        }

        float AttackSpeedModifier => 1 / _weapon.speed;

        protected abstract void Attack(Vector3 direction);

        public void EndAttack()
        {
            if (!attacking) return;

            attacking = false;

            anim.SetTrigger("endCooldown");
            anim.SetBool("attacking", false);
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

            animPlayerTop.SetBool("attacking", false);
            animPlayerLegs.SetBool("attacking", false);
            animPlayerLegs.SetFloat("moveX", tempDirection.x);
            animPlayerLegs.SetFloat("moveY", tempDirection.y);

            direction = Vector2.zero;
        }

    }
}