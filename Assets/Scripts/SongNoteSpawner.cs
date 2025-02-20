using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongNoteSpawner : MonoBehaviour
{
    [SerializeField]
    private PreProcessAudioData preProcessAudioData;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<TransFormByFrequencyDomain> transformByFrequencyDomains;

    private int lastIndex = 0;

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

    private Transform GetTransformByDomain(FrequencyDomain domain) => transformByFrequencyDomains.First(transFormByFrequencyDomain => transFormByFrequencyDomain.Domain == domain).Transform;

    private void Update()
    {
        if (audioSource.isPlaying)
        {

            int indexToPlot = preProcessAudioData.GetIndexFromTime(audioSource.time) / 1024;
            //Debug.Log(indexToPlot);

            if (lastIndex == indexToPlot)
                return;

            if (!preProcessAudioData.UseFrequencyDomainClassification)
            {
                if (preProcessAudioData.OnsetDetection.SpectralFluxInfoList[indexToPlot].isPeak)
                {
                    Instantiate(prefab);
                }
            }
            else
            {
                for (int i = 0; i < preProcessAudioData.OnsetDetectionFrequencyClassified.FrequencyDomainCount; i++)
                {
                    if (preProcessAudioData.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[indexToPlot].isPeak)
                    {
                        Instantiate(prefab, GetTransformByDomain(preProcessAudioData.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain));
                    }
                }
            }

            lastIndex = indexToPlot;
            
        }
    }
}
