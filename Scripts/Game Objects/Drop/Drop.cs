using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    public AudioClip takeSound;

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            if (OnTriggerEnter2DIsPlayer(collider)) {
                Destroy(this.gameObject);
            }
        }
    }

    protected abstract bool OnTriggerEnter2DIsPlayer(Collider2D collider);

}
