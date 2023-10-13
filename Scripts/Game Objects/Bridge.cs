using Assets.Scripts.Game_Objects.Inheritable;
using System.Linq;
using UnityEngine;

public class Bridge : Interacting
{
    public Sprite spriteActionButton;

    GameObject ActionButtonRepaire;

    readonly string EVENT_NAME = "bridge_repaired";
    readonly (int amount, string nameCode)[] BRIDGE_INGREDIENTS = { (1, "rope"), (2, "dark_wood_plank") };

    private void Start()
    {
        if (MainGameManager._storyEventManager._scenario.Exists(x => x == EVENT_NAME))
        {
            Destroy(gameObject);
            return;
        }
    }

    bool CanBeRepaired()
    {
        foreach(var ingredient in BRIDGE_INGREDIENTS)
        {
            if (MainGameManager._inventoryManager._items.FindAll(x => x.NameCode == ingredient.nameCode).Count < ingredient.amount)
            {
                return false;
            }
        }

        return true;

        //return BRIDGE_INGREDIENTS.All(element => MainGameManager._inventoryManager._items.Exists(x => x.NameCode == element.nameCode));
    }

    void Repair()
    {
        foreach (var x in BRIDGE_INGREDIENTS)
        {
            MainGameManager._inventoryManager.RemoveItem(x.nameCode);
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

            if (CanBeRepaired())
            {
                ActionButtonRepaire = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Repair);
            }
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            if (ActionButtonRepaire != null) Destroy(ActionButtonRepaire);
            ActionButtonRepaire = null;
        }
    }

    protected override void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

}
