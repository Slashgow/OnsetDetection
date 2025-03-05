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

    [SerializeField]
    private bool debugIsCampaign;

    private AudioClipData audioClipData;    

    private void Awake()
    {
        generateTrackButton.onClick.AddListener(GenerateTrack);
        startButton.interactable = false;
        statusGenerationText.text = string.Empty;
        startButton.onClick.AddListener(LoadScene);
    }

    private void LoadScene() => SceneLoader.Instance.LoadScene(1);

    private void OnEnable()
    {
        generateTrackButton.interactable =true;
        preProcessAudioData.OnFinishAnalyseFullSpectrum += PreProcessAudioData_OnFinishAnalyseFullSpectrum;
    }
    private void OnDestroy()
    {
        generateTrackButton.onClick.RemoveListener(GenerateTrack);
        preProcessAudioData.OnFinishAnalyseFullSpectrum -= PreProcessAudioData_OnFinishAnalyseFullSpectrum;
        startButton.onClick.RemoveListener(LoadScene);
    }

    private void PreProcessAudioData_OnFinishAnalyseFullSpectrum()
    {
        statusGenerationText.text = completed;
        GameManager.Instance.CurrentAudioClipData = audioClipData;
        startButton.interactable = true;
        
    }

    private void GenerateTrack()
    {
        
        if(!string.IsNullOrEmpty(authorInputField.text) && !string.IsNullOrEmpty(songInputField.text) && AudioClipImporter.Instance.AudioClip != null)
        {
            generateTrackButton.interactable = false;
            statusGenerationText.text = generating;

#if UNITY_EDITOR
            audioClipData = new AudioClipData(songInputField.text, authorInputField.text, AudioClipImporter.Instance.Extension, AudioClipImporter.Instance.CurrentFilePath, debugIsCampaign);
#else
            audioClipData = new AudioClipData(songInputField.text, authorInputField.text, AudioClipImporter.Instance.Extension, AudioClipImporter.Instance.CurrentFilePath, false);
#endif
            preProcessAudioData.audioClipMapper = new AudioClipMapper(AudioClipImporter.Instance.AudioClip, audioClipData);
            preProcessAudioData.GenerateTrack();

            //SongImporter.Instance.CopyImportedFile(SongImporter.Instance.CurrentFilePath, SaveDataPaths.AudioClipCampaignAbsoluteFolderPath, SaveDataPaths.GetFileName(audioClipData));
        }
        else
        {
            statusGenerationText.text = missingInfo;
        }
   
    }
}
