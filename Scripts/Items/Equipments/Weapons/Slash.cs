using System;
using UnityEngine;

namespace Assets.Scripts.Items.Equipments.Weapons
{
    [Serializable]
    public class Slash : Weapon
    {
        protected new void Start()
        {
            base.Start();
        }

        protected new void Update()
        {
            base.Update();
        }

        protected sealed override void Attack(Vector3 direction)
        {
            float angle = Vector3.SignedAngle(Vector3.down, direction, Vector3.forward);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            anim.SetTrigger("Attack");
        }
    }
}