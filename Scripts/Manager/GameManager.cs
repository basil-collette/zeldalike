using Assets.Scripts.Manager;
using UnityEngine;

public class GameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public TargetScene firstLoadedScene;
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

        scenesManager.AdditiveLoadScene(firstLoadedScene, () => {
            scenesManager.SetCurrentScene(firstLoadedScene.libelle);

            if (destroyAfterLoad) {
                DestroyImmediate(gameObject);
            }
        });
    }

}
