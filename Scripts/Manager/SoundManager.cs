using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class SoundManager : SingletonGameObject<SoundManager>
    {
        [SerializeField] public AudioSource musicSource;
        public float MusicVolume => musicSource.volume;
        [SerializeField] Sound[] musics;

        [SerializeField] public AudioSource soundSource;
        public float SoundVolume => soundSource.volume;
        [SerializeField] Sound[] sounds;

        [SerializeField] public AudioSource dialogSource;

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

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
            Save();
        }

        public void SetSoundVolume(float volume)
        {
            soundSource.volume = volume;
            dialogSource.volume = volume;
            Save();
        }

        public void PlayEffect(string name)
        {
            if (soundSource.mute == true) return;
            soundSource.PlayOneShot(sounds.First(e => e.Name == name).Clip);
        }
        public void PlayEffect(string name, float volume)
        {
            if (soundSource.mute == true) return;

            soundSource.PlayOneShot(sounds.First(e => e.Name == name).Clip, soundSource.volume * volume);
        }

        public void PlayEffect(AudioClip clip)
        {
            if (soundSource.mute == true) return;
            soundSource.PlayOneShot(clip);
        }
        public void PlayEffect(AudioClip clip, float volume)
        {
            if (soundSource.mute == true) return;

            soundSource.PlayOneShot(clip, soundSource.volume * volume);
        }

        public void PlaySoundIndependently(AudioClip clip, float pitch)
        {
            PlaySoundIndependently(clip, pitch, soundSource.volume);
        }
        public void PlaySoundIndependently(AudioClip clip, float pitch, float volume)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            
            audioSource.Play();
            Destroy(audioSource, clip.length);
        }

        public void StopEffect()
        {
            soundSource.Stop();
        }

        public void SetMutePlayMusic(bool mute)
        {
            musicSource.mute = mute;
            Save();
        }

        public void SetMutePlaySounds(bool mute)
        {
            soundSource.mute = mute;
            dialogSource.mute = mute;
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

            dialogSource.volume = soundSource.volume;
            dialogSource.mute = soundSource.mute;
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

    [System.Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
    }

}
