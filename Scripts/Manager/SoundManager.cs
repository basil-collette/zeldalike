using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    [System.Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
    }

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] public AudioSource musicSource;
        [SerializeField] public float musicVolume = 1;
        [SerializeField] public Sound[] musics;

        [SerializeField] public AudioSource effectSource;
        [SerializeField] public float effectVolume = 1;
        [SerializeField] public Sound[] effects;

        private void Start()
        {

        }

        public void OnSceneSwitchSetMusic(TargetScene scene)
        {
            musicSource.Stop();

            if (scene.musicName != null && scene.musicName != string.Empty)
            {
                musicSource.clip = musics.First(music => music.Name == scene.musicName).Clip;
                musicSource.Play();
            }
        }

        public void PlayMusic(string Name)
        {
            musicSource.Stop();
            musicSource.clip = musics.First(music => music.Name == Name).Clip;
            musicSource.Play();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void PlayEffect(string Name)
        {
            effectSource.PlayOneShot(effects.First(e => e.Name == Name).Clip);
        }

        public void SetSoundsVolume(float volume)
        {
            effectVolume = volume;

            foreach (var obj in FindObjectsOfType<AudioSource>())
            {
                if (obj.name == "MusicSource")
                    break;

                obj.GetComponent<AudioSource>().volume = volume;
            }
        }

        public void TogglePlayMusic()
        {
            musicSource.mute = !musicSource.mute;
        }

        public void TogglePlaySounds(bool play)
        {
            effectSource.mute = !effectSource.mute;
        }

        /*
        public void StartRepetitiveSound(AudioClip clip)
        {
            repetitivesSounds.Add(clip);
            StartCoroutine(RepetitiveSoundCo(clip));
        }

        IEnumerator RepetitiveSoundCo(string clipname)
        {
            var sound = effects.FirstOrDefault(c => c.Name == clipname);
            while (sound != null)
            {
                effectSource.PlayOneShot(sound.Clip);

                sound = effects.FirstOrDefault(c => c.Name == clipname);
                yield return null;
            }
        }
        */

    }
}
