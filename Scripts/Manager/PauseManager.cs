using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : SingletonGameObject<PauseManager>
{
    static bool IsPaused = false;

    public GameObject controlsCanva;
    public GameObject overlay;
    public GameObject MenuOverlay;

    string loadedSceneName;
    bool playSound = true;

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

    public void Resume() { Resume(null); }

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

                if (playSound)
                {
                    SoundManager soundManager = GetComponentInChildren<SoundManager>();
                    soundManager.PlayEffect("pause_exit");
                    soundManager.musicSource.Play();
                }

                MenuOverlay.SetActive(false);

                AfterResume?.Invoke();

                //Input.ResetInputAxes();
            };

            StartCoroutine(UnloadPauseSceneCo(OnUnloadPauseSceneEnd));
        }
    }

    void Pause(PauseParameter pauseParameter)
    {
        IsPaused = true;
        Time.timeScale = 0f;

        overlay.SetActive(true);
        overlay.GetComponent<Image>().color = (pauseParameter.TransparentOverlay) ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 0.7f);

        //controlsCanva.SetActive(false);

        MenuOverlay.SetActive(pauseParameter.ShowMenu);

        if (pauseParameter.PlaySound)
        {
            SoundManager soundManager = GetComponentInChildren<SoundManager>();
            soundManager.musicSource.Stop();
            soundManager.PlayEffect("pause_enter");
        }        
    }

    public void ShowPausedInterface(string interfaceName)
    {
        ShowPausedInterface(new PauseParameter() { InterfaceName = interfaceName });
    }
    public void ShowPausedInterface(PauseParameter pauseParameter)
    {
        if (pauseParameter.InterfaceName == loadedSceneName)
            return;

        loadedSceneName = pauseParameter.InterfaceName;
        playSound = pauseParameter.PlaySound;

        StartCoroutine(SwitchPausedSceneCo(pauseParameter));
    }

    public bool GetIsPaused()
    {
        return IsPaused;
    }

    IEnumerator SwitchPausedSceneCo(PauseParameter pauseParameter)
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

        Pause(pauseParameter);

        pauseParameter.OnPauseProcessed?.Invoke();
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

public class PauseParameter
{
    public string InterfaceName { get; set; }
    public Action OnPauseProcessed { get; set; }
    public bool TransparentOverlay { get; set; } = false;
    public bool ShowMenu { get; set; } = false;
    public bool PlaySound { get; set; } = true;

    public PauseParameter() { }
}