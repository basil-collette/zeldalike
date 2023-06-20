using System;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public TargetScene targetScene;

    public static event Action exitPause;

    public void StartGame()
    {
        FindGameObjectHelper.FindByName("Screen UI Canva").SetActive(true);
        FindGameObjectHelper.FindByName("Screen Controls Canva").SetActive(true);

        FindAnyObjectByType<ScenesManager>().SwitchScene(targetScene);
    }

    public void Resume()
    {
        exitPause?.Invoke();
        FindAnyObjectByType<PauseManager>().Resume();
    }

    public void ShowPausedInterface(string sceneName)
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface(sceneName);
    }

}
