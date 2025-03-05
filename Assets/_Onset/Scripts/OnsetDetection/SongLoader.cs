using System;
using System.Collections;
using UnityEngine;

public class SongLoader : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [SerializeField]
    private bool useFrequencyDomainClassification;
    public bool UseFrequencyDomainClassification => useFrequencyDomainClassification;

    private float sampleRate;

    public OnsetDetection OnsetDetection { get; private set; }

    public event Action OnFinishLoadingSong;
    

    private void Start()
    {
        string audioClipPath = GameManager.Instance.CurrentAudioClipData.IsCampaign ?
                SaveDataPaths.GetAbsolutePathAudioClipCampaign(GameManager.Instance.CurrentAudioClipData) :
                GameManager.Instance.CurrentAudioClipData.AudioClipPath;

        LoadAudioAsync(audioClipPath);
    }

    private void LoadAudioAsync(string path)
    {
        StartCoroutine(AudioClipImporter.Instance.LoadAudioClip(path));
        AudioClipImporter.Instance.OnEndImportAudioClip -= SongImporter_OnEndImportAudioClip;
        AudioClipImporter.Instance.OnEndImportAudioClip += SongImporter_OnEndImportAudioClip;
    }

    private void SongImporter_OnEndImportAudioClip()
    {
        audioClip = AudioClipImporter.Instance.AudioClip;
        sampleRate = audioClip.frequency;
        LoadSongDataFromJson();
    }

    public void LoadSongDataFromJson()
    {
        if (!useFrequencyDomainClassification)
        {
            string filePath = GameManager.Instance.CurrentAudioClipData.IsCampaign ? 
                SaveDataPaths.GetAbsolutePathSongMapCampaign(GameManager.Instance.CurrentAudioClipData) :
                SaveDataPaths.GetAbsolutePathSongMapImported(GameManager.Instance.CurrentAudioClipData); 
     

            if (!System.IO.File.Exists(filePath))
            {
                Debug.Log($"path : {filePath} doesnt exist");
                return;
            }

            string songData = System.IO.File.ReadAllText(filePath);
            OnsetDetection = JsonUtility.FromJson<OnsetDetection>(songData);
            OnFinishLoadingSong?.Invoke();
            Debug.Log("Chargement effectué");
        }
    }

    public int GetIndexFromTime(float curTime) => Mathf.FloorToInt(curTime * this.sampleRate);
    public float GetTimeFromIndex(int index) => ((1f / (float)this.sampleRate) * index);
}
