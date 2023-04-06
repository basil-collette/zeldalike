using UnityEngine;

public abstract class FacingObject : MonoBehaviour
{
    protected bool playerInRange;
    protected Animator animatorComp;
    protected bool isFacing = false;

    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (playerInRange
            && isFacing
        )
        {
            OnFacing();
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

            OnQuitFacing();
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
            }
        }
    }

    protected abstract void OnFacing();

    protected abstract void OnQuitFacing();

}