using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour //SignletonGameObject<StartGameManager>
{
    public VectorValue initialPlayerPos;

    static readonly Vector2 START_PLAYER_POS = new Vector2(-2.5f, 1.8f);
    static readonly string START_SCENE_NAME = "HomeScene";

    void Start()
    {
        initialPlayerPos.initalValue = START_PLAYER_POS;

        StartCoroutine(StartFirstSceneCo());
    }

    IEnumerator StartFirstSceneCo()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(START_SCENE_NAME, LoadSceneMode.Single); //, LoadSceneMode.Additive

        while (!loadOp.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(START_SCENE_NAME));
        //DestroyImmediate(gameObject);
    }

}
