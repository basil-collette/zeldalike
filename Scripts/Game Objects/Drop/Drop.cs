using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    public AudioClip takeSound;
    public string _dropEventName;

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            if (OnTriggerEnter2DIsPlayer(collider)) {

                Destroy(this.gameObject);

                if (takeSound != null)
                    MainGameManager._soundManager.PlayEffect(takeSound);

                if (_dropEventName != string.Empty)
                    MainGameManager._storyEventManager.AddScenarioEvent(_dropEventName);
            }
        }
    }

    protected abstract bool OnTriggerEnter2DIsPlayer(Collider2D collider);

}
