using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string saveName;
    public string sceneName;
    public List<string> inventoryItemsCodes;
    public List<string> inventoryHotbarsCodes;
    public string? inventoryWeaponCode;
    public float playerHealth;

    //etat du player, vie, etat
    //questlog
    //coffres ouverts
}