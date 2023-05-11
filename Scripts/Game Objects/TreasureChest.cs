using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Game_Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TreasureChest : NorthApproachingInteractable
{
    [HideInInspector] public Item content;
    public Inventory inventory;
    public bool isOpen;
    public Signal raiseItem;
    public string itemNameCode;

    private GameObject dialogWindow;
    private Text dialogText;
    private GameObject receivedItemContext;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        receivedItemContext = FindGameObjectHelper.FindInactiveObjectByName("received item");

        dialogWindow = FindGameObjectHelper.FindInactiveObjectByName("DialogBox");
        dialogText = dialogWindow.GetComponentInChildren<Text>();

        content = ItemRepository.Current.GetByCode(itemNameCode);
    }

    void Update()
    {
        if (Gamepad.current[GamepadButton.East].wasPressedThisFrame
            && playerInRange)
        {
            if (!isOpen)
            {
                anim.SetBool("open", true);

                exitSignal.Raise();

                dialogWindow.SetActive(true);
                dialogText.text = content.Description;

                inventory.items.Add(content);

                receivedItemContext.SetActive(true);
                receivedItemContext.GetComponent<SpriteRenderer>().sprite = content.Sprite;

                raiseItem.Raise();

                isOpen = true;
            }
            else
            {
                raiseItem.Raise();
                dialogWindow.SetActive(false);
                receivedItemContext.SetActive(false);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isOpen)
        {
            base.OnTriggerEnter2D(collider);
        }
    }

    protected void OnTriggerStay2D(Collider2D collider)
    {
        if (!isOpen)
        {
            base.OnTriggerStay2D(collider);
        }
    }

}
