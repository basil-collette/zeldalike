using Assets.Scripts.Manager;
using UnityEngine;
using System;

public class MainGameManager : SignletonGameObject<MainGameManager>
{
    public TargetScene firstLoadedScene;

    public ScenesManager scenesManager;
    public bool resetBDD = false;

    //public static event Action OnTrigger; //Observer Patern

    void Start()
    {
        if (resetBDD)
        {
            DatabaseHelper.ResetTableItem();
            //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
        }

        scenesManager.ClearScenes();

        scenesManager.AdditiveLoadScene(firstLoadedScene, () => {
            scenesManager.SetCurrentScene(firstLoadedScene.libelle);
        });
    }

}
