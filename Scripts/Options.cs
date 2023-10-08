using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] GameObject musicOnButton;
    [SerializeField] GameObject musicOffButton;

    [SerializeField] Slider soundSlider;
    [SerializeField] GameObject soundOnButton;
    [SerializeField] GameObject soundOffButton;

    SoundManager _soundManager;

    void Awake()
    {
        _soundManager = FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>();

        musicSlider.value = _soundManager.musicSource.volume;
        soundSlider.value = _soundManager.soundSource.volume;

        SetMuteButtons();
    }

    void SetMuteButtons()
    {
        musicOnButton.SetActive(!_soundManager.musicSource.mute);
        musicOffButton.SetActive(_soundManager.musicSource.mute);

        soundOnButton.SetActive(!_soundManager.soundSource.mute);
        soundOffButton.SetActive(_soundManager.soundSource.mute);
    }

    public void SetMusicVolume()
    {
        _soundManager.SetMusicVolume(musicSlider.value);
    }

    public void SetSoundVolume()
    {
        _soundManager.SetSoundVolume(soundSlider.value);
    }

    public void TogglePlayMusic(bool mute)
    {
        _soundManager.SetMutePlayMusic(mute);
        SetMuteButtons();
    }

    public void TogglePlaySounds(bool mute)
    {
        _soundManager.SetMutePlaySounds(mute);
        SetMuteButtons();
    }

    public void SaveGame()
    {
        FindAnyObjectByType<SaveManager>().SaveGame();
    }

    public void EraseSaveGame()
    {
        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<SaveManager>().EraseSave();
    }

}
