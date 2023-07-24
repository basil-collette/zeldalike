using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Game_Objects;
using System;
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
    public string Uid;

    GameObject receivedItemContext;

    void Start()
    {
        if (FindAnyObjectByType<SaveManager>().GameData.opennedChestGuids.Contains(Uid.ToString()))
        {
            GetComponent<Animator>().SetBool("open", true);
            enabled = false;
            return;
        }

        receivedItemContext = FindAnyObjectByType<Player>().transform.Find("received item").gameObject;

        content = Singleton<ItemRepository<Item>>.Instance.GetByCode(itemNameCode);

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

                GetComponent<Animator>().SetTrigger("opentrigger");

                exitSignal.Raise(); //to remove the "?" clue

                inventory.AddItem(content);

                receivedItemContext.gameObject.SetActive(true);
                receivedItemContext.GetComponent<SpriteRenderer>().sprite = content.Sprite;

                FindAnyObjectByType<Player>().RaiseItem();

                FindAnyObjectByType<SaveManager>().GameData.opennedChestGuids.Add(Uid.ToString());

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
