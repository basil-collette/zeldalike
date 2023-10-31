using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using UnityEngine;

public class LogTreeGrow : Interacting
{
    readonly string EVENT_NAME = "tree_grown";
    readonly string REQUIRED_ITEM_CODE = "arrosoir";

    [SerializeField] Sprite spriteActionButton;
    [SerializeField] CircleCollider2D actionArrosoirButtonZone;

    GameObject ActionButtonGrow;
    LogTreeHit logTreeHit;

    void Start()
    {
        logTreeHit = GetComponent<LogTreeHit>();

        if (MainGameManager._storyEventManager._scenario.Exists(x => x == EVENT_NAME))
        {
            GetComponent<Animator>().SetBool("grown", true);
            logTreeHit.enabled = true;
            enabled = false;
            actionArrosoirButtonZone.enabled = false;
            return;
        }

        logTreeHit.enabled = false;
    }

    void Grow()
    {
        MainGameManager._storyEventManager.AddScenarioEvent(EVENT_NAME);

        GetComponent<Animator>().SetTrigger("grow");

        actionArrosoirButtonZone.enabled = false;
        Destroy(ActionButtonGrow);
    }

    public void OnGrowAnimationEnd()
    {
        logTreeHit.enabled = true;
        enabled = false;
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            if (MainGameManager._inventoryManager._items.Exists(x => x.NameCode == REQUIRED_ITEM_CODE))
            {
                ActionButtonGrow = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Grow);
            }
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            if (ActionButtonGrow != null) Destroy(ActionButtonGrow);

            ActionButtonGrow = null;
        }
    }

    protected override void OnInteract()
    {
        //
    }

}
