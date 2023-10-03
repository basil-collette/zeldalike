using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Game_Objects.Inheritable
{
    public abstract class FacingInteracting : Interacting
    {
        protected bool isFacing = false;

        new protected void Update()
        {
            if (playerInRange
                && isFacing
                && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
            {
                OnInteract();
            }
        }

        protected new void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player") && !collider.isTrigger)
            {
                playerInRange = true;
                SetIsFacing(collider);

                if (isFacing)
                    enterSignal?.Raise();
            }
        }

        protected new void OnTriggerExit2D(Collider2D collision)
        {
            exitSignal?.Raise();

            playerInRange = false;
            isFacing = false;
        }

        protected virtual void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player") && !collider.isTrigger)
            {
                SetIsFacing(collider);

                if (isFacing)
                {
                    enterSignal?.Raise();
                }
                else
                {
                    exitSignal?.Raise();
                }
            }
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

    }
}
