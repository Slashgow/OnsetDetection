using UnityEngine;

public class SongNoteSpawner : MonoBehaviour
{
    [SerializeField]
    private PreProcessAudioData preProcessAudioData;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        preProcessAudioData.OnFinishAnalyseFullSpectrum += PreProcessAudioData_OnFinishAnalyseFullSpectrum;
    }

    private void OnDestroy()
    {
        preProcessAudioData.OnFinishAnalyseFullSpectrum += PreProcessAudioData_OnFinishAnalyseFullSpectrum;
    }
    private void PreProcessAudioData_OnFinishAnalyseFullSpectrum()
    {
        audioSource.clip = preProcessAudioData.AudioClip;
        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {

            int indexToPlot = preProcessAudioData.GetIndexFromTime(audioSource.time) / 1024;
            //Debug.Log(indexToPlot);
            if (preProcessAudioData.OnsetDetection.SpectralFluxInfoList[indexToPlot].isPeak)
            {
                Instantiate(prefab);
            }
        }
    }
}
