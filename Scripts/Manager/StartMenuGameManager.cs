using UnityEngine;

public class StartMenuGameManager : MonoBehaviour
{
    public TargetScene targetScene;

    public void StartGame()
    {
        FindGameObjectHelper.FindInactiveObjectByName("Screen UI Canva").active = true;
        FindGameObjectHelper.FindInactiveObjectByName("Screen Controls Canva").active = true;

        FindAnyObjectByType<ScenesManager>().SwitchScene(targetScene);
    }

}
