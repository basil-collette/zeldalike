using Assets.Scripts.Game_Objects.Inheritable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MoneyChest : FacingInteracting
{
    public int _amount;
    public string _eventOpenCode;

    [SerializeField] Sprite _spriteActionButton;
    [SerializeField] Sprite _moneySprite;

    Transform _receivedItemContext;
    GameObject _ActionButtonOpen;

    void Start()
    {
        if (MainGameManager._storyEventManager._opennedChests.Contains(_eventOpenCode))
        {
            GetComponent<Animator>().SetBool("open", true);
            GetComponent<PolygonCollider2D>().enabled = false;
            enabled = false;
            return;
        }

        _receivedItemContext = FindGameObjectHelper.FindByName("Received Item Shadow").transform;
    }

    new void Update()
    {
        if (playerInRange
            && Gamepad.current != null
            && Gamepad.current[GamepadButton.East].wasPressedThisFrame)
        {
            ObtainContent();
        }
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerEnter2D(collider);

            if (isFacing)
            {
                _ActionButtonOpen = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(_spriteActionButton, ObtainContent);
            }
        }
    }

    protected new void OnTriggerStay2D(Collider2D collider)
    {
        base.OnTriggerStay2D(collider);

        if (isFacing)
        {
            if (_ActionButtonOpen == null)
            {
                _ActionButtonOpen = FindGameObjectHelper.FindByName("Actions Container").GetComponent<ActionButtonsManager>().AddButton(_spriteActionButton, ObtainContent);
            }
        }
        else
        {
            Destroy(_ActionButtonOpen);
            _ActionButtonOpen = null;
        }
    }

    protected new void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            base.OnTriggerExit2D(collider);

            Destroy(_ActionButtonOpen);
            _ActionButtonOpen = null;
        }
    }

    void ObtainContent()
    {
        Player player = FindAnyObjectByType<Player>();

        MainGameManager._inventoryManager.AddMoney(_amount);

        MainGameManager._soundManager.PlayEffect("door");

        GetComponent<PolygonCollider2D>().enabled = false;

        GetComponent<Animator>().SetTrigger("opentrigger");

        exitSignal.Raise(); //to remove the "?" clue

        _receivedItemContext.gameObject.SetActive(true);

        Transform receivedItemSprite = _receivedItemContext.transform.GetChild(0).transform;
        receivedItemSprite.GetComponent<SpriteRenderer>().sprite = _moneySprite;

        //Show the amount

        player.RaiseItem();

        MainGameManager._storyEventManager.AddOpennedChestsEvent(_eventOpenCode);

        FindAnyObjectByType<PauseManager>().ShowPausedInterface(new PauseParameter()
        {
            InterfaceName = "InfoScene",
            TransparentOverlay = true,
            OnPauseProcessed = () => { FindGameObjectHelper.FindByName("Info Canva").GetComponentInChildren<Text>().text = $"{_amount} pièces"; EventHelper.exitPause += ExitRaiseItem; },
            PlaySound = false
        });

        Destroy(_ActionButtonOpen);
        _ActionButtonOpen = null;

        enabled = false;
    }

    public void ExitRaiseItem()
    {
        FindAnyObjectByType<Player>().CloseRaiseItem();

        _receivedItemContext.gameObject.SetActive(false);

        EventHelper.exitPause -= ExitRaiseItem;
    }

    protected override void OnInteract()
    {
        //throw new System.NotImplementedException();
    }

}
