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
    public string itemNameCode;

    private GameObject receivedItemContext;

    void Start()
    {
        receivedItemContext = FindAnyObjectByType<Player>().transform.Find("received item").gameObject;

        content = ItemRepository.Current.GetByCode(itemNameCode);

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
                isOpen = true;

                GetComponent<Animator>().SetBool("open", true);

                exitSignal.Raise(); //to remove the "?" clue

                inventory.items.Add(content);

                receivedItemContext.gameObject.SetActive(true);
                receivedItemContext.GetComponent<SpriteRenderer>().sprite = content.Sprite;

                FindAnyObjectByType<Player>().RaiseItem();

                FindAnyObjectByType<PauseManager>().ShowPausedInterface("InfoScene", () =>
                {
                    FindGameObjectHelper.FindByName("Info Canva").GetComponentInChildren<Text>().text = content.Description;
                }, true);
            }
        }
    }

    public void ExitRaiseItem()
    {
        FindAnyObjectByType<Player>().CloseRaiseItem();
        receivedItemContext.SetActive(false);
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
