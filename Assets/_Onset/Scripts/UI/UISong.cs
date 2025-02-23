using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UISong : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI songTitle;

    [SerializeField]
    private TextMeshProUGUI songAuthor;

    [SerializeField]
    private Toggle toggle;
    public Toggle Toggle => toggle;

    public AudioSource AudioSource { get; set; }
    private AudioClipData audioClipData;
    private AudioClip audioClip;

    public event Action<AudioClipData> OnIsSelected;

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

    public void UpdateSongTitle(AudioClipData audioClipData)
    {
        this.audioClipData = audioClipData;
        songTitle.text = this.audioClipData.SongTitle;
        songAuthor.text = this.audioClipData.Author;
    }

    public void StopMusic()
    {
        if(audioClip != null && AudioSource.clip == audioClip)
            AudioSource.Stop();
    }

    public void PlayMusic()
    {
        if(audioClip == null)
            audioClip = Resources.Load<AudioClip>($"Songs/{SaveSongFolderURL.GetFileName(audioClipData.Author, audioClipData.SongTitle)}");

        AudioSource.clip = audioClip;
        AudioSource.Play();
    }
}
