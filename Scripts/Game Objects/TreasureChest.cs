using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Game_Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TreasureChest : NorthApproachingInteractable
{
    public Item content;
    public Inventory inventory;
    public bool isOpen;
    public Signal raiseItem;

    public string itemNameCode;

    private GameObject dialogWindow;
    private Text dialogText;

    private SpriteRenderer receivedItemContext;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        receivedItemContext = GameObject.Find("received item").GetComponent<SpriteRenderer>();

        dialogWindow = FindGameObjectHelper.FindInactiveObjectByName("DialogBox");
        dialogText = dialogWindow.GetComponentInChildren<Text>();

        content = ItemRepository.Current.GetByCode(itemNameCode);
    }

    void Update()
    {
        if (Gamepad.current[GamepadButton.East].wasPressedThisFrame
            && playerInRange
            && !isOpen)
        {
            dialogWindow.SetActive(true);

            dialogText.text = content.Description;

            inventory.items.Add(content);

            receivedItemContext.sprite = content.Sprite;

            raiseItem.Raise();

            isOpen = true;

            //contextclue.raise ???
        }
    }

}
