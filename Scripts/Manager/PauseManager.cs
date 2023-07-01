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
    public GameObject MenuOverlay;

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

                MenuOverlay.SetActive(false);

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
            MenuOverlay.SetActive(loadedSceneName != "DialogueScene");

            SoundManager soundManager = GetComponentInChildren<SoundManager>();
            soundManager.musicSource.Stop();
            soundManager.PlayEffect("pause_enter");
        }
        else
        {
            transparentOverlay.SetActive(true);
            controlsCanva.SetActive(false);

            SoundManager soundManager = GetComponentInChildren<SoundManager>();
            soundManager.musicSource.Stop();
            //soundManager.PlayEffect("pause_enter"); play info sound ?
        }
    }

    public void ShowPausedInterface(string interfaceName)
    {
        ShowPausedInterface(interfaceName, null);
    }
    public void ShowPausedInterface(string interfaceName, Action OnPauseProcessed, bool transparentOverlay = false)
    {
        if (interfaceName == loadedSceneName) return;

        loadedSceneName = interfaceName;

        StartCoroutine(SwitchPausedSceneCo(() => {
            Pause(transparentOverlay);
            OnPauseProcessed?.Invoke();
        }));
    }

    public void SwitchPausedInterface(string interfaceName)
    {
        if (interfaceName == loadedSceneName) return;

        loadedSceneName = interfaceName;

        StartCoroutine(SwitchPausedSceneCo());
    }

    public bool GetIsPaused()
    {
        return IsPaused;
    }

    IEnumerator SwitchPausedSceneCo(Action resultCallback = null)
    {
        Scene[] scenes = SceneManager.GetAllScenes();
        if (scenes.Length > 2)
        {
            for (int i = 2; i < scenes.Length; i++)
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scenes[i]);
                while (!unloadOp.isDone)
                {
                    yield return null;
                }
            }
        }

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
