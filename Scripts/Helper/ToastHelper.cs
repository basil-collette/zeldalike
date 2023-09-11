using UnityEngine;

public class ToastHelper : MonoBehaviour
{
    public static Vector2 currentPos;

    public void TopLeftEnd()
    {
        RectTransform rec = GetComponent<RectTransform>();
        rec.anchorMin = new Vector2(0, 0.5f);
        rec.anchorMax = new Vector2(0, 0.5f);
        rec.anchoredPosition = new Vector2(550, 450);
    }

    public void TopMiddleEnd()
    {
        RectTransform rec = GetComponent<RectTransform>();
        rec.anchorMin = new Vector2(0.5f, 0.5f);
        rec.anchorMax = new Vector2(0.5f, 0.5f);
        rec.anchoredPosition = new Vector2(0, 450);
        currentPos = new Vector2(0, 450);

    }

    public void SetBeginningPos()
    {
        RectTransform rec = GetComponent<RectTransform>();
        rec.anchorMin = new Vector2(0.5f, 0.5f);
        rec.anchorMax = new Vector2(0.5f, 0.5f);
        rec.transform.position = currentPos;
    }

}
