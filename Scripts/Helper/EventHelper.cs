using System;
using UnityEngine;

public class EventHelper : MonoBehaviour
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

    public void StartMenuShowOptions()
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface(new PauseParameter() { InterfaceName = "StartOptionsScene", TransparentOverlay = true });
    }

    public void Resume()
    {
        exitPause?.Invoke();
        FindAnyObjectByType<PauseManager>().Resume();
    }

    public void ShowPausedInterface(string sceneName)
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface(new PauseParameter() { InterfaceName = sceneName, ShowMenu = true });
    }

    public void AddQuest(string questName)
    {
        MainGameManager._questManager.AddQuest(questName);
    }

    public void UnlockMemoryPartIdCard()
    {
        FindGameObjectHelper.FindByName("Parts").GetComponent<MemoryPartsCarroussel>().ShowMemoryPart("Identity", true);
    }

    public void UnlockMemoryPartDiplomas()
    {
        FindGameObjectHelper.FindByName("Parts").GetComponent<MemoryPartsCarroussel>().ShowMemoryPart("Diplomes", true);
    }

    public void UnlockMemoryPartCompetences()
    {
        FindGameObjectHelper.FindByName("Parts").GetComponent<MemoryPartsCarroussel>().ShowMemoryPart("Competences", true);
    }

}
