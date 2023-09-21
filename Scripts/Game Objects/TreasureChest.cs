using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Game_Objects;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TreasureChest : NorthApproachingInteractable
{
    [HideInInspector] [SerializeReference] public Item content;
    public Inventory inventory;
    public bool isOpen;
    public string itemNameCode;
    public ItemTypeEnum itemTypeEnum = ItemTypeEnum.item;
    public string code;

    Transform receivedItemContext;

    void Start()
    {
        if (SaveManager.GameData.opennedChestGuids.Contains(code))
        {
            GetComponent<Animator>().SetBool("open", true);
            enabled = false;
            return;
        }

        receivedItemContext = FindGameObjectHelper.FindByName("Received Item Shadow").transform;

        content = ItemManager.GetItem(itemNameCode, itemTypeEnum);

        ButtonHelper.exitPause += ExitRaiseItem;
    }

    private void OnDisable()
    {
        ButtonHelper.exitPause -= ExitRaiseItem;
    }

    void Update()
    {
        if (playerInRange
            && Gamepad.current != null
            && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
        {
            if (!isOpen)
            {
                ObtainItem();
            }
        }
    }

    void ObtainItem()
    {
        isOpen = true;

        GetComponent<Animator>().SetTrigger("opentrigger");

        exitSignal.Raise(); //to remove the "?" clue

        inventory.AddItem(content);

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

        FindAnyObjectByType<Player>().RaiseItem();

        SaveManager.GameData.opennedChestGuids.Add(code.ToString());

        FindAnyObjectByType<PauseManager>().ShowPausedInterface("InfoScene", () =>
        {
            FindGameObjectHelper.FindByName("Info Canva").GetComponentInChildren<Text>().text = content.Description;
        }, true);
    }

    public void ExitRaiseItem()
    {
        FindAnyObjectByType<Player>().CloseRaiseItem();

        receivedItemContext.gameObject.SetActive(false);

        this.enabled = false;
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isOpen)
        {
            base.OnTriggerEnter2D(collider);
        }
    }

    protected new void OnTriggerStay2D(Collider2D collider)
    {
        if (!isOpen)
        {
            base.OnTriggerStay2D(collider);
        }
    }

}
