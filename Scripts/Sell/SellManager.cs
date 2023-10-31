using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    public SellItem rope;
    public SellItem heart;
    public SellItem axe;
    public Text money;

    void Start()
    {
        List<string> shop = MainGameManager._storyEventManager._shop;

        bool showRope = !(MainGameManager._inventoryManager.HasByCode("rope")
            || shop.Exists(x => x == rope.item.NameCode)
            || MainGameManager._storyEventManager._scenario.Contains("bridge_repaired"));

        rope.gameObject.SetActive(showRope);
        heart.gameObject.SetActive(!shop.Exists(x => x == heart.item.NameCode));
        axe.gameObject.SetActive(!shop.Exists(x => x == axe.item.NameCode));

        money.text = MainGameManager._inventoryManager._money.ToString();
    }

}
