using Assets.Scripts.Game_Objects.Inheritable;
using UnityEngine;

public class Shop : Interacting
{
    public Sprite spriteActionButton;
    GameObject ActionButtonShop;

    protected override void OnInteract()
    {
        //
    }

    void StartPourchase()
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface(new PauseParameter() { InterfaceName = "ShopScene" });
    }

    protected sealed override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            ActionButtonShop = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, StartPourchase);
        }
    }

    protected sealed override void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(ActionButtonShop);
            ActionButtonShop = null;
        }
    }

}