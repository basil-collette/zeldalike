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

    public class SoundManager : SingletonGameObject<SoundManager>
    {
        [SerializeField] public AudioSource musicSource;
        public float MusicVolume => musicSource.volume;
        [SerializeField] Sound[] musics;

        [SerializeField] public AudioSource soundSource;
        public float SoundVolume => soundSource.volume;
        [SerializeField] Sound[] sounds;

        private void Start()
        {
            Init();
            Load();
        }

        public void OnSceneSwitchSetMusic(string musicName)
        {
            musicSource.Stop();

            if (musicName != null && musicName != string.Empty)
            {
                musicSource.clip = musics.First(music => music.Name == musicName).Clip;
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
            Save();
        }

        public void SetSoundVolume(float volume)
        {
            soundSource.volume = volume;
            Save();
        }

        public void PlayEffect(string Name)
        {
            soundSource.PlayOneShot(sounds.First(e => e.Name == Name).Clip);
        }

        public void PlayEffect(string Name, float currentVolume)
        {
            soundSource.PlayOneShot(sounds.First(e => e.Name == Name).Clip, currentVolume);
        }

        public void PlayEffect(AudioClip clip)
        {
            soundSource.PlayOneShot(clip);
        }

        public void StopEffect()
        {
            soundSource.Stop();
        }

        /*
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
        */

        public void SetMutePlayMusic(bool mute)
        {
            musicSource.mute = mute;
            Save();
        }

        public void SetMutePlaySounds(bool mute)
        {
            soundSource.mute = mute;
            Save();
        }

        public void Init()
        {
            if (!PlayerPrefs.HasKey("musicVolume"))
                PlayerPrefs.SetFloat("musicVolume", 1);
            if (!PlayerPrefs.HasKey("musicMute"))
                PlayerPrefs.SetString("musicMute", false.ToString());

            if (!PlayerPrefs.HasKey("soundVolume"))
                PlayerPrefs.SetFloat("soundVolume", 1);
            if (!PlayerPrefs.HasKey("soundMute"))
                PlayerPrefs.SetString("soundMute", false.ToString());
        }

        public void Load()
        {
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
            musicSource.mute = bool.Parse(PlayerPrefs.GetString("musicMute"));

            soundSource.volume = PlayerPrefs.GetFloat("soundVolume");
            soundSource.mute = bool.Parse(PlayerPrefs.GetString("soundMute"));
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("musicVolume", musicSource.volume);
            PlayerPrefs.SetString("musicMute", musicSource.mute.ToString());
            
            PlayerPrefs.SetFloat("soundVolume", soundSource.volume);
            PlayerPrefs.SetString("soundMute", soundSource.mute.ToString());
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
