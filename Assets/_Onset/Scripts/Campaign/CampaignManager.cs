using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class CampaignManager : PersistentMonoSingleton<CampaignManager>
{
    public int TotalScore { get; private set; }

    private List<AudioClipDataCampaign> audioClipDataCampaigns = new List<AudioClipDataCampaign>();
    public List<AudioClipDataCampaign> AudioClipDataCampaigns => audioClipDataCampaigns;

    public event Action OnUnlockSong;
    public event Action<int> OnUpdateTotalScore;

    protected override void Awake()
    {
        base.Awake();

        LoadCampaignSongsFromJson();
        GameManager_OnTotalScoreUpdated();
    }

    private void Start()
    {
        GameManager.Instance.OnTotalScoreUpdated += GameManager_OnTotalScoreUpdated;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnTotalScoreUpdated -= GameManager_OnTotalScoreUpdated;
    }

    private void GameManager_OnTotalScoreUpdated()
    {
        UpdateTotalScore();
        TryUnlockSong();
    }

    public void LoadCampaignSongsFromJson()
    {
        List<string> filesPathsCreditsCampaign = Directory.GetFiles(SaveDataPaths.SongMapsCampaignAbsoluteFolderPath, "*_credit.json").ToList();

        foreach (string filePath in filesPathsCreditsCampaign)
        {
            string audioClipDataText = System.IO.File.ReadAllText(filePath);
            AudioClipDataCampaign audioClipDataCampaign = JsonUtility.FromJson<AudioClipDataCampaign>(audioClipDataText);
            audioClipDataCampaign.Unlock(TotalScore);
            audioClipDataCampaigns.Add(audioClipDataCampaign);
        }
    }

    private void UpdateTotalScore()
    {
        TotalScore = 0;
        foreach (AudioClipDataCampaign audioClipDataCampaign in audioClipDataCampaigns)
        {
            TotalScore += SaveDataPaths.GetHighScore(audioClipDataCampaign);
        }
        OnUpdateTotalScore?.Invoke(TotalScore);
    }

    private void TryUnlockSong()
    {
        foreach (AudioClipDataCampaign audioClipDataCampaign in audioClipDataCampaigns)
        {
            if (!audioClipDataCampaign.IsUnlocked)
            {
                audioClipDataCampaign.Unlock(TotalScore);
                if (audioClipDataCampaign.IsUnlocked)
                    OnUnlockSong?.Invoke();
            }
        }
    }

    public int GetRemainingScoreToUnlockSong(int totalScoreToUnlock) => totalScoreToUnlock - TotalScore;
}
