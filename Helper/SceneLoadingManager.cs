using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public void UnloadScene(string sceneName, Action resultCallback = null)
    {
        StartCoroutine(UnloadSceneCo(sceneName, resultCallback));
    }

    public void LoadScene(string sceneName, Action resultCallback = null)
    {
        LoadScene(sceneName, LoadSceneMode.Single, resultCallback);
    }

    public void LoadScene(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        StartCoroutine(LoadSceneCo(sceneName, loadmode, resultCallback));
    }

    public void AdditiveLoadScene(string sceneName, Action resultCallback = null)
    {
        StartCoroutine(LoadSceneCo(sceneName, LoadSceneMode.Additive, resultCallback));
    }

    IEnumerator LoadSceneCo(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, loadmode);

        while (!loadOp.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        if (resultCallback != null) resultCallback();
    }

    IEnumerator UnloadSceneCo(string sceneName, Action resultCallback = null)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);

        while (!unloadOp.isDone)
        {
            yield return null;
        }

        if (resultCallback != null) resultCallback();
    }

    /*
    IEnumerator PreloadSceneCo(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, loadmode);
        loadOp.allowSceneActivation = false;

        while (!loadOp.isDone)
        {
            if (loadOp.progress >= 0.9f)
            {
                if (usePreload)
                {
                    asyncLoadOp.allowSceneActivation = true;

                    yield return null;

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
                }
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        if (resultCallback != null) resultCallback();
    }
    */
}