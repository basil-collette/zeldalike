using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SaveManager : SignletonGameObject<SaveManager>
{
    public GameData GameData;
    public TargetScene StartScene;

    void Start()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Save");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    public void LoadGame(string gameDataName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, $"Save/{gameDataName}/gameData.json");

        string loadedJson = File.ReadAllText(savePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(loadedJson);
        if (loadedData != null)
        {
            GameData = loadedData;
            /*score = loadedData.score;
            playerHealth = loadedData.playerHealth;
            playerTransform.position = loadedData.playerPosition;*/
        }
    }

    public void SaveGame()
    {
        GameData gameData = GetCurrentRunningData();
        WriteSave(gameData);
    }

    void WriteSave(GameData gameData)
    {
        string saveFolderPath = Path.Combine(Application.persistentDataPath, $"Save/{gameData.saveName}");
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText($"{saveFolderPath}/gameData.json", jsonData);
    }

    public List<string> GetSaveNames()
    {
        List<string> saveFiles = new List<string>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "Save"));
        foreach (var file in directoryInfo.GetFiles("*.json"))
        {
            saveFiles.Add(file.Name);
        }
        return saveFiles;
    }

    public GameData GetCurrentRunningData()
    {
        GameData gameData = new GameData();

        gameData.saveName = GameData.saveName;
        gameData.sceneName = FindAnyObjectByType<ScenesManager>()._currentScene;

        Player player = FindGameObjectHelper.FindByName("Player").GetComponent<Player>();
        gameData.inventoryItemsCodes = player.inventory.Items.AsEnumerable().Select(x => x.NameCode).ToList();
        gameData.inventoryHotbarsCodes = player.inventory.Hotbars.AsEnumerable().Select(x => x.NameCode).ToList();
        gameData.inventoryWeaponCode = player.inventory.Weapon.NameCode;
        gameData.playerHealth = player.GetComponent<Health>().health.RuntimeValue;

        return gameData;
    }

    public void CreateNewSave(string _saveName)
    {
        GameData gameData = new GameData() {
            sceneName = StartScene.libelle,
            saveName = _saveName,
            inventoryItemsCodes = new List<string>(),
            inventoryHotbarsCodes = new List<string>(),
            inventoryWeaponCode = "sword",
            playerHealth = 3f
        };

        WriteSave(gameData);
    }

    public void SetDataToRunning()
    {
        /*
        find scriptable object of player health
        find scriptable object of player inventory
        find scriptableobject of player questlog

        Player player = FindGameObjectHelper.FindByName("Player").GetComponent<Player>();
        gameData.inventoryItemsCodes = player.inventory.Items.AsEnumerable().Select(x => x.NameCode).ToList();
        gameData.inventoryHotbarsCodes = player.inventory.Hotbars.AsEnumerable().Select(x => x.NameCode).ToList();
        gameData.inventoryWeaponCode = player.inventory.Weapon.NameCode;
        gameData.playerHealth = player.GetComponent<Health>().health.RuntimeValue;*/
    }

}