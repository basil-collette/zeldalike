using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Game_Objects.Inheritable
{
    public abstract class Facing : Approaching
    {
        protected bool isFacing = false;

        new protected void Update()
        {
            if (playerInRange
                && isFacing)
            {
                OnFacing();
            }
        }

        protected override sealed void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player") && !collider.isTrigger)
            {
                playerInRange = true;
                SetIsFacing(collider);

                if (isFacing)
                    enterSignal?.Raise();
            }
        }

        protected override sealed void OnTriggerExit2D(Collider2D collision)
        {
            exitSignal?.Raise();

            playerInRange = false;
            isFacing = false;
        }

        protected virtual void SetIsFacing(Collider2D collider)
        {
            AliveEntity entity = collider.GetComponent<AliveEntity>();
            if (entity != null
                && DirectionHelper.IsFacingUp(entity.direction))
            {
                isFacing = true;
                return;
            }

            if (isFacing && entity.direction == Vector3.zero)
            {
                return;
            }

            isFacing = false;
        }

        protected abstract void OnFacing();

    }
}
