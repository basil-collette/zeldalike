using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

//it is preferred to use FacingInteractObject to trigger events whose
//objects concerned come from a specific scene. Inherit this class to use it
public abstract class FacingInteractObject : FacingObject
{

    new void FixedUpdate()
    {
        if (playerInRange
            && isFacing
            && Gamepad.current[GamepadButton.East].wasPressedThisFrame
        )
        {
            OnFacingInterfact();
        }
    }

    protected abstract void OnFacingInterfact();

}