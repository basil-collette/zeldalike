using UnityEngine;

//it is preferred to use FacingObject to trigger events whose
//objects concerned come from a specific scene. Inherit this class to use it
//For global object (as screen ui), use NorthApproachingInteractable
public abstract class Approaching : MonoBehaviour
{
    protected bool playerInRange;

    protected void FixedUpdate()
    {
        if (playerInRange)
        {
            OnApproaching();
        }
        else
        {
            OnQuit();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = true;

            OnApproaching();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = false;

            OnQuit();
        }
    }

    protected abstract void OnApproaching();

    protected abstract void OnQuit();

}