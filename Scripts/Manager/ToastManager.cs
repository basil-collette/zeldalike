using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : SingletonGameObject<ToastManager>
{
    public Animator anim;
    public Image ToastBackground;
    public Image ToastLogo;
    public Text ToastTitle;
    public Text ToastText;

    public float VisibleDuration = 5;
    public ToastPositionType PositionType = ToastPositionType.TopMiddle;
    public bool OpacityAppear = false;
    public bool OpacityDisappear = false;

    List<Toast> Toasts = new List<Toast>();
    bool InProcess = false;

    public void Add(Toast toast)
    {
        Toasts.Add(toast);

        if (!InProcess) StartCoroutine(ShowToastCo());
    }

    IEnumerator ShowToastCo()
    {
        InProcess = true;

        Toast currentToast = Toasts[0];
        Toasts.RemoveAt(0);

        ToastText.text = currentToast.Text;

        Show(true);

        yield return new WaitForSecondsRealtime(VisibleDuration);

        Show(false);

        InProcess = Toasts.Count > 0;

        if (InProcess) StartCoroutine(ShowToastCo());
    }

    void Show(bool show)
    {
        if (show && OpacityAppear)
        {
            StartCoroutine(DisappearCo()); //ShowOpacity(true);
            return;
        }

        if (!show && OpacityDisappear)
        {
            StartCoroutine(DisappearCo()); //ShowOpacity(false);
            return;
        }

        switch (PositionType)
        {
            case ToastPositionType.TopLeft: ShowTopLeft(show);  break;
            case ToastPositionType.TopMiddle: ShowTopMiddle(show); break;
            case ToastPositionType.TopRight: ShowTopRight(show); break;

            case ToastPositionType.MiddleMiddle: ShowMiddleMiddle(show); break;

            case ToastPositionType.BottomLeft: ShowBottomLeft(show); break;
            case ToastPositionType.BottomMiddle: ShowBottomMiddle(show); break;
            case ToastPositionType.BottomRight: ShowBottomRight(show); break;

            default: ShowTopMiddle(show); break;
        }
    }

    void ShowOpacity(bool show)
    {
        if (show)
            anim.SetTrigger("appear");
        else
            anim.SetTrigger("disappear");
    }

    void ShowTopLeft(bool show)
    {
        if (show)
            anim.SetTrigger("topleft");
        else
            anim.SetTrigger("topleft_exit");
    }

    void ShowTopMiddle(bool show)
    {
        if (show)
            anim.SetTrigger("topmiddle");
        else
            anim.SetTrigger("topmiddle_exit");
    }

    void ShowTopRight(bool show)
    {

    }

    void ShowMiddleMiddle(bool show)
    {

    }

    void ShowBottomLeft(bool show)
    {

    }

    void ShowBottomMiddle(bool show)
    {

    }

    void ShowBottomRight(bool show)
    {

    }

    IEnumerator DisappearCo()
    {
        for (float f = 0f; f <= 4; f += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            Color colorTemp = ToastBackground.color;
            colorTemp.a -= 0.035f;
            ToastBackground.color = colorTemp;
        }
        yield return null;
    }
}

public class Toast
{
    public string Text { get; set; }
    public ToastType Type { get; set; }

    public Toast(string text, ToastType type)
    {
        Text = text;
        Type = type;
    }
}