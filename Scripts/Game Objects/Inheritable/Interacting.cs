using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Game_Objects.Inheritable
{
    public abstract class Interacting : Approaching
    {
        new void FixedUpdate()
        {
            if (playerInRange
                && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
            {
                OnInterfact();
            }
        }

        protected abstract void OnInterfact();

    }
}
