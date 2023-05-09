using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class NorthApproachingInteractable : ApproachingInteractable
    {

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (enterSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                enterSignal.Raise();
                playerInRange = true;
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (enterSignal != null
                && exitSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                if (DirectionHelper.IsFacingUp(collider.GetComponent<Player>().orientation))
                {
                    //enterSignal.Raise();
                    playerInRange = true;
                }
                else
                {
                    base.OnTriggerExit2D(collider);
                }
            }
            
        }

    }
}
