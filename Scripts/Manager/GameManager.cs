using Assets.Scripts.Manager;
using UnityEngine;

public class GameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public string firstLoadedSceneName = "HomeScene";
    public ScenesManager scenesManager;
    public bool destroyAfterLoad = false;
    public bool resetBDD = false;

    void Start()
    {
        if (resetBDD)
        {
            DatabaseHelper.ResetTableItem();
            //Assembly.GetAssembly(typeof(BaseRepitory<T>)).GetTypes().FirstOrDefault(testc => testc.isSubsclassOf(typeof(GenericRepitory<T>)));
        }

        scenesManager.ClearScenes();

        scenesManager.AdditiveLoadScene(firstLoadedSceneName, () => {
            scenesManager.SetCurrentScene(firstLoadedSceneName);

            if (destroyAfterLoad) {
                DestroyImmediate(gameObject);
            }
        });
    }

}
