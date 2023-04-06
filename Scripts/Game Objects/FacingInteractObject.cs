using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public abstract class FacingInteractObject : MonoBehaviour
{
    protected bool playerInRange;
    protected Animator animatorComp;
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

            animatorComp = collider.GetComponent<Animator>();

            SetIsFacing();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            animatorComp = null;
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
        //debugText.text = (animatorComp != null) ? "True" : "False";
        if (animatorComp != null)
        {
            AnimatorClipInfo[] animStateInfo = animatorComp.GetCurrentAnimatorClipInfo(0);

            if (animStateInfo[0].clip.name == "idleUp"
                || animStateInfo[0].clip.name == "walkUp")
            {
                isFacing = true;
            }
        }
    }

    protected abstract void OnFacingInterfact();

    protected abstract void OnQuitFacing();

}