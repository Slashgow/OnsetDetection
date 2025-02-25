using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    [SerializeField]
    private Slider mainVolumeSlider, musicSlider, sfxSlider;

    [SerializeField, Range(0.0001f, 1f)]
    private float defaultMainVolume, defaultMusicVolume, defaultSfXVolume;


    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        mainVolumeSlider.value = defaultMainVolume;
        musicSlider.value = defaultMusicVolume;
        sfxSlider.value = defaultSfXVolume;
        SetVolumeMaster(defaultMainVolume);
        SetVolumeMusic(defaultMusicVolume);
        SetVolumeSFX(defaultSfXVolume);
    }

    public void SetVolumeMaster(float sliderValue) => audioMixer.SetFloat("mainVolume", Mathf.Log10(sliderValue) * 20);
    public void SetVolumeSFX(float sliderValue) => audioMixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20);
    public void SetVolumeMusic(float sliderValue) => audioMixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
}
