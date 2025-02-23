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
        StartCoroutine(LoadAudioAsync($"Songs/{SaveSongFolderURL.GetFileName(GameManager.Instance.CurrentAudioClipData.Author, GameManager.Instance.CurrentAudioClipData.SongTitle)}"));
    }

    private IEnumerator LoadAudioAsync(string path)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<AudioClip>(path);
        yield return resourceRequest;
        audioClip = resourceRequest.asset as AudioClip;
        sampleRate = audioClip.frequency;
        LoadSongDataFromJson();
    }
    public void LoadSongDataFromJson()
    {
        if (!useFrequencyDomainClassification)
        {
            string filePath = SaveSongFolderURL.GetAbsoluteURLSongMap(GameManager.Instance.CurrentAudioClipData); //TO DO   SaveSongFolderURL.GetAbsoluteURLSongMap();
     

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
