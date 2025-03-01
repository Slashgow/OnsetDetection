using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class UIMenuSongLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject content;

    [SerializeField]
    private UISongToggleGroupController toggleGroupController;

    [SerializeField]
    private GameObject UISongGroupPrefab;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Button startButton;

    private List<string> fileNames =new List<string>();
    private List<AudioClipData> audioClipDatas = new List<AudioClipData>();

    private void Start()
    {
        audioSource.ignoreListenerPause = true;
        SetAllAudioClipData();
        LoadUI();
        startButton.onClick.AddListener(LoadScene);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        SceneLoader.Instance.LoadScene(1);
    }

    public void SetAllAudioClipData()
    {       
        //Debug.Log(Application.dataPath);
        //Debug.Log(SaveSongFolderURL.SaveSongAbsoluteFolderURL);

        List<string> filesPathsCreditsCampaign = Directory.GetFiles(SaveDataPaths.SongMapsCampaignAbsoluteFolderPath, "*_credit.json").ToList();
        filesPathsCreditsCampaign.ForEach(filePath => fileNames.Add(Path.GetFullPath(filePath)));

        foreach (string filePath in filesPathsCreditsCampaign)
        {
            string audioClipDataText = System.IO.File.ReadAllText(filePath);
            AudioClipData  audioClipData = JsonUtility.FromJson<AudioClipData>(audioClipDataText);
            audioClipDatas.Add(audioClipData);
        }

        List<string> filesPathsCreditsImported = Directory.GetFiles(SaveDataPaths.SongMapsImportedAbsoluteFolderPath, "*_credit.json").ToList();
        filesPathsCreditsCampaign.ForEach(filePath => fileNames.Add(Path.GetFullPath(filePath)));
        foreach (string filePath in filesPathsCreditsImported)
        {
            string audioClipDataText = System.IO.File.ReadAllText(filePath);
            AudioClipData audioClipData = JsonUtility.FromJson<AudioClipData>(audioClipDataText);

            if (File.Exists(audioClipData.AudioClipPath))
                audioClipDatas.Add(audioClipData);
            else
                Debug.Log("Couldn't find associated music file");
        }
    }

    public void LoadUI()
    {
        Utility.DestroyAllChidren(content.transform);

        foreach(AudioClipData audioClipData in audioClipDatas)
        {
            GameObject uiSongGroupGameObjectInstance = Instantiate(UISongGroupPrefab, content.transform);
            UISong uiSong = uiSongGroupGameObjectInstance.GetComponent<UISong>();
            uiSong.UpdateSongTitle(audioClipData);
            uiSong.AudioSource = audioSource;

            uiSong.OnIsSelected -= UiSong_OnIsSelected;
            uiSong.OnIsSelected += UiSong_OnIsSelected;

            toggleGroupController.Toggles.Add(uiSong.Toggle);
        }

        toggleGroupController.RegisterValueChanged();
    }

    private void UiSong_OnIsSelected(AudioClipData audioClipData)
    {
        GameManager.Instance.CurrentAudioClipData = audioClipData;
    }
}
