using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISong : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI songTitle;

    [SerializeField]
    private TextMeshProUGUI songAuthor;

    [SerializeField]
    private TextMeshProUGUI highScore;

    [SerializeField]
    private Toggle toggle;
    public Toggle Toggle => toggle;

    public AudioSource AudioSource { get; set; }
    private AudioClipData audioClipData;
    public AudioClipData AudioClipData => audioClipData;
    private AudioClip audioClip;

    public event Action<AudioClipData> OnIsSelected;
    public event Action OnAudioClipPathNotValid;

    private void OnEnable() => toggle.onValueChanged.AddListener(ToggleMusic);
    private void OnDisable() => toggle.onValueChanged.RemoveListener(ToggleMusic);

    private void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            PlayMusic();
            OnIsSelected?.Invoke(audioClipData);
        }
        else
            StopMusic();
    }

    public void UpdateSongInfo(AudioClipData audioClipData)
    {
        this.audioClipData = audioClipData;
        songTitle.text = this.audioClipData.SongTitle;
        songAuthor.text = this.audioClipData.Author;
        highScore.text = SaveDataPaths.GetHighScore(audioClipData).ToString();

        if (!audioClipData.IsCampaign && !File.Exists(audioClipData.AudioClipPath))
        {
            Debug.Log("File does not exist");
            OnAudioClipPathNotValid?.Invoke();
            
        }
            
    }

    public void StopMusic()
    {
        if(audioClip != null && AudioSource.clip == audioClip)
            AudioSource.Stop();

        AudioClipImporter.Instance.OnEndImportAudioClip -= SongImporter_OnEndImportAudioClip;
    }

    public void PlayMusic()
    {
        Debug.Log(audioClipData.AudioClipPath);
        

        if(audioClip == null)
        {
            string audioClipPath = audioClipData.IsCampaign ?
                SaveDataPaths.GetAbsolutePathAudioClipCampaign(audioClipData) :
                audioClipData.AudioClipPath;

            StartCoroutine(AudioClipImporter.Instance.LoadAudioClip(audioClipPath));
            AudioClipImporter.Instance.OnEndImportAudioClip -= SongImporter_OnEndImportAudioClip;
            AudioClipImporter.Instance.OnEndImportAudioClip += SongImporter_OnEndImportAudioClip;
        }
        else
        {
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
    }

    private void SongImporter_OnEndImportAudioClip()
    {
        Debug.Log("on import audio clip end");
        audioClip = AudioClipImporter.Instance.AudioClip;
        AudioSource.clip = audioClip;
        AudioSource.Play();
    }

    public void UpdateAudioClipData(AudioClipData audioClipData)
    {
        this.audioClipData = audioClipData;
        OnIsSelected?.Invoke(audioClipData);
    }
}
