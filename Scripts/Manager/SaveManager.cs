using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
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
            SetDataToRunning();
        }
    }

    public void SaveGame()
    {
        GameData gameData = GetGameDataFromRunning();
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
        foreach (var file in directoryInfo.GetDirectories())
        {
            saveFiles.Add(file.Name);
        }
        return saveFiles;
    }

    public GameData GetGameDataFromRunning()
    {
        Player player = FindGameObjectHelper.FindByName("Player").GetComponent<Player>();

        return new GameData() {
            saveName = GameData.saveName,
            sceneName = FindAnyObjectByType<ScenesManager>()._currentScene,

            playerHealth = player.GetComponent<Health>().health.RuntimeValue,

            inventoryItems = player.inventory.Items.AsEnumerable().Select(x => JsonUtility.ToJson(x)).ToList(),
            inventoryHotbars = player.inventory.Hotbars.AsEnumerable().Select(x => JsonUtility.ToJson(x)).ToList(),
            inventoryWeapon = JsonUtility.ToJson(player.inventory.Weapon)
        };
    }

    public void CreateNewSave(string _saveName)
    {
        Weapon sword = Singleton<WeaponRepository>.Instance.GetByCode("sword");

        GameData gameData = new GameData() {
            sceneName = StartScene.libelle,
            saveName = _saveName,
            inventoryItems = new List<string>(),
            inventoryHotbars = new List<string>(),
            inventoryWeapon = JsonUtility.ToJson(sword),
            playerHealth = 3f
        };

        WriteSave(gameData);
    }

    public void SetDataToRunning()
    {
        //SCENE
        TargetScene currentScene = Resources.Load<TargetScene>($"Scenes/{GameData.sceneName.Substring(0, GameData.sceneName.Length - 5)}/{GameData.sceneName}");
        FindAnyObjectByType<CameraMovement>().CameraParams = currentScene.cameraParameters;

        //PLAYER
        Resources.Load<FloatValue>("ScriptableObjects/Player/Health/PlayerHealth").RuntimeValue = GameData.playerHealth;
        //Resources.Load<FloatValue>("ScriptableObjects/Player/position/PlayerPosition").RuntimeValue = currentScene.pos;

        //INVENTORY
        Inventory inventory = Resources.Load<Inventory>("ScriptableObjects/Player/Inventory/Inventory");
        inventory.Items = GameData.inventoryItems.Select(x => JsonUtility.FromJson<Item>(x)).ToList();
        inventory.Hotbars = GameData.inventoryHotbars.Select(x => JsonUtility.FromJson<HoldableItem>(x)).ToList();
        inventory.Weapon = JsonUtility.FromJson<Weapon>(GameData.inventoryWeapon);

        //questlog
    }

}