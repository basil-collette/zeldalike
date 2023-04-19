using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    //static readonly Vector2 START_PLAYER_POS = new Vector2(-2.5f, 1.8f);
    static readonly string START_SCENE_NAME = "HomeScene";

    void Start()
    {
        //initialPlayerPos.initalValue = START_PLAYER_POS;

        if (SceneManager.sceneCount > 1)
        {
            Scene[] scenes = SceneManager.GetAllScenes();
            for(int i = 1; i< scenes.Length; i++)
            {
                SceneManager.UnloadScene(scenes[i]);
            }
        }

        GetComponent<SceneLoadManager>().AsyncAdditiveLoadScene(START_SCENE_NAME, () => {
            DestroyImmediate(gameObject);
        });
    }

}
