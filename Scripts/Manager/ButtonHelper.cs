using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    public TargetScene targetScene;

    public static event Action exitPause;

    /*
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.unscaledTime > 0 && Time.timeScale == 0)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition))
            {
                //button.onClick.Invoke();
            }
        }
    }
    */

    public void StartGame()
    {
        FindGameObjectHelper.FindByName("Canva UI").SetActive(true);
        FindGameObjectHelper.FindByName("Canva Controls").SetActive(true);

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
