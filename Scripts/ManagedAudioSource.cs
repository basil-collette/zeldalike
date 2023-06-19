using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedAudioSource : MonoBehaviour
{
    List<AudioClip> repetitivesSounds = new List<AudioClip>();

    void Awake()
    {
        GetComponent<AudioSource>().volume = FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>().effectVolume;
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
        AudioSource audioSource = GetComponent<AudioSource>();
        var sound = repetitivesSounds.Find(c => c == clip);

        while (sound != null)
        {
            audioSource.PlayOneShot(sound);

            sound = repetitivesSounds.Find(c => c == clip);
            yield return null;
        }
    }

}
