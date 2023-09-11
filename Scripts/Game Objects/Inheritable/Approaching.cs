using UnityEngine;

public abstract class Approaching : MonoBehaviour
{
    protected bool playerInRange;

    public Signal enterSignal;
    public Signal exitSignal;

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = true;

            enterSignal?.Raise();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = false;

            exitSignal?.Raise();
        }
    }

}