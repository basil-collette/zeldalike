using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedAudioSource : MonoBehaviour
{
    AudioSource audioSource;
    List<AudioClip> repetitivesSounds = new List<AudioClip>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = FindObjectOfType<SoundManager>().soundVolume;
    }

    public void StartRepetitiveSound(AudioClip clip)
    {
        repetitivesSounds.Add(clip);
        StartCoroutine(RepetitiveSoundCo(clip));
    }

    public void StopRepetitiveSound(AudioClip clip)
    {
        repetitivesSounds.Remove(clip);
    }

    IEnumerator RepetitiveSoundCo(AudioClip clip)
    {
        while (repetitivesSounds.Find(c => c == clip))
        {
            audioSource.clip = clip;
            audioSource.Play();

            yield return null;
        }
    }

}
