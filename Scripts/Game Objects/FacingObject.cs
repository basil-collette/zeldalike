using UnityEngine;

public abstract class FacingObject : MonoBehaviour
{
    protected bool playerInRange;
    protected bool isFacing = false;
    protected AliveEntity entity;

    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (playerInRange
            && isFacing)
        {
            OnFacing();
        }
        else
        {
            OnQuitFacing();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            entity = collider.GetComponent<AliveEntity>();
            playerInRange = true;

            SetIsFacing();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetIsFacing();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            isFacing = false;

            OnQuitFacing();
        }
    }

    void SetIsFacing()
    {
        if (DirectionHelper.IsFacingUp(entity.direction))
        {
            isFacing = true;
            return;
        }

        isFacing = false;
    }

    protected abstract void OnFacing();

    protected abstract void OnQuitFacing();

}