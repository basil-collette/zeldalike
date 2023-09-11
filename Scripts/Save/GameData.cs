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

    public EventCodes events;
}

public class EventCodes
{
    public List<string> Shop = new List<string>();
    public List<string> Scenario = new List<string>();
    public List<string> MapDiscovery = new List<string>();
    public List<string> Tutorial = new List<string>();
}