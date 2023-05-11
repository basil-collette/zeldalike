using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class NorthApproachingInteractable : ApproachingInteractable
    {

        protected void OnTriggerEnter2D(Collider2D collider)
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

        protected void OnTriggerStay2D(Collider2D collider)
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
