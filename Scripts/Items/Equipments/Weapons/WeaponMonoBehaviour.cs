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
        [HideInInspector] public bool attacking;

        protected Animator animatorTop;
        protected Animator animatorLegs;
        protected PlayerInput playerInputs;

        protected void Start()
        {
            animatorTop = transform.parent.GetChild(1).GetComponent<Animator>();
            animatorLegs = transform.parent.GetChild(0).GetComponent<Animator>();

            playerInputs = GetComponent<PlayerInput>();
            anim = GetComponentInChildren<Animator>();
            anim.speed = 1 / weapon.speed;

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
                }
            }
        }

        public void OnAnimationEnd()
        {
            attacking = false;
            animatorTop.SetBool("attacking", false);
            animatorLegs.SetBool("attacking", false);
            direction = Vector2.zero;
        }

        protected abstract void Attack(Vector3 direction);

    }
}