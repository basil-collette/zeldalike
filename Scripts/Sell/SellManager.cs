using System;
using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public GameObject rope;
    public GameObject heart;
    public GameObject axe;

    void Start()
    {
        List<string> shop = SaveManager.GameData.events.Shop;

        if (shop.Exists(x => x ==  "rope"))
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
