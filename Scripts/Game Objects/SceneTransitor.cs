using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitor : MonoBehaviour
{
    public string sceneToLoad;
    bool usePreload;
    public bool needPreload = false;
    public Vector2 playerNextPosition;
    public VectorValue playerPositionStorage;

    void Start()
    {
        var askedScene = SceneManager.GetSceneByName(sceneToLoad);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneToLoad + "\".");
        }

        if (needPreload)
        {
            usePreload = false;
            StartCoroutine(PreloadScene());
        }
    }

    void OnDestroy()
    {
        if (needPreload && !usePreload)
        {
            SceneManager.UnloadSceneAsync(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerPositionStorage.initalValue = playerNextPosition;

            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            unloadOp.completed += (AsyncOperation op) =>
            {
                if (needPreload)
                {
                    usePreload = true;
                }
                else
                {
                    AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
                    loadOp.completed += (AsyncOperation op) =>
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
                    };
                }
            };
        }
    }

    IEnumerator PreloadScene()
    {
        yield return null;

        AsyncOperation asyncLoadOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        asyncLoadOp.allowSceneActivation = false;

        while (!asyncLoadOp.isDone)
        {
            //Debug.Log("Loading progress: " + (asyncLoadOp.progress * 100) + "%");

            if (asyncLoadOp.progress >= 0.9f)
            {
                if (usePreload)
                {
                    asyncLoadOp.allowSceneActivation = true;

                    yield return null;

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

}
