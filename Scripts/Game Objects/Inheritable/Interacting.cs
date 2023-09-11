using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Game_Objects.Inheritable
{
    public abstract class Interacting : Approaching
    {
        protected void Update()
        {
            if (playerInRange
                && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
            {
                OnInteract();
            }
        }

        protected abstract void OnInteract();

    }
}
