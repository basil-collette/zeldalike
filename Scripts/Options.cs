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

    void Awake()
    {
        musicSlider.value = MainGameManager._soundManager.musicSource.volume;
        soundSlider.value = MainGameManager._soundManager.soundSource.volume;

        SetMuteButtons();
    }

    void SetMuteButtons()
    {
        musicOnButton.SetActive(!MainGameManager._soundManager.musicSource.mute);
        musicOffButton.SetActive(MainGameManager._soundManager.musicSource.mute);

        soundOnButton.SetActive(!MainGameManager._soundManager.soundSource.mute);
        soundOffButton.SetActive(MainGameManager._soundManager.soundSource.mute);
    }

    public void SetMusicVolume()
    {
        MainGameManager._soundManager.SetMusicVolume(musicSlider.value);
    }

    public void SetSoundVolume()
    {
        MainGameManager._soundManager.SetSoundVolume(soundSlider.value);
    }

    public void TogglePlayMusic(bool mute)
    {
        MainGameManager._soundManager.SetMutePlayMusic(mute);
        SetMuteButtons();
    }

    public void TogglePlaySounds(bool mute)
    {
        MainGameManager._soundManager.SetMutePlaySounds(mute);
        SetMuteButtons();
    }

    public void SaveGame()
    {
        FindAnyObjectByType<SaveManager>().SaveGame();
    }

    public void EraseSaveGame()
    {
        MainGameManager._saveManager.EraseSave();
    }

}
