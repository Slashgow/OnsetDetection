using TMPro;
using UnityEngine;

public class UIMenuSongLoaderCampaign : UIMenuSongLoader
{
    [SerializeField]
    private TextMeshProUGUI totalScoreText;

    protected override void Start()
    {
        base.Start();
        CampaignManager_OnUpdateTotalScore(CampaignManager.Instance.TotalScore);
        CampaignManager.Instance.OnUnlockSong += CampaignManager_OnUnlockSong;
        CampaignManager.Instance.OnUpdateTotalScore += CampaignManager_OnUpdateTotalScore;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CampaignManager.Instance.OnUnlockSong -= CampaignManager_OnUnlockSong;
        CampaignManager.Instance.OnUpdateTotalScore -= CampaignManager_OnUpdateTotalScore;
    }

    private void CampaignManager_OnUpdateTotalScore(int totalScore) => totalScoreText.text = totalScore.ToString();

    private void CampaignManager_OnUnlockSong() => LoadUI();

    public override void LoadUI()
    {
        Utility.DestroyAllChidren(content.transform);

        foreach (AudioClipDataCampaign audioClipDataCampaign in CampaignManager.Instance.AudioClipDataCampaigns)
        {
            GameObject uiSongGroupGameObjectInstance = Instantiate(UISongGroupPrefab, content.transform);
            UISongCampaign uiSongCampaign = uiSongGroupGameObjectInstance.GetComponent<UISongCampaign>();
            uiSongCampaign.UpdateSongInfo(audioClipDataCampaign, CampaignManager.Instance.GetRemainingScoreToUnlockSong(audioClipDataCampaign.TotalScoreToUnlock));
            uiSongCampaign.AudioSource = audioSource;

            uiSongCampaign.OnIsSelected -= UiSong_OnIsSelected;
            uiSongCampaign.OnIsSelected += UiSong_OnIsSelected;

            toggleGroupController.Toggles.Add(uiSongCampaign.Toggle);

            if (audioClipDataCampaign.IsUnlocked)
                uiSongCampaign.Toggle.interactable = true;
            else 
                uiSongCampaign.Toggle.interactable = false;
        }

        toggleGroupController.RegisterValueChanged();
    }
}
