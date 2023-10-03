using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsManager : SingletonGameObject<ActionButtonsManager>
{
    [SerializeField] GameObject ActionButtonsContainer;
    [SerializeField] GameObject PrefabButton;

    public GameObject AddButton(Sprite sprite, Action OnClick)
    {
        GameObject button = Instantiate(PrefabButton, ActionButtonsContainer.transform);
        button.GetComponent<Button>().onClick.AddListener(delegate () { OnClick?.Invoke(); });

        button.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

        return button;
    }

}
