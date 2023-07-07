using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string saveName;
    public string sceneName;
    public List<string> inventoryItems;
    public List<string> inventoryHotbars;
    public string? inventoryWeapon;
    public float playerHealth;

    //etat du player, vie, etat
    //questlog
    //coffres ouverts
}