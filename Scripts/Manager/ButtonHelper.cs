using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
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
        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<MainGameManager>().StartGame();
    }

    public void EraseSave()
    {
        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<SaveManager>().EraseSave();
    }

    public void Resume()
    {
        exitPause?.Invoke();
        FindAnyObjectByType<PauseManager>().Resume();
    }

    public void ShowPausedInterface(string sceneName)
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface(sceneName, null, false, true);
    }

    public void Save()
    {
        FindAnyObjectByType<SaveManager>().SaveGame();
    }

}
