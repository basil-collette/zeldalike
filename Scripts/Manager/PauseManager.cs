using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : SignletonGameObject<PauseManager>
{
    static bool IsPaused = false;

    public GameObject overlay;

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
        if (IsPaused)
        {
            Action OnUnloadPauseSceneEnd = () => {
                loadedSceneName = null;

                IsPaused = false;
                Time.timeScale = 1f;

                overlay.SetActive(false);

                SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
                soundManager.PlayEffect("pause_exit");
                soundManager.musicSource.Play();
            };

            StartCoroutine(UnloadPauseSceneCo(OnUnloadPauseSceneEnd));
        }
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

    void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        overlay.SetActive(true);

        SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.musicSource.Stop();
        soundManager.PlayEffect("pause_enter");
    }

    public void ShowPausedInterface(string interfaceName)
    {
        Pause();
        loadedSceneName = interfaceName;

        ScenesManager scenesManager = GameObject.Find("ScenesManager").GetComponent<ScenesManager>();
        scenesManager.LoadScene(loadedSceneName, LoadSceneMode.Additive);
    }

    public bool GetIsPaused()
    {
        return IsPaused;
    }

}
