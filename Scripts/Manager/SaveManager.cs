using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
using Assets.Tools;
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
        Player player = FindAnyObjectByType<Player>();

        PNJDialogues[] dialogues = Resources.LoadAll<PNJDialogues>("ScriptableObjects/Dialogues/PNJ Dialogues/");
        SerializableDic<string, SerializableDic<string, bool>> dialoguesStates = new SerializableDic<string, SerializableDic<string, bool>>()
        {
            keys = dialogues.Select(x => x.name).ToList(),
            values = dialogues.Select(x => new SerializableDic<string, bool>()
            {
                keys = x.Dialogues.Select(y => y.NameCode).ToList(),
                values = x.Dialogues.Select(y => y.IsSaid).ToList()
            }).ToList()
        };

        return new GameData() {
            saveName = GameData.saveName,

            sceneName = FindAnyObjectByType<ScenesManager>()._currentScene,
            position = player.transform.position,

            playerHealth = player.GetComponent<Health>()._health.RuntimeValue,

            inventoryItems = player.inventory.Items.Select(x => JsonUtility.ToJson(x)).ToList(),
            inventoryHotbars = player.inventory.Hotbars.Select(x => JsonUtility.ToJson(x)).ToList(),
            inventoryWeapon = (player.inventory.Weapon == null || player.inventory.Weapon.Id == 0) ? null : JsonUtility.ToJson(player.inventory.Weapon),

            opennedChestGuids = GameData.opennedChestGuids,

            dialoguesStates = JsonUtility.ToJson(DialogueStates.Current),

            quests = player.playerQuest.PlayerQuests.Select(x => JsonUtility.ToJson(x)).ToList()
        };
    }

    public void CreateNewSave(string _saveName)
    {
        Weapon sword = Singleton<WeaponRepository>.Instance.GetByCode("sword");

        GameData gameData = new GameData() {
            saveName = _saveName,
            sceneName = StartScene.libelle,
            position = new Vector3(-2.3f, 1.75f, 0),
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
        Resources.Load<VectorValue>("ScriptableObjects/Player/position/PlayerPosition").initalValue = GameData.position;
        
        //INVENTORY
        Inventory inventory = Resources.Load<Inventory>("ScriptableObjects/Player/Inventory/Inventory");
        inventory.Items = GameData.inventoryItems.Select(x => GetSerializedItem(x)).ToList();
        inventory.Hotbars = GameData.inventoryHotbars.Select(x => GetSerializedItem(x) as HoldableItem).ToList();
        inventory.Weapon = (GameData.inventoryWeapon == null || GameData.inventoryWeapon == string.Empty) ? null : GetSerializedItem(GameData.inventoryWeapon) as Weapon;

        //DIALOGUES STATES
        DialogueStates.Current.States = JsonUtility.FromJson<List<SerializableWrappedList<string>>>(GameData.dialoguesStates);

        //QUESTS
        PlayerQuest playerQuest = Resources.Load<PlayerQuest>("ScriptableObjects/Player/Quest/PlayerQuest");
        playerQuest.PlayerQuests.Clear();
        for (int i = 0; i < GameData.quests.Count; i++) {
            TempQuest tempQuest = JsonUtility.FromJson<TempQuest>(GameData.quests[i]);
            Quest quest = Resources.Load<Quest>($"ScriptableObjects/quests/{tempQuest.Name}");
            JsonUtility.FromJsonOverwrite(GameData.quests[i], quest);
            playerQuest.AddQuest(quest);
        }

        //COFFRES
        //gardé en mémoire dans GameData
    }

    Item GetSerializedItem(string json)
    {
        Item item = Item.InstanciateFromJsonString(json);

        switch (item.ItemType)
        {
            case ItemTypeEnum.holdable:
                return HoldableItem.InstanciateFromJsonString(json);

            case ItemTypeEnum.weapon:
                return Weapon.InstanciateFromJsonString(json);
            
            case ItemTypeEnum.item:
            default:
                return item;
        }
    }

}