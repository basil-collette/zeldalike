using UnityEngine;

public class StartGameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public ScenesManager scenesManager;

    static readonly string START_SCENE_NAME = "HomeScene";

    void Start()
    {
        //initialPlayerPos.initalValue = START_PLAYER_POS;

        scenesManager.ClearScenes();

        scenesManager.AdditiveLoadScene(START_SCENE_NAME, () => {
            //DestroyImmediate(gameObject);

            scenesManager.SetCurrentScene(START_SCENE_NAME);
        });
    }

}
