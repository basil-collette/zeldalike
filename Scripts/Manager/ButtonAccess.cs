using System;
using UnityEngine;

public class ButtonAccess : MonoBehaviour
{
    public TargetScene targetScene;

    public static event Action exitPause;

    public void StartGame()
    {
        FindGameObjectHelper.FindByName("Screen UI Canva").active = true;
        FindGameObjectHelper.FindByName("Screen Controls Canva").active = true;

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
