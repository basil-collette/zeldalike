using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float closeDuration = 0.75f;
    public float readyDuration = 0.75f;
    public float openDuration = 2f;
    public float delay = 0f;

    AudioSource audioSource;
    Animator anim;
    BoxCollider2D collider;    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();

        StartCoroutine(CycleCo());
    }

    IEnumerator CycleCo()
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        while (true)
        {
            anim.SetBool("close", true);
            anim.SetBool("open", false);
            collider.enabled = false;
            yield return new WaitForSeconds(closeDuration);

            anim.SetBool("ready", true);
            anim.SetBool("close", false);
            yield return new WaitForSeconds(readyDuration);

            anim.SetBool("open", true);
            anim.SetBool("ready", false);
            audioSource.Play();
            collider.enabled = true;
            yield return new WaitForSeconds(openDuration);
        }
    }

}
