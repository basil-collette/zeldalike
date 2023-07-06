using Assets.Scripts.Manager;
using UnityEngine;

public class MainGameManager : SignletonGameObject<MainGameManager>
{
    public TargetScene firstLoadedScene;
    public GameObject CanvaUI;
    public GameObject CanvaControls;
    public bool resetBDD = false;

    void Start()
    {
        InitBDD();

        InitSave();

        ScenesManager scenesManager = GetComponent<ScenesManager>();
        scenesManager.ClearScenes();
        scenesManager.AdditiveLoadScene(firstLoadedScene.libelle, () => {
            scenesManager.SetCurrentScene(firstLoadedScene.libelle);
        });
    }

    void InitBDD()
    {
        if (resetBDD)
        {
            DatabaseHelper.ResetTables();
            //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
        }
    }

    void InitSave()
    {
        SaveManager saveManager = GetComponent<SaveManager>();
        if (saveManager.GetSaveNames().Count == 0)
        {
            saveManager.CreateNewSave("main");
        }
    }

    public void StartGame()
    {
        SaveManager saveManager = GetComponent<SaveManager>();
        saveManager.LoadGame("main");

        string sceneName = saveManager.GameData.sceneName;
        string scenePath = $"Scenes/{sceneName.Substring(0, sceneName.Length - 5)}/{sceneName}";
        TargetScene targetScene = Resources.Load<TargetScene>(scenePath);
        GetComponent<ScenesManager>().SwitchScene(targetScene);

        CanvaUI.SetActive(true);
        CanvaControls.SetActive(true);
    }

}
