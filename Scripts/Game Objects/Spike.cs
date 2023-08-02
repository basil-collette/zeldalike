using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float closeDuration = 0.5f;
    public float readyDuration = 1f;
    public float openDuration = 2f;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(CycleCo());
    }

    IEnumerator CycleCo()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(closeDuration);
            anim.SetTrigger("ready");
            yield return new WaitForSecondsRealtime(readyDuration);
            anim.SetTrigger("open");
            yield return new WaitForSecondsRealtime(openDuration);
            anim.SetTrigger("close");
        }
    }

}
