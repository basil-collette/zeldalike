using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EventWhenCross : MonoBehaviour
{
    [SerializeField] string eventNameCode;
    [SerializeField] bool OnlyOnce = true;

    protected void Start()
    {
        if (OnlyOnce && MainGameManager._storyEventManager._scenario.Exists(x => x == eventNameCode))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            collider.GetComponent<Player>().Imobilize();

            TriggerEvent();

            if (OnlyOnce) MainGameManager._storyEventManager.AddScenarioEvent(eventNameCode);
            enabled = false;

            Destroy(gameObject);
        }
    }

    protected abstract void TriggerEvent();

}