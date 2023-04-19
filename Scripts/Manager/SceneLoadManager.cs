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
        Resources.UnloadUnusedAssets();
        /*
        canvas = FindAnyObjectByType<Canvas>();
        GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
        panel.transform.SetParent(canvas.transform, false);
        Destroy(panel, 1);
        */
    }

    void OnDestroy()
    {
        //
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    IEnumerator AsyncLoadSceneCo(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
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
        Scene scene = SceneManager.GetSceneByName(sceneName);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        SceneManager.UnloadScene(scene);

        Resources.UnloadUnusedAssets();

        if (resultCallback != null) resultCallback();

        return null;
    }

    IEnumerator PreloadScene(string sceneName, Func<string, bool> UseSceneCallback, Func<string, bool> IsCanceledCallBack, Action<string> RemovePreloadedSceneCallback, Func<int> GetPreloadedScenesCountCallback)
    {
        AsyncOperation asyncLoadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoadOp.allowSceneActivation = false;

        while (!asyncLoadOp.isDone)
        {
            //Debug.Log(sceneName + " scene is loading progress: " + (asyncLoadOp.progress * 100) + "%");

            if (IsCanceledCallBack(sceneName))
            {
                asyncLoadOp.allowSceneActivation = true;

                Scene scene = SceneManager.GetSceneByName(sceneName);
                while(!scene.isLoaded)
                {
                    scene = SceneManager.GetSceneByName(sceneName);
                    yield return null;
                }

                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    obj.SetActive(false);
                }

                SceneManager.UnloadScene(scene);

                Resources.UnloadUnusedAssets();

                RemovePreloadedSceneCallback(sceneName);
            }
            else
            {
                if (asyncLoadOp.progress >= 0.9f)
                {
                    if (UseSceneCallback(sceneName))
                    {
                        foreach (PreloadedScene pScene in preloadedScenes)
                        {
                            if (pScene.useScene == false)
                            {
                                pScene.canceled = true;
                            }
                        }

                        asyncLoadOp.allowSceneActivation = true;

                        while (GetPreloadedScenesCountCallback() > 1)
                        {
                            yield return null;
                        }

                        Scene scene = SceneManager.GetSceneByName(gameObject.scene.name);

                        foreach (GameObject obj in scene.GetRootGameObjects())
                        {
                            obj.SetActive(false);
                        }

                        SceneManager.UnloadScene(scene);

                        Resources.UnloadUnusedAssets();

                        yield return null;

                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void UsePreloadedScene(string sceneName)
    {
        preloadedScenes.Find(pScene => pScene.Name == sceneName).useScene = true;
    }

    public bool IsCanceled(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.Name == sceneName).canceled;
    }

    bool MustUsePreloadedScene(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.Name == sceneName).useScene;
    }

    int GetPreloadedScenesCount()
    {
        return preloadedScenes.Count;
    }

    void RemovePreloadedScene(string sceneName)
    {
        preloadedScenes.Remove(preloadedScenes.Find(pScene => pScene.Name == sceneName));
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(AsyncLoadSceneCo(sceneName));
    }

    IEnumerator AsyncLoadSceneCo(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(gameObject.scene.name);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        
        SceneManager.UnloadScene(scene);

        Resources.UnloadUnusedAssets();
    }

    public void UnloadScene(string sceneName, Action resultCallback = null)
    {
        var askedScene = SceneManager.GetSceneByName(sceneName);
        if (!askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneName + "\".");
        }

        StartCoroutine(UnloadSceneCo(sceneName, resultCallback));
    }

    public void AsyncAdditiveLoadScene(string sceneName, Action resultCallback = null)
    {
        AsyncLoadScene(sceneName, LoadSceneMode.Additive, resultCallback);
    }

    public void AsyncLoadScene(string sceneName, Action resultCallback = null)
    {
        AsyncLoadScene(sceneName, LoadSceneMode.Single, resultCallback);
    }

    public void AsyncLoadScene(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        StartCoroutine(AsyncLoadSceneCo(sceneName, loadmode, resultCallback));
    }

    public void PreloadScene(string sceneName)
    {
        //Check if allready in preloaded list
        if (preloadedScenes.Find(pScene => pScene.Name == sceneName) != null) return;

        preloadedScenes.Add(new PreloadedScene() {
            Name = sceneName,
            useScene = false
        });

        StartCoroutine(PreloadScene(sceneName, MustUsePreloadedScene, IsCanceled, RemovePreloadedScene, GetPreloadedScenesCount));
    }

}
