using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : SignletonGameObject<PauseManager>
{
    static bool IsPaused = false;

    public GameObject controlsCanva;
    public GameObject blackOverlay;
    public GameObject transparentOverlay;

    string loadedSceneName;

    /*
    void Update()
    {
        if (Input.) //key escape or gamepad select
        {
            if (IsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    */

    public void Resume()
    {
        Resume(null);
    }

    public void Resume(Action AfterResume = null)
    {
        if (IsPaused)
        {
            Action OnUnloadPauseSceneEnd = () => {
                loadedSceneName = null;

                IsPaused = false;
                Time.timeScale = 1f;

                blackOverlay.SetActive(false);
                transparentOverlay.SetActive(false);
                controlsCanva.SetActive(true);

                SoundManager soundManager = GetComponentInChildren<SoundManager>();
                soundManager.PlayEffect("pause_exit");
                soundManager.musicSource.Play();

                AfterResume?.Invoke();
            };

            StartCoroutine(UnloadPauseSceneCo(OnUnloadPauseSceneEnd));
        }
    }

    void Pause(bool transparent = false)
    {
        IsPaused = true;
        Time.timeScale = 0f;

        if (transparent == false)
        {
            blackOverlay.SetActive(true);
        }
        else
        {
            transparentOverlay.SetActive(true);
            controlsCanva.SetActive(false);
        }

        SoundManager soundManager = GetComponentInChildren<SoundManager>();
        soundManager.musicSource.Stop();
        soundManager.PlayEffect("pause_enter");
    }
    public void ShowPausedInterface(string sceneName, bool transparentOverlay, Action OnPauseProcessed = null)
    {
        loadedSceneName = sceneName;

        StartCoroutine(LoadSceneCo(() => { Pause(transparentOverlay); OnPauseProcessed?.Invoke(); }));
    }

    public void ShowPausedInterface(string interfaceName)
    {
        ShowPausedInterface(interfaceName);
    }

    public void ShowPausedInterface(string interfaceName, Action OnPauseProcessed = null)
    {
        loadedSceneName = interfaceName;

        StartCoroutine(LoadSceneCo(() => { Pause(); OnPauseProcessed?.Invoke(); }));
    }

    public bool GetIsPaused()
    {
        return IsPaused;
    }

    IEnumerator LoadSceneCo(Action resultCallback = null)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(loadedSceneName, LoadSceneMode.Additive);

        while (!loadOp.isDone)
        {
            yield return null;
        }

        yield return new WaitForFixedUpdate();
        resultCallback?.Invoke();
    }

    IEnumerator UnloadPauseSceneCo(Action OnUnloadPauseSceneEnd)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(loadedSceneName);

        while (!unloadOp.isDone)
        {
            yield return null;
        }

        OnUnloadPauseSceneEnd?.Invoke();
    }

}
