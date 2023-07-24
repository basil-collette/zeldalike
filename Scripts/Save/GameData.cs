using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string saveName;

    public string sceneName;
    public Vector3 position;

    public List<string> inventoryItems;
    public List<string> inventoryHotbars;
    public string? inventoryWeapon;

    public float playerHealth;

    public List<string> opennedChestGuids;

    public string dialoguesStates;

    public List<string> quests;
}