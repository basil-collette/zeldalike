using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public abstract class FacingInteractObject : MonoBehaviour
{
    protected bool playerInRange;
    protected AliveEntity entity;
    protected bool isFacing = false;

    void FixedUpdate()
    {
        if (playerInRange
            && isFacing
            && Gamepad.current[GamepadButton.South].wasPressedThisFrame
        )
        {
            OnFacingInterfact();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;

            entity = collider.GetComponent<AliveEntity>();

            SetIsFacing();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            isFacing = false;

            OnQuitFacing();
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            SetIsFacing();
        }
    }

    protected virtual void SetIsFacing()
    {
        if (DirectionHelper.IsFacingUp(entity.direction))
        {
            isFacing = true;
        }
    }

    protected abstract void OnFacingInterfact();

    protected abstract void OnQuitFacing();

}