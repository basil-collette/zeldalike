using UnityEngine;

//it is preferred to use FacingObject to trigger events whose
//objects concerned come from a specific scene. Inherit this class to use it
//For global object (as screen ui), use NorthApproachingInteractable
public abstract class FacingObject : MonoBehaviour
{
    protected bool playerInRange;
    protected bool isFacing = false;

    protected void FixedUpdate()
    {
        if (isFacing
            && playerInRange)
        {
            OnFacing();
        }
        else
        {
            OnQuitFacing();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = true;

            SetIsFacing(collider);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            playerInRange = false;
            isFacing = false;

            OnQuitFacing();
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            SetIsFacing(collider);
        }
    }

    protected virtual void SetIsFacing(Collider2D collider)
    {
        AliveEntity entity = collider.GetComponent<AliveEntity>();
        if (entity != null
            && DirectionHelper.IsFacingUp(entity.direction))
        {
            isFacing = true;
            return;
        }

        if (isFacing && entity.direction == Vector3.zero)
        {
            return;
        }

        isFacing = false;
    }

    protected abstract void OnFacing();

    protected abstract void OnQuitFacing();

}