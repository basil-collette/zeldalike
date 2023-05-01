using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class ApproachingInteractable : MonoBehaviour
    {
        public Signal enterSignal;
        public Signal staySignal;
        public Signal exitSignal;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (enterSignal != null)
            {
                if (collider.CompareTag("Player")
                    && !collider.isTrigger)
                {
                    enterSignal.Raise();
                }
            }
            Debug.Log("enter");
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (staySignal != null)
            {
                if (collider.CompareTag("Player")
                    && !collider.isTrigger)
                {
                    staySignal.Raise();
                }
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (exitSignal != null)
            {
                if (collider.CompareTag("Player")
                    && !collider.isTrigger)
                {
                    exitSignal.Raise();
                }
            }
            Debug.Log("exit");
        }

    }
}
