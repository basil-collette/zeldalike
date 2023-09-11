using System;
using System.Collections;
using UnityEngine;

public class MemoryPartsCarroussel : MonoBehaviour
{

    public RectTransform parts;
    public int sizeParts;
    public float movementSpeed = 20;

    Inventory inv;
    int index;
    bool moving;

    void Start()
    {
        index = 0;
        moving = false;

        inv = FindGameObjectHelper.FindByName("Player").GetComponent<Player>().inventory;

        ShowMemoryPart("Identity");
        ShowMemoryPart("Diplomes");
        ShowMemoryPart("Competences");
    }

    public void ShowMemoryPart(string partName)
    {
        switch (partName)
        {
            case "Competences":
                ShowMemoryPart(partName, inv.Items.Exists(x => x.NameCode == "competences")); break;
            case "Diplomes":
                ShowMemoryPart(partName, inv.Items.Exists(x => x.NameCode == "diplomes")); break;
            case "Identity":
            default:
                ShowMemoryPart(partName, inv.Items.Exists(x => x.NameCode == "id_card")); break;
        }
    }

    public void ShowMemoryPart(string partName, bool show)
    {
        switch (partName)
        {
            case "Competences":
                transform.Find("Competences").Find("Content").gameObject.SetActive(show); break;
            case "Diplomes":
                transform.Find("Diplomes").Find("Content").gameObject.SetActive(show); break;
            case "Identity":
            default:
                transform.Find("Identity").Find("Content").gameObject.SetActive(show); break;
        }
    }

    public void Left()
    {
        if (!moving && index > 0)
        {
            StartCoroutine(MoveCo(parts.anchoredPosition.x + sizeParts));
            index--;
        }
    }

    public void Right()
    {
        if (!moving && index < (parts.childCount - 1))
        {
            StartCoroutine(MoveCo(parts.anchoredPosition.x - sizeParts));
            index++;
        }
    }

    IEnumerator MoveCo(float targetPosX)
    {
        moving = true;

        if (targetPosX < parts.anchoredPosition.x)
        {
            while (targetPosX < parts.anchoredPosition.x)
            {
                float difference = Math.Abs(parts.anchoredPosition.x - targetPosX);
                float finalMovementSpeed = (difference < movementSpeed) ? difference : movementSpeed;

                parts.anchoredPosition = parts.anchoredPosition + new Vector2(-finalMovementSpeed, 0);

                yield return null;
            }
        }
        else
        {
            while (targetPosX > parts.anchoredPosition.x)
            {
                float difference = Math.Abs(parts.anchoredPosition.x - targetPosX);
                float finalMovementSpeed = (difference < movementSpeed) ? difference : movementSpeed;

                parts.anchoredPosition = parts.anchoredPosition + new Vector2(finalMovementSpeed, 0);

                yield return null;
            }
        }        

        moving = false;
    }

}
