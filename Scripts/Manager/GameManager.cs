using UnityEngine;

public class GameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public ScenesManager scenesManager;

    static readonly string START_SCENE_NAME = "HomeScene"; //"HomeScene";

    void Start()
    {
        //DatabaseHelper.ResetTableItem();
        //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));

        scenesManager.ClearScenes();

        scenesManager.AdditiveLoadScene(START_SCENE_NAME, () => {
            scenesManager.SetCurrentScene(START_SCENE_NAME);
            //DestroyImmediate(gameObject);
        });
    }

}
