using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public bool playerInRange;

    //InputAction clickB;
    //bool isButtonDown = false;

    protected bool isFacing = false;

    private void Start()
    {
        /*
        clickB = GetComponent<PlayerInput>().actions["B"];
        clickB.canceled += OnUnpress;
        */
    }

    void Update()
    {
        if (playerInRange
            && isFacing
            && Gamepad.current[GamepadButton.South].wasPressedThisFrame
            //&& clickB.ReadValue<float>() > 0//Input.GetMouseButtonDown(1)
        ) {
            //isButtonDown = true;

            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }

/*
    void OnUnpress(InputAction.CallbackContext ctx) {
        isButtonDown = false;
    }
*/
    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            playerInRange = true;

            SetIsFacing(playerCollider);
        }
    }

    void OnTriggerExit2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);

            SetIsFacing(playerCollider);
        }
    }

    private void OnTriggerStay2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            SetIsFacing(playerCollider);
        }
    }

    void SetIsFacing(Collider2D playerCollider)
    {
        Animator animatorComp = playerCollider.GetComponent<Animator>();
        if (animatorComp != null)
        {
            AnimatorClipInfo[] animStateInfo = animatorComp.GetCurrentAnimatorClipInfo(0);
            
            if (animStateInfo[0].clip.name == "idleUp"
                || animStateInfo[0].clip.name == "walkUp")
            {
                isFacing = true;
                return;
            }
        }

        isFacing = false;
    }

    /*
        void OnDestroy()
        {
            clickB.canceled -= OnUnpress;
        }

        void OnDisable()
        {
            clickB.canceled -= OnUnpress;
        }
    */
}
