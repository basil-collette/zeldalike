using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Game_Objects.Inheritable;
using Assets.Scripts.Manager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TreasureChest : FacingInteracting
{
    [HideInInspector] [SerializeReference] public Item content;
    public string itemNameCode;
    public ItemTypeEnum itemTypeEnum = ItemTypeEnum.item;
    public string code;

    [SerializeField] Sprite spriteActionButton;

    Transform receivedItemContext;
    GameObject ActionButtonOpen;

    void Start()
    {
        if (MainGameManager._storyEventManager._opennedChests.Contains(code))
        {
            GetComponent<Animator>().SetBool("open", true);
            GetComponent<PolygonCollider2D>().enabled = false;
            enabled = false;
            return;
        }

        receivedItemContext = FindGameObjectHelper.FindByName("Received Item Shadow").transform;

        content = ItemManager.GetItem(itemNameCode, itemTypeEnum);
    }

    new void Update()
    {
        if (playerInRange
            && Gamepad.current != null
            && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
        {
            ObtainItem();
        }
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            if (isFacing)
            {
                ActionButtonOpen = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, ObtainItem);
            }
        }
    }

    protected new void OnTriggerStay2D(Collider2D collider)
    {
        base.OnTriggerStay2D(collider);

        if (isFacing)
        {
            if (ActionButtonOpen == null)
            {
                ActionButtonOpen = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(spriteActionButton, ObtainItem);
            }
        }
        else
        {
            Destroy(ActionButtonOpen);
            ActionButtonOpen = null;
        }
    }

    protected new void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(ActionButtonOpen);
            ActionButtonOpen = null;
        }
    }

    void ObtainItem()
    {
        Player player = FindAnyObjectByType<Player>();
        if (content.NameCode == "heart")
        {
            player.GetComponent<Health>().AddHeartMax();
        }
        else
        {
            if (!MainGameManager._inventoryManager.AddItem(content))
            {
                return;
            }
        }

        GetComponent<PolygonCollider2D>().enabled = false;

        GetComponent<Animator>().SetTrigger("opentrigger");

        exitSignal.Raise(); //to remove the "?" clue

        receivedItemContext.gameObject.SetActive(true);

        Transform receivedItemSprite = receivedItemContext.transform.GetChild(0).transform;
        receivedItemSprite.GetComponent<SpriteRenderer>().sprite = content.Sprite;
        receivedItemSprite.localScale = new Vector3(content.SpriteScale, content.SpriteScale, 1);
        if (itemTypeEnum == ItemTypeEnum.weapon)
        {
            receivedItemSprite.rotation = Quaternion.Euler(0, 0, 90);
            Sprite sprite = receivedItemSprite.GetComponent<SpriteRenderer>().sprite;
            float height = sprite.rect.height / sprite.pixelsPerUnit;
            receivedItemSprite.localPosition = new Vector3(-(height * content.SpriteScale) / 2, 0, 0);
        }

        player.RaiseItem();

        MainGameManager._storyEventManager.AddOpennedChestsEvent(code.ToString());

        FindAnyObjectByType<PauseManager>().ShowPausedInterface(new PauseParameter()
        {
            InterfaceName = "InfoScene",
            TransparentOverlay = true,
            OnPauseProcessed = () => { FindGameObjectHelper.FindByName("Info Canva").GetComponentInChildren<Text>().text = content.Description; ButtonHelper.exitPause += ExitRaiseItem; }
        });

        Destroy(ActionButtonOpen);
        ActionButtonOpen = null;

        enabled = false;
    }

    public void ExitRaiseItem()
    {
        FindAnyObjectByType<Player>().CloseRaiseItem();

        receivedItemContext.gameObject.SetActive(false);

        ButtonHelper.exitPause -= ExitRaiseItem;
    }

    protected override void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

}
