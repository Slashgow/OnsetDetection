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
}
