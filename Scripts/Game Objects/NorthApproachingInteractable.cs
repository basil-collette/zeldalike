using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class NorthApproachingInteractable : MonoBehaviour
    {
        public Signal enterSignal;
        public Signal exitSignal;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                if (DirectionHelper.IsFacingUp(collider.GetComponent<Player>().orientation))
                {
                    enterSignal.Raise();
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                if (DirectionHelper.IsFacingUp(collider.GetComponent<Player>().orientation))
                {
                    enterSignal.Raise();
                }
                else
                {
                    exitSignal.Raise();
                }
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag("Player")
                && !collider.isTrigger)
            {
                exitSignal.Raise();
            }
        }

    }
}
