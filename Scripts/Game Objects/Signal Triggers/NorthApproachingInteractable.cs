using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    //it is preferred to use NorthApproachingInteractable to trigger
    //events whose objects concerned are global (as screen ui).
    //For specific scene objects, use FacingObject
    public class NorthApproachingInteractable : ApproachingInteractable
    {

        protected new void OnTriggerEnter2D(Collider2D collider)
        {
            if (enterSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                if (DirectionHelper.IsFacingUp(collider.GetComponent<Player>().orientation))
                {
                    enterSignal.Raise();
                    playerInRange = true;
                }
            }
        }

        protected new void OnTriggerStay2D(Collider2D collider)
        {
            if (enterSignal != null
                && exitSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                if (DirectionHelper.IsFacingUp(collider.GetComponent<Player>().orientation))
                {
                    base.OnTriggerEnter2D(collider);
                }
                else
                {
                    base.OnTriggerExit2D(collider);
                }
            }
            
        }

    }
}
