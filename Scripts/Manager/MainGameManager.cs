using Assets.Scripts.Manager;
using System.IO;
using UnityEngine;

public class MainGameManager : SingletonGameObject<MainGameManager>
{
    public TargetScene firstLoadedScene;
    public GameObject CanvaUI;
    public GameObject CanvaControls;
    public bool resetBDD = false;

    public static QuestbookManager _questbookManager;
    public static DialogStatesManager _dialogStatesManager;
    public static InventoryManager _inventoryManager;
    public static StoryEventManager _storyEventManager;


    //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
    //FindAnyObjectByType<ToastManager>().Add(new Toast("La partie à été supprimée!", ToastType.Success));

    void Start()
    {
        //Hide URP Debuger
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        _questbookManager = new QuestbookManager();
        _dialogStatesManager = new DialogStatesManager();
        _inventoryManager = new InventoryManager();
        _storyEventManager = new StoryEventManager();

        //Application.targetFrameRate = 10;

        InitLocalDataFolder();

        InitBDD();

        GetComponent<SaveManager>().InitSave();

        ShowMenuScene();
    }

    public void ShowMenuScene()
    {
        ScenesManager scenesManager = GetComponent<ScenesManager>();
        scenesManager.ClearScenes();
        scenesManager.AdditiveLoadScene(firstLoadedScene.libelle, () => {
            scenesManager.SetCurrentScene(firstLoadedScene.libelle);
        });
    }

    private void InitLocalDataFolder()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);

            resetBDD = true;
        }
    }

    void InitBDD()
    {
        if (!DatabaseHelper.DbExists())
        {
            resetBDD = true;
        }

        if (resetBDD)
        {
            DatabaseHelper.ResetTables();
        }
    }

    public void StartGame()
    {
        SaveManager saveManager = GetComponent<SaveManager>();
        saveManager.LoadGame("main");

        string sceneName = SaveManager.GameData.sceneName;
        string scenePath = $"Scenes/{sceneName.Substring(0, sceneName.Length - 5)}/{sceneName}";
        TargetScene targetScene = Resources.Load<TargetScene>(scenePath);
        GetComponent<ScenesManager>().SwitchScene(targetScene);

        CanvaUI.SetActive(true);
        CanvaControls.SetActive(true);
    }

}
