using Assets.Scripts.Manager;
using System.IO;
using UnityEngine;

public class MainGameManager : SingletonGameObject<MainGameManager>
{
    public TargetScene firstLoadedScene;
    public GameObject CanvaUI;
    public GameObject CanvaControls;
    public bool resetBDD = false;

    public static QuestManager _questManager;
    public static DialogStatesManager _dialogStatesManager;
    public static InventoryManager _inventoryManager;
    public static StoryEventManager _storyEventManager;
    public static SoundManager _soundManager;
    public static SaveManager _saveManager;
    public static ToastManager _toastManager;

    void Start()
    {
        //Hide URP Debuger
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        _questManager = new QuestManager();
        _dialogStatesManager = new DialogStatesManager();
        _inventoryManager = new InventoryManager();
        _storyEventManager = new StoryEventManager();
        _soundManager = GetComponentInChildren<SoundManager>();
        _saveManager = GetComponent<SaveManager>();
        _toastManager = GetComponent<ToastManager>();

        //Application.targetFrameRate = 10;

        InitLocalDataFolder();

        //InitBDD();

        GetComponent<SaveManager>().InitSave();

        ShowMenuScene();

        /*
        MethodInfo info = GetType().GetMethod(nameof(Test));
        Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, info);
        OnCompleted.AddListener(() => action.Invoke("value"));
        */

        //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
        //FindAnyObjectByType<ToastManager>().Add(new Toast("La partie à été supprimée!", ToastType.Success));
    }

    public void Test(string param)
    {
        Debug.Log("success : " + param);
    }

    public void Test2()
    {
        Debug.Log("success");
    }

    public void ShowMenuScene()
    {
        _soundManager.StopMusic();

        ScenesManager scenesManager = GetComponent<ScenesManager>();
        scenesManager.ClearScenes();
        scenesManager.AdditiveLoadScene(firstLoadedScene.nameCode, () => {
            scenesManager.SetCurrentScene(firstLoadedScene.nameCode);
            _soundManager.PlayMusic(firstLoadedScene.musicName);
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

    /*
    void InitBDD()
    {
        if (!DatabaseHelper.DbExists("item") || !DatabaseHelper.DbExists("weapon"))
        {
            resetBDD = true;
        }

        if (resetBDD)
        {
            DatabaseHelper.ResetTables();
        }
    }
    */

    public void StartGame()
    {
        _saveManager.LoadGame("main");

        string sceneName = SaveManager.GameData.sceneName;
        string scenePath = $"Scenes/{sceneName.Substring(0, sceneName.Length - 5)}/{sceneName}";
        TargetScene targetScene = Resources.Load<TargetScene>(scenePath);
        GetComponent<ScenesManager>().SwitchScene(targetScene);

        CanvaUI.SetActive(true);
        CanvaControls.SetActive(true);
    }

}
