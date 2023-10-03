using Assets.Database.Model.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SaveManager : SingletonGameObject<SaveManager>
{
    public static GameData GameData;
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
            LoadData();
        }
    }

    public void SaveGame()
    {
        GameData gameData = GetGameDataFromRunning();
        WriteSave(gameData);

        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<ToastManager>().Add(new Toast("La partie à été sauvegardée!", ToastType.Success));
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
            playerMaxHealth = player.GetComponent<Health>()._health.initialValue,

            inventory = MainGameManager._inventoryManager.ToJsonString(),

            dialoguesStates = MainGameManager._dialogStatesManager.ToJsonString(),

            quests = MainGameManager._questbookManager.ToJsonString(),

            events = MainGameManager._storyEventManager.ToJsonString()
        };
    }

    public void CreateNewSave(string _saveName)
    {
        string FIRST_WEAPON_CODE = "sword";
        MainGameManager._inventoryManager.Set(new InventorySaveModel() { Weapon = Singleton<WeaponRepository>.Instance.GetByCode(FIRST_WEAPON_CODE) });

        string FIRST_QUEST_NAME = "Je m'appelle...";
        var firstQuest = Resources.Load<ScriptableQuest>($"ScriptableObjects/quests/{FIRST_QUEST_NAME}");
        MainGameManager._questbookManager.Set(new QuestbookSaveModel() { Quests = { new Quest(firstQuest) } });
        var test = JsonUtility.ToJson(new StoryEventSaveModel());

        GameData gameData = new GameData() {
            saveName = _saveName,
            sceneName = StartScene.libelle,
            position = new Vector3(-2.3f, 1.75f, 0),
            inventory = MainGameManager._inventoryManager.ToJsonString(),
            playerHealth = 3f,
            playerMaxHealth = 3f,
            dialoguesStates = JsonUtility.ToJson(new DialogStatesSaveModel()),
            quests = MainGameManager._questbookManager.ToJsonString(),
            events = JsonUtility.ToJson(new StoryEventSaveModel())
        };

        WriteSave(gameData);
    }

    public void EraseSave()
    {
        string saveFolderPath = Path.Combine(Application.persistentDataPath, $"Save/main");
        if (Directory.Exists(saveFolderPath))
        {
            //popup confirmation

            GetComponent<PauseManager>().Resume();

            GetComponent<MainGameManager>().ShowMenuScene();

            Directory.Delete(saveFolderPath, true);

            CreateNewSave("main");

            FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<ToastManager>().Add(new Toast("La partie à été supprimée!", ToastType.Success));
        }
    }

    public void InitSave()
    {
        var savePath = Path.Combine(Application.persistentDataPath, "Save");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        if (GetSaveNames().Count == 0)
        {
            CreateNewSave("main");
        }
    }

    public void LoadData()
    {
        //SCENE
        TargetScene currentScene = Resources.Load<TargetScene>($"Scenes/{GameData.sceneName.Substring(0, GameData.sceneName.Length - 5)}/{GameData.sceneName}");
        FindAnyObjectByType<CameraMovement>().CameraParams = currentScene.cameraParameters;

        //PLAYER
        FloatValue playerHealth = Resources.Load<FloatValue>("ScriptableObjects/Player/Health/PlayerHealth");
        playerHealth.initialValue = GameData.playerMaxHealth;
        playerHealth.RuntimeValue = GameData.playerHealth;
        Resources.Load<VectorValue>("ScriptableObjects/Player/position/PlayerPosition").initalValue = GameData.position;

        //INVENTORY
        MainGameManager._inventoryManager.Load(GameData.inventory);

        //Story Events
        MainGameManager._storyEventManager.Load(GameData.events);

        //DIALOGUES STATES
        MainGameManager._dialogStatesManager.Load(GameData.dialoguesStates);

        //QUESTS
        MainGameManager._questbookManager.Load(GameData.quests);
    }

}