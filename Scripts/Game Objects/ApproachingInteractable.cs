using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class ApproachingInteractable : MonoBehaviour
    {
        public Signal enterSignal;
        public Signal staySignal;
        public Signal exitSignal;

        protected bool playerInRange = false;

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            if (enterSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                enterSignal.Raise();
                playerInRange = true;
            }
        }

        protected void OnTriggerStay2D(Collider2D collider)
        {
            if (staySignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                staySignal.Raise();
            }
        }

        protected void OnTriggerExit2D(Collider2D collider)
        {
            if (exitSignal != null
                && collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                exitSignal.Raise();
                playerInRange = false;
            }
        }

    }
}
