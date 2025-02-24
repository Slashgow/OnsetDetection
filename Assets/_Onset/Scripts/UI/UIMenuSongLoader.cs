using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

    private List<string> fileNames =new List<string>();
    private List<AudioClipData> audioClipDatas = new List<AudioClipData>();

    private void Start()
    {
        SetAllAudioClipData();
        LoadUI();
    }

    public void SetAllAudioClipData()
    {       
        //Debug.Log(Application.dataPath);
        //Debug.Log(SaveSongFolderURL.SaveSongAbsoluteFolderURL);

        List<string> filesPathsCredits = Directory.GetFiles(SaveDataPaths.SaveSongAbsoluteFolderPath, "*_credit.json").ToList();
        filesPathsCredits.ForEach(filePath => fileNames.Add(Path.GetFullPath(filePath)));

        foreach (string filePath in filesPathsCredits)
        {
            string audioClipDataText = System.IO.File.ReadAllText(filePath);
            AudioClipData  audioClipData = JsonUtility.FromJson<AudioClipData>(audioClipDataText);
            audioClipDatas.Add(audioClipData);
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
