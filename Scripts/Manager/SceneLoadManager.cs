using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
class PreloadedScene
{
    public string Name = "Anonymous Scene";
    public bool useScene = false;
    public bool canceled = false;
}

public class SceneLoadManager : MonoBehaviour
{
    List<PreloadedScene> preloadedScenes = new List<PreloadedScene>();
    /*
    Canvas canvas;
    public GameObject fadeInPanel;
    */

    void Start()
    {
        /*
        canvas = FindAnyObjectByType<Canvas>();
        GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
        panel.transform.SetParent(canvas.transform, false);
        Destroy(panel, 1);
        */
    }

    void OnDestroy()
    {
        foreach (PreloadedScene pScene in preloadedScenes)
        {
            if (pScene.useScene == false)
            {
                pScene.canceled = true;
            }
        }
    }

    public void Test(string sceneName)
    {
        StartCoroutine(TestCo(sceneName));
    }

    IEnumerator TestCo(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(gameObject.scene.name);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        SceneManager.UnloadScene(scene);

        Resources.UnloadUnusedAssets();

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    IEnumerator LoadSceneCo(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, loadmode);

        while (!loadOp.isDone)
        {
            yield return null;
        }

        if (resultCallback != null) resultCallback();
    }

    IEnumerator UnloadSceneCo(string sceneName, Action resultCallback = null)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);

        while (!unloadOp.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();

        if (resultCallback != null) resultCallback();
    }

    IEnumerator PreloadScene(string sceneName, Func<string, bool> UseSceneCallback)
    {
        AsyncOperation asyncLoadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoadOp.allowSceneActivation = false;

        while (!asyncLoadOp.isDone)
        {
            //Debug.Log("Loading progress: " + (asyncLoadOp.progress * 100) + "%");

            if (asyncLoadOp.progress >= 0.9f)
            {
                if (UseSceneCallback(sceneName))
                {
                    Scene scene = SceneManager.GetSceneByName(gameObject.scene.name);

                    GameObject[] rootObjects = scene.GetRootGameObjects();
                    foreach (GameObject obj in rootObjects)
                    {
                        obj.SetActive(false);
                    }

                    asyncLoadOp.allowSceneActivation = true;

                    SceneManager.UnloadScene(scene);

                    Resources.UnloadUnusedAssets();

                    yield return null;

                    Debug.Log(preloadedScenes.ToArray().ToString());

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void UsePreloadedScene(string sceneName)
    {
        preloadedScenes.Find(pScene => pScene.Name == sceneName).useScene = true;
    }

    bool MustUsePreloadedScene(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.Name == sceneName).useScene;
    }

    #region CALLS

    public void UnloadScene(string sceneName, Action resultCallback = null)
    {
        var askedScene = SceneManager.GetSceneByName(sceneName);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneName + "\".");
        }

        StartCoroutine(UnloadSceneCo(sceneName, resultCallback));
    }

    public void LoadScene(string sceneName, Action resultCallback = null)
    {
        LoadScene(sceneName, LoadSceneMode.Single, resultCallback);
    }

    public void LoadScene(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        var askedScene = SceneManager.GetSceneByName(sceneName);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneName + "\".");
        }

        StartCoroutine(LoadSceneCo(sceneName, loadmode, resultCallback));
    }

    public void AdditiveLoadScene(string sceneName, Action resultCallback = null)
    {
        var askedScene = SceneManager.GetSceneByName(sceneName);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneName + "\".");
        }

        StartCoroutine(LoadSceneCo(sceneName, LoadSceneMode.Additive, resultCallback));
    }

    public void PreloadScene(string sceneName)
    {
        var askedScene = SceneManager.GetSceneByName(sceneName);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneName + "\".");
        }

        //Check if allready in preloaded list
        if (preloadedScenes.Find(pScene => pScene.Name == sceneName) != null) return;

        preloadedScenes.Add(new PreloadedScene() {
            Name = sceneName,
            useScene = false
        });

        Debug.Log(preloadedScenes.ToArray().ToString());

        StartCoroutine(PreloadScene(sceneName, MustUsePreloadedScene));
    }

    #endregion

}

/*
PreloadedScene pScene = preloadedScenes.Find(pScene => pScene.Name == sceneName);

if (pScene.useScene)
{
    Debug.Log(sceneName + " " + pScene.canceled);
    if (pScene.canceled)
    {
        SceneManager.UnloadScene(sceneName);
    }
    else
    {
        Scene scene = SceneManager.GetSceneByName(gameObject.scene.name);

        /*
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }
        */
        /*

        asyncLoadOp.allowSceneActivation = true;

        SceneManager.UnloadScene(scene);

        Resources.UnloadUnusedAssets();

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}
*/