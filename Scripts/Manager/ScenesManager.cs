using Assets.Scripts.Helper;
using Assets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    GameObject textBox;
    Text placeText;

    string _currentScene = string.Empty;
    List<PreloadedScene> preloadedScenes = new List<PreloadedScene>();

    //To keep it in memory
    public VectorValue playerPositionStorage;
    public VectorValue playerDirectionStorage;

    void Start()
    {
        textBox = FindGameObjectHelper.FindInactiveObjectByName("PlaceName");
        placeText = textBox.GetComponentInChildren<Text>();

        //StartCoroutine(FadeTransitionCo(fadeToVisible));
    }

    IEnumerator FadeTransitionCo(GameObject fadePanel)
    {
        fadePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        fadePanel.SetActive(false);
    }

    #region PROCESS

    IEnumerator SwitchSceneCo(TargetScene targetScene)
    {
        foreach (PreloadedScene pScene in preloadedScenes)
        {
            if (!targetScene.scenesNeedingPreloadNames.Contains(pScene.scene.libelle))
            {
                pScene.canceled = true;
            }
        }

        //Verify that there are no preloded scene that aren't needed for next scene
        while (preloadedScenes.Select(pScene => pScene.scene.libelle).Any(a => !targetScene.scenesNeedingPreloadNames.Contains(a)))
        {
            yield return null;
        }

        Scene currentScene = SceneManager.GetSceneByName(_currentScene);

        DisableGameObjects(currentScene);

        //fade to white here

        yield return StartCoroutine(UnLoadSceneCo(currentScene));

        yield return StartCoroutine(LoadSceneCo(targetScene, LoadSceneMode.Additive));

        FindObjectOfType<SoundManager>().OnSceneSwitchSetMusic(targetScene);

        //Play ambiance sound ??

        _currentScene = targetScene.libelle;

        //fade to transparent here
    }

    IEnumerator LoadSceneCo(TargetScene targetScene, LoadSceneMode loadmode, Action resultCallback = null)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetScene.libelle, loadmode);

        while (!loadOp.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene.libelle));

        FindObjectOfType<SoundManager>().OnSceneSwitchSetMusic(targetScene);

        if (resultCallback != null) resultCallback();
    }

    IEnumerator PreloadSceneCo(TargetScene scene,
        Func<string, bool> MustUseSceneCallback,
        Action<string[]> CancelPreloadCallback,
        Func<string, bool> IsCanceledCallBack,
        Action<string> RemovePreloadedSceneCallback,
        Func<int> GetPreloadedScenesCountCallback)
    {
        AsyncOperation asyncLoadOp = SceneManager.LoadSceneAsync(scene.libelle, LoadSceneMode.Additive);
        asyncLoadOp.allowSceneActivation = false;

        while (!asyncLoadOp.isDone)
        {
            //Debug.Log(scene.name + " scene is loading progress: " + (asyncLoadOp.progress * 100) + "%");

            if (IsCanceledCallBack(scene.libelle))
            {
                asyncLoadOp.allowSceneActivation = true;

                yield return StartCoroutine(CancelPreloadCo(scene.libelle, RemovePreloadedSceneCallback));
            }
            else
            {
                if (asyncLoadOp.progress >= 0.9f)
                {
                    if (MustUseSceneCallback(scene.libelle))
                    {
                        CancelPreloadCallback(scene.scenesNeedingPreloadNames);

                        RemovePreloadedSceneCallback(scene.libelle);

                        DisableGameObjects(_currentScene);

                        asyncLoadOp.allowSceneActivation = true;

                        while (GetPreloadedScenesCountCallback() > 1)
                        {
                            yield return null;
                        }

                        FindObjectOfType<SoundManager>().OnSceneSwitchSetMusic(scene);

                        //fade to white here

                        Scene currentScene = SceneManager.GetSceneByName(_currentScene);

                        yield return StartCoroutine(UnLoadSceneCo(currentScene));

                        _currentScene = scene.libelle;

                        //fade to transparent here

                        yield return null;

                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.libelle));
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
            UsePreloadedScene(targetScene.libelle);
        }
        else
        {
            StartCoroutine(SwitchSceneCo(targetScene));
        }

        StartCoroutine(PlaceNameCo(targetScene.libelle));
    }

    public void PreloadScene(TargetScene scene)
    {
        //Check if allready in preloaded list
        if (preloadedScenes.Find(pScene => pScene.scene.libelle == scene.libelle) != null)
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
        preloadedScenes.Find(pScene => pScene.scene.libelle == sceneName).useScene = true;
    }

    public void AdditiveLoadScene(TargetScene targetScene, Action resultCallback = null)
    {
        LoadScene(targetScene, LoadSceneMode.Additive, resultCallback);
    }

    public void LoadScene(TargetScene targetScene, Action resultCallback = null)
    {
        LoadScene(targetScene, LoadSceneMode.Single, resultCallback);
    }

    public void LoadScene(TargetScene targetScene, LoadSceneMode loadmode, Action resultCallback = null)
    {
        StartCoroutine(LoadSceneCo(targetScene, loadmode, resultCallback));
    }

    public void ClearScenes()
    {
        
        /*
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
            SceneManager.GetSceneAt(i);
        }
        */

        //below: deprecated but up: not working

        Scene[] scenes = SceneManager.GetAllScenes();

        //Starting at 1, to skip first scene (GUI and scene managing stuff)
        for (int i = 1; i < scenes.Length; i++)
        {
            SceneManager.UnloadScene(scenes[i]);
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
        return preloadedScenes.Find(pScene => pScene.scene.libelle == sceneName).canceled;
    }

    bool MustUsePreloadedScene(string sceneName)
    {
        return preloadedScenes.Find(pScene => pScene.scene.libelle == sceneName).useScene;
    }

    int GetPreloadedScenesCount()
    {
        return preloadedScenes.Count;
    }

    void RemovePreloadedScene(string sceneName)
    {
        preloadedScenes.Remove(preloadedScenes.Find(pScene => pScene.scene.libelle == sceneName));
    }

    void CancelPreload(string[] scenesNeedingPreloadNames)
    {
        foreach (PreloadedScene pScene in preloadedScenes)
        {
            if (!scenesNeedingPreloadNames.Contains(pScene.scene.libelle))
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

    private IEnumerator PlaceNameCo(string sceneName)
    {
        placeText.text = TextHelper.Labelize(sceneName).Substring(0, sceneName.Length - 5); ;
        textBox.SetActive(true);

        yield return new WaitForSeconds(4f);
        textBox.SetActive(false);
    }

    #endregion

}
