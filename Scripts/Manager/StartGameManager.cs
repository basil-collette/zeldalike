using Assets.Scripts.Manager;
using UnityEngine;

public class StartGameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public ScenesManager scenesManager;

    static readonly string START_SCENE_NAME = "HomeScene";

    void Start()
    {
        //DatabaseHelper.ResetTableItem();

        scenesManager.ClearScenes();

        scenesManager.AdditiveLoadScene(START_SCENE_NAME, () => {
            DestroyImmediate(gameObject);

            scenesManager.SetCurrentScene(START_SCENE_NAME);
        });
    }

}
