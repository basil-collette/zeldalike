using Assets.Scripts.Game_Objects.Inheritable;
using Assets.Scripts.Manager;
using UnityEngine;

public class Door : Interacting
{
    [SerializeField] Sprite spriteActionButton;

    readonly string EVENT_NAME = "door_collectioner_open";

    GameObject ActionButtonOpen;

    void Start()
    {
        if (MainGameManager._storyEventManager._scenario.Exists(x => x == EVENT_NAME))
        {
            Destroy(gameObject);
            return;
        }
    }

    void Open()
    {
        MainGameManager._soundManager.PlayEffect("door");

        MainGameManager._storyEventManager.AddScenarioEvent(EVENT_NAME);

        Destroy(gameObject);

        exitSignal?.Raise();
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            if (MainGameManager._inventoryManager._items.Exists(x => x.NameCode == "key"))
            {
                ActionButtonOpen = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Open);
            }
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            if (ActionButtonOpen != null) Destroy(ActionButtonOpen);
            ActionButtonOpen = null;
        }
    }

    protected override void OnInteract()
    {
        //
    }

}
