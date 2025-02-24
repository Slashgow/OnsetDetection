using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuSongImporter : MonoBehaviour
{
    [SerializeField]
    private PreProcessAudioData preProcessAudioData;

    [SerializeField]
    private TMP_InputField authorInputField;

    [SerializeField]
    private TMP_InputField songInputField;

    [SerializeField]
    private Button generateTrackButton;

    [SerializeField]
    private TextMeshProUGUI statusGenerationText;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private string missingInfo, completed, generating;

    private AudioClipData audioClipData;    

    private void Awake()
    {
        generateTrackButton.onClick.AddListener(GenerateTrack);
        startButton.interactable = false;
    }

    private void OnEnable()
    {
        preProcessAudioData.OnFinishAnalyseFullSpectrum += PreProcessAudioData_OnFinishAnalyseFullSpectrum;
    }
    private void OnDestroy()
    {
        generateTrackButton.onClick.RemoveListener(GenerateTrack);
        preProcessAudioData.OnFinishAnalyseFullSpectrum -= PreProcessAudioData_OnFinishAnalyseFullSpectrum;
    }

    private void PreProcessAudioData_OnFinishAnalyseFullSpectrum()
    {
        statusGenerationText.text = completed;
        GameManager.Instance.CurrentAudioClipData = audioClipData;
        startButton.interactable = true;
        
    }

    private void GenerateTrack()
    {
        if(!string.IsNullOrEmpty(authorInputField.text) && !string.IsNullOrEmpty(songInputField.text))
        {
            statusGenerationText.text = generating;
            audioClipData = new AudioClipData(songInputField.text, authorInputField.text, SongImporter.Instance.Extension);
            preProcessAudioData.audioClipMapper = new AudioClipMapper(SongImporter.Instance.AudioClip, audioClipData);
            preProcessAudioData.GenerateTrack();

            SongImporter.Instance.CopyImportedFile(SongImporter.Instance.CurrentFilePath, SaveDataPaths.AbsolutePathAudioClipDirectory, SaveDataPaths.GetFileName(audioClipData));
        }
        else
        {
            statusGenerationText.text = missingInfo;
        }
   
    }
}
