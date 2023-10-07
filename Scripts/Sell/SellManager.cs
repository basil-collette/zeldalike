using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public GameObject rope;
    public GameObject heart;
    public GameObject axe;

    void Start()
    {
        List<string> shop = MainGameManager._storyEventManager._shop;

        if (shop.Exists(x => x ==  "rope")
            && !MainGameManager._inventoryManager._items.Exists(x => x.NameCode == "rope"))
        {
            rope.SetActive(true);
        }

        if (shop.Exists(x => x == "heart"))
        {
            heart.SetActive(true);
        }

        if (shop.Exists(x => x == "axe"))
        {
            axe.SetActive(true);
        }
    }

}
