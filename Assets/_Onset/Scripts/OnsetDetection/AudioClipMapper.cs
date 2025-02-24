using System;
using UnityEngine;

[Serializable]
public class AudioClipMapper
{
    [SerializeField]
    private AudioClip audioclip;
    public AudioClip AudioClip => audioclip;

    [SerializeField]
    private AudioClipData audioclipData;
    public AudioClipData AudioClipData => audioclipData;

    public AudioClipMapper(AudioClip audioclip, AudioClipData audioclipData)
    {
        this.audioclip = audioclip;
        this.audioclipData = audioclipData;
    }
}
