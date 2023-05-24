using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    [System.Serializable]
    public class Music
    {
        public string Name;
        public AudioClip Clip;
    }

    public class SoundManager : SignletonGameObject<SoundManager>
    {
        public bool playMusic = true;
        public float musicVolume = 1;
        public bool playSounds = true;
        public float soundVolume = 1;

        public Music[] musics;

        private void Start()
        {

        }

        public void OnSceneSwitchSetMusic(TargetScene scene)
        {
            AudioSource audioSource = GetComponent<AudioSource>();

            audioSource.Stop();

            if (playMusic &&
                scene.musicName != null && scene.musicName != string.Empty)
            {
                audioSource.clip = musics.First(music => music.Name == scene.musicName).Clip;
                audioSource.Play();
            }
        }

        public void PlayMusic(string Name)
        {
            if (playMusic)
            {
                AudioSource audioSource = GetComponent<AudioSource>();

                audioSource.Stop();
                audioSource.clip = musics.First(music => music.Name == Name).Clip;
                audioSource.Play();
            }
        }

        public void SetMusicVolume(float volume)
        {
            GetComponent<AudioSource>().volume = volume;
        }

        public void SetSoundsVolume(float volume)
        {
            soundVolume = volume;

            foreach (var obj in FindObjectsOfType<AudioSource>())
            {
                if (obj == this)
                    break;

                obj.GetComponent<AudioSource>().volume = volume;
            }
        }

        public void SetPlayMusic(bool play)
        {
            playMusic = play;
        }

        public void SetPlaySounds(bool play)
        {
            playSounds = play;
        }

    }

}
