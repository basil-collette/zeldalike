using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using UnityEngine;

public class LogTreeGrow : Interacting
{
    public bool hasGrown;
    public Sprite spriteActionButton;

    readonly string EVENT_NAME = "tree_grown";
    GameObject ActionButtonGrow;

    void Start()
    {
        if (MainGameManager._storyEventManager._scenario.Exists(x => x == EVENT_NAME))
        {
            hasGrown = true;
            this.enabled = false;
            return;
        }

        //set animator bool de si grow or not
    }

    void Grow()
    {
        MainGameManager._storyEventManager.AddScenarioEvent(EVENT_NAME);

        //trigger animation
        //enable le component LogTreeHit à la fin de l'animation (dans l'animation directement)

        hasGrown = true;
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            ActionButtonGrow = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, Grow);
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(ActionButtonGrow);
            ActionButtonGrow = null;
        }
    }

    protected override void OnInteract()
    {
        //
    }

}
