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
        public Weapon weapon;

        [HideInInspector] public Vector2 direction;
        [HideInInspector] public Animator anim;
        public bool attacking;
        protected Animator animatorTop;
        protected Animator animatorLegs;
        protected PlayerInput playerInputs;
        protected CooldownManager cooldownManager;

        protected void Start()
        {
            animatorTop = transform.parent.GetChild(1).GetComponent<Animator>();
            animatorLegs = transform.parent.GetChild(0).GetComponent<Animator>();

            playerInputs = GetComponent<PlayerInput>();
            cooldownManager = GetComponent<CooldownManager>();
            anim = GetComponentInChildren<Animator>();
            anim.speed = weapon.speed;
            //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(weapon.animatorName);

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
                    animatorTop.SetBool("attacking", true);
                    animatorLegs.SetBool("attacking", true);

                    Attack(direction);

                    Action OnEnd = () => {
                        attacking = false;
                        animatorTop.SetBool("attacking", false);
                        animatorLegs.SetBool("attacking", false);
                        direction = Vector2.zero;
                    };

                    cooldownManager.StartCooldown("attackCooldown", weapon.attackDelay, null, OnEnd);
                }
            }
        }

        protected abstract void Attack(Vector3 direction);

    }
}