using System;
using UnityEngine;

[Serializable]
public class AudioClipDataCampaign : AudioClipData, IUnlockable
{
    [SerializeField]
    private int totalScoreToUnlock;
    public int TotalScoreToUnlock => totalScoreToUnlock;

    [SerializeField]
    private bool isUnlocked;
    public bool IsUnlocked => isUnlocked;

    public AudioClipDataCampaign(string songTitle, string author, string extension, string audioClipPath, bool isCampaign) : base(songTitle, author, extension, audioClipPath, isCampaign)
    {
    }

    public void Unlock(int totalScore)
    {
        if(totalScore >= totalScoreToUnlock)
        {
            isUnlocked = true;
        }
    }
}
