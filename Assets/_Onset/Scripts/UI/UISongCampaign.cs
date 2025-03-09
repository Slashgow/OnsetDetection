using TMPro;
using UnityEngine;

public class UISongCampaign : UISong
{
    [SerializeField]
    private TextMeshProUGUI scoreToUnlockText;

    public void UpdateSongInfo(AudioClipDataCampaign audioClipDataCampaign, int remainingScoreToUnlock)
    {
        base.UpdateSongInfo(audioClipDataCampaign);

        if(audioClipDataCampaign.IsUnlocked)
            scoreToUnlockText.enabled = false;
        else
            scoreToUnlockText.text = $"need {remainingScoreToUnlock.ToString()} to unlock";
    }
}
