using System;
using UnityEngine;

[Serializable]
public class AudioClipData 
{
    [SerializeField]
    private string songTitle;
    public string SongTitle => songTitle;

    [SerializeField]
    private string author;
    public string Author => author;

    [SerializeField]
    private string extension;
    public string Extension => extension;

    public AudioClipData(string songTitle, string author, string extension)
    {
        this.songTitle = songTitle;
        this.author = author;
        this.extension = extension;
    }
}
