using Assets.Scripts.Game_Objects.Inheritable;
using System.Linq;
using UnityEngine;

public class Bridge : Interacting
{
    InventoryManager inventory;
    public Sprite spriteActionButton;

    bool canBeRepaired = false;
    bool isRepaired = false;

    GameObject ActionButtonRepaire;

    readonly string EVENT_NAME = "bridge_repaired";
    readonly string[] BRIDGE_INGREDIENTS = { "rope", "woodplank_dark" };

    private void Start()
    {
        inventory = MainGameManager._inventoryManager;

        CheckRepairability();
    }

    void CheckRepairability()
    {
        if (MainGameManager._storyEventManager._scenario.Exists(x => x == EVENT_NAME))
        {
            isRepaired = true;
            this.enabled = false;
            return;
        }

        if (BRIDGE_INGREDIENTS.All(element => inventory._items.Exists(x => x.NameCode == element)))
        {
            canBeRepaired = true;
            return;
        }
    }

    void Repair()
    {
        CheckRepairability();

        if (!canBeRepaired) return;

        foreach (var x in BRIDGE_INGREDIENTS)
        {
            inventory.RemoveItem(x);
        }

        MainGameManager._storyEventManager.AddScenarioEvent(EVENT_NAME);

        Destroy(gameObject);

        exitSignal?.Raise();
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            ActionButtonRepaire = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Repair);
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(ActionButtonRepaire);
            ActionButtonRepaire = null;
        }
    }

    protected override void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

}
