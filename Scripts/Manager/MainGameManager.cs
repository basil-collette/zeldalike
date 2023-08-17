using Assets.Scripts.Manager;
using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;

public class MainGameManager : SignletonGameObject<MainGameManager>
{
    public TargetScene firstLoadedScene;
    public GameObject CanvaUI;
    public GameObject CanvaControls;
    public bool resetBDD = false;

    void Start()
    {
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
        if (!DbExists())
            resetBDD = true;

        if (resetBDD)
        {
            DatabaseHelper.ResetTables();
            //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
        }
    }

    bool DbExists()
    {
        using (SqliteConnection connexion = DatabaseHelper.GetConnexion())
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND (name='item' OR name='weapon');";
            var command = new SqliteCommand(query, connexion);
            command.ExecuteNonQuery();
            var result = command.ExecuteReader();
            if (result.HasRows)
            {
                return true;
            }
        }

        return false;
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
