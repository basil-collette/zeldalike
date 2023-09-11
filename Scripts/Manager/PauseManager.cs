using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : SignletonGameObject<PauseManager>
{
    static bool IsPaused = false;

    public GameObject controlsCanva;
    public GameObject overlay;
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

                overlay.SetActive(false);
                //controlsCanva.SetActive(true);

                SoundManager soundManager = GetComponentInChildren<SoundManager>();
                soundManager.PlayEffect("pause_exit");
                soundManager.musicSource.Play();

                MenuOverlay.SetActive(false);

                AfterResume?.Invoke();

                //Input.ResetInputAxes();
            };

            StartCoroutine(UnloadPauseSceneCo(OnUnloadPauseSceneEnd));
        }
    }

    void Pause(bool transparent = false, bool showMenu = false)
    {
        IsPaused = true;
        Time.timeScale = 0f;

        overlay.SetActive(true);
        overlay.GetComponent<Image>().color = (transparent) ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 0.7f);

        //controlsCanva.SetActive(false);

        MenuOverlay.SetActive(showMenu);

        SoundManager soundManager = GetComponentInChildren<SoundManager>();
        soundManager.musicSource.Stop();
        soundManager.PlayEffect("pause_enter");
    }

    public void ShowPausedInterface(string interfaceName)
    {
        ShowPausedInterface(interfaceName, null);
    }
    public void ShowPausedInterface(string interfaceName, Action OnPauseProcessed, bool transparentOverlay = false, bool showMenu = false)
    {
        if (interfaceName == loadedSceneName)
            return;

        loadedSceneName = interfaceName;

        StartCoroutine(SwitchPausedSceneCo(() => {
            Pause(transparentOverlay, showMenu);
            OnPauseProcessed?.Invoke();
        }));
    }

    public void SwitchPausedInterface(string interfaceName)
    {
        if (interfaceName == loadedSceneName)
            return;

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
