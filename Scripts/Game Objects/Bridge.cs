using Assets.Scripts.Game_Objects.Inheritable;
using System.Linq;

public class Bridge : Interacting
{
    public Inventory inventory;

    bool canBeRepaired = false;
    bool isRepaired = false;

    readonly string EVENT_NAME = "bridge";
    readonly string[] BRIDGE_INGREDIENTS = { "rope", "woodplank_dark" };

    private void Start()
    {
        CheckRepairability();
    }

    void CheckRepairability()
    {
        if (SaveManager.GameData.events.Scenario.Exists(x => x == EVENT_NAME))
        {
            isRepaired = true;
            this.enabled = false;
            return;
        }

        if (BRIDGE_INGREDIENTS.All(element => inventory.Items.Exists(x => x.NameCode == element)))
        {
            canBeRepaired = true;
            return;
        }
    }

    void Repair()
    {
        foreach(var x in BRIDGE_INGREDIENTS)
        {
            inventory.RemoveItem(x);
        }

        SaveManager.GameData.events.Scenario.Add(EVENT_NAME);

        Destroy(gameObject);
    }

    protected override void OnInteract()
    {
        CheckRepairability();

        /*
        if (canBeRepaired)
        {
            Repair();
        }
        else
        {
            //Message ?
        }
        */
        Repair();
    }

}
