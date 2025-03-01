using System;
using UnityEngine;

[Serializable]
public class AudioClipData 
{
    [SerializeField]
    private string audioClipPath;
    public string AudioClipPath => audioClipPath;

    [SerializeField]
    private bool isCampaign;
    public bool IsCampaign => isCampaign;

    [SerializeField]
    private string songTitle;
    public string SongTitle => songTitle;

    [SerializeField]
    private string author;
    public string Author => author;

    [SerializeField]
    private string extension;
    public string Extension => extension;

    public AudioClipData(string songTitle, string author, string extension, string audioClipPath, bool isCampaign)
    {
        this.songTitle = songTitle;
        this.author = author;
        this.extension = extension;
        this.audioClipPath = audioClipPath;
        this.isCampaign = isCampaign;
    }
}
