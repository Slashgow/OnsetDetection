using System;
using UnityEngine;
using UnityEngine.UI;

public class UISongRemapper : MonoBehaviour
{
    [SerializeField]
    private UISong uiSong;

    [SerializeField]
    private Button remapButton;


    private void OnEnable()
    {
        uiSong.OnAudioClipPathNotValid += UiSong_OnAudioClipPathNotValid;
        remapButton.onClick.AddListener(Remap);
        remapButton.interactable = false;
    }
    private void OnDisable()
    {
        uiSong.OnAudioClipPathNotValid -= UiSong_OnAudioClipPathNotValid;
        remapButton.onClick.RemoveListener(Remap);
    }

    private void Remap()
    {
        SongImporter.Instance.OpenFileBrowser(false);

        SongImporter.Instance.OnEndImportAudioClip -= SongImporter_OnEndImportAudioClip;
        SongImporter.Instance.OnEndImportAudioClip += SongImporter_OnEndImportAudioClip;
    }
    private void UiSong_OnAudioClipPathNotValid() => remapButton.interactable = true;

    private void SongImporter_OnEndImportAudioClip()
    {

        AudioClipData audioClipDataValidPath = new AudioClipData(uiSong.AudioClipData.SongTitle, uiSong.AudioClipData.Author,
            uiSong.AudioClipData.Extension, SongImporter.Instance.CurrentFilePath, uiSong.AudioClipData.IsCampaign);

        SaveNewPathToJSON(audioClipDataValidPath);
        UpdateUISongAudioClipData(audioClipDataValidPath);
    }

    private void UpdateUISongAudioClipData(AudioClipData validPathAudioClipData) => uiSong.UpdateAudioClipData(validPathAudioClipData);

    private void SaveNewPathToJSON(AudioClipData audioClipDataValidPath)
    {
        string creditsData = JsonUtility.ToJson(audioClipDataValidPath);

        string filePathCredits = audioClipDataValidPath.IsCampaign ?
               SaveDataPaths.GetAbsolutePathSongCreditsCampaign(audioClipDataValidPath) :
               SaveDataPaths.GetAbsolutePathSongCreditsImported(audioClipDataValidPath);

        if (System.IO.File.Exists(filePathCredits))
            System.IO.File.Delete(filePathCredits);

        System.IO.File.WriteAllText(filePathCredits, creditsData);
    }
}
