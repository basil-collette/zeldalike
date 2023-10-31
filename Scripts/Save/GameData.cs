using UnityEngine;

[System.Serializable]
public class GameData
{
    public string saveName;

    public string sceneName;
    public Vector3 position;
    public Vector3 direction;

    public string inventory;

    public float playerHealth;
    public float playerMaxHealth;

    public string dialoguesStates;

    public string quests;

    public string events;
}