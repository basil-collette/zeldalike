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

    public Text debugText;

    protected Animator animatorComp;
    protected bool isFacing = false;

    private void Start()
    {
        
    }

    void Update()
    {
        if (playerInRange
            && isFacing
            && Gamepad.current[GamepadButton.South].wasPressedThisFrame
        ) {
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
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;

            animatorComp = collider.GetComponent<Animator>();

            SetIsFacing();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            animatorComp = null;
            dialogBox.SetActive(false);
            isFacing = false;
        }
    }

    private void OnTriggerStay2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            SetIsFacing();
        }
    }

    void SetIsFacing()
    {
        //debugText.text = (animatorComp != null) ? "True" : "False";
        if (animatorComp != null)
        {
            AnimatorClipInfo[] animStateInfo = animatorComp.GetCurrentAnimatorClipInfo(0);
            
            if (animStateInfo[0].clip.name == "idleUp"
                || animStateInfo[0].clip.name == "walkUp")
            {
                isFacing = true;
                //debugText.text = "True";
                return;
            }
        }

        isFacing = false;
        //debugText.text = "False";
    }

}
