using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
class PreloadedScene
{
    public TargetScene scene;
    public bool useScene = false;
    public bool canceled = false;
}

public class ScenesManager : MonoBehaviour
{
    public GameObject fadeToWhite;
    public GameObject fadeToVisible;

    string _currentScene = string.Empty;
    List<PreloadedScene> preloadedScenes = new List<PreloadedScene>();

    public VectorValue playerPositionStorage;

    void Start()
    {
        //StartCoroutine(FadeTransitionCo(fadeToVisible));
    }

    IEnumerator FadeTransitionCo(GameObject fadePanel)
    {
        fadePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        fadePanel.SetActive(false);
    }

    #region PROCESS

    IEnumerator SwitchSceneCo(TargetScene scene)
    {
        foreach (PreloadedScene pScene in preloadedScenes)
        {
            if (!scene.scenesNeedingPreloadNames.Contains(pScene.scene.name))
            {
                pScene.canceled = true;
            }
        }

        //Verify that there are no preloded scene that aren't needed for next scene
        while (preloadedScenes.Select(pScene => pScene.scene.name).Any(a => !scene.scenesNeedingPreloadNames.Contains(a)))
        {
            yield return null;
        }

        Scene currentScene = SceneManager.GetSceneByName(_currentScene);

        DisableGameObjects(currentScene);

        //fade to white here

        yield return StartCoroutine(UnLoadSceneCo(currentScene));

        yield return StartCoroutine(LoadSceneCo(scene.name, LoadSceneMode.Additive));

        _currentScene = scene.name;

        //fade to transparent here
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

    IEnumerator PreloadSceneCo(TargetScene scene,
        Func<string, bool> MustUseSceneCallback,
        Action<string[]> CancelPreloadCallback,
        Func<string, bool> IsCanceledCallBack,
        Action<string> RemovePreloadedSceneCallback,
        Func<int> GetPreloadedScenesCountCallback)
    {
        AsyncOperation asyncLoadOp = SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
        asyncLoadOp.allowSceneActivation = false;

        while (!asyncLoadOp.isDone)
        {
            //Debug.Log(scene.name + " scene is loading progress: " + (asyncLoadOp.progress * 100) + "%");

            if (IsCanceledCallBack(scene.name))
            {
                asyncLoadOp.allowSceneActivation = true;

                yield return StartCoroutine(CancelPreloadCo(scene.name, RemovePreloadedSceneCallback));
            }
            else
            {
                if (asyncLoadOp.progress >= 0.9f)
                {
                    if (MustUseSceneCallback(scene.name))
                    {
                        CancelPreloadCallback(scene.scenesNeedingPreloadNames);

                        RemovePreloadedSceneCallback(scene.name);

                        DisableGameObjects(_currentScene);

                        asyncLoadOp.allowSceneActivation = true;

                        while (GetPreloadedScenesCountCallback() > 1)
                        {
                            yield return null;
                        }

                        //fade to white here

                        Scene currentScene = SceneManager.GetSceneByName(_currentScene);

                        yield return StartCoroutine(UnLoadSceneCo(currentScene));

                        _currentScene = scene.name;

                        //fade to transparent here

                        yield return null;

                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator CancelPreloadCo(string sceneName, Action<string> RemovePreloadedSceneCallback)
    {
        Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);
        while (!sceneToUnload.isLoaded)
        {
            sceneToUnload = SceneManager.GetSceneByName(sceneName);
            yield return null;
        }

        //DisableGameObjects(scene);

        yield return StartCoroutine(UnLoadSceneCo(sceneToUnload));

        RemovePreloadedSceneCallback(sceneName);
    }

    IEnumerator UnLoadSceneCo(string sceneName, Action resultCallback = null)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);

        while (!unloadOp.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();

        if (resultCallback != null) resultCallback();
    }

    IEnumerator UnLoadSceneCo(Scene scene, Action resultCallback = null)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);

        while (!unloadOp.isDone)
        {
            Debug.Log(scene.name + " scene is loading progress: " + (unloadOp.progress * 100) + "%");
            yield return null;
        }

        Resources.UnloadUnusedAssets();

        if (resultCallback != null) resultCallback();
    }

    #endregion

    #region PUBLIC

    public void SwitchScene(TargetScene targetScene)
    {
        if (targetScene.needPreload)
        {
            UsePreloadedScene(targetScene.name);
        }
        else
        {
            StartCoroutine(SwitchSceneCo(targetScene));
        }
    }

    public void PreloadScene(TargetScene scene)
    {
        //Check if allready in preloaded list
        if (preloadedScenes.Find(pScene => pScene.scene.name == scene.name) != null)
            return;

        preloadedScenes.Add(new PreloadedScene()
        {
            scene = scene,
            useScene = false,
            canceled = false
        });

        StartCoroutine(PreloadSceneCo(scene, MustUsePreloadedScene, CancelPreload, IsCanceled, RemovePreloadedScene, GetPreloadedScenesCount));
    }

    public void UsePreloadedScene(string sceneName)
    {
        preloadedScenes.Find(pScene => pScene.scene.name == sceneName).useScene = true;
    }

    public void AdditiveLoadScene(string sceneName, Action resultCallback = null)
    {
        LoadScene(sceneName, LoadSceneMode.Additive, resultCallback);
    }

    public void LoadScene(string sceneName, Action resultCallback = null)
    {
        LoadScene(sceneName, LoadSceneMode.Single, resultCallback);
    }

    public void LoadScene(string sceneName, LoadSceneMode loadmode, Action resultCallback = null)
    {
        StartCoroutine(LoadSceneCo(sceneName, loadmode, resultCallback));
    }

    public void ClearScenes()
    {
        if (SceneManager.sceneCount > 1)
        {
            Scene[] scenes = SceneManager.GetAllScenes();

            for (int i = 1; i < scenes.Length; i++)
            {
                SceneManager.UnloadScene(scenes[i]);
            }
        }
    }

    public void SetCurrentScene(string sceneName)
    {
        _currentScene = sceneName;
    }

    #endregion

    #region CALLBACKS

    bool IsCanceled(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.scene.name == sceneName).canceled;
    }

    bool MustUsePreloadedScene(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.scene.name == sceneName).useScene;
    }

    int GetPreloadedScenesCount()
    {
        return preloadedScenes.Count;
    }

    void RemovePreloadedScene(string sceneName)
    {
        preloadedScenes.Remove(preloadedScenes.Find(pScene => pScene.scene.name == sceneName));
    }

    void CancelPreload(string[] scenesNeedingPreloadNames)
    {
        foreach (PreloadedScene pScene in preloadedScenes)
        {
            if (!scenesNeedingPreloadNames.Contains(pScene.scene.name))
            {
                pScene.canceled = true;
            }
        }
    }

    #endregion

    #region PRIVATE PROCESS

    void DisableGameObjects(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }
    }

    void DisableGameObjects(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            obj.SetActive(false);
        }
    }

    #endregion

}
