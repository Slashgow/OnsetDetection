using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTimer;

public class SongNoteSpawner : MonoBehaviour
{
    [SerializeField]
    private SongSpawnData songSpawnData;

    [SerializeField]
    private PreProcessAudioData preProcessAudioData;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<TransFormByFrequencyDomain> transformByFrequencyDomains;

    public event Action<NoteHitClassification> OnMissNoteReachedHitPoint;

    public float timeElapsed = 0.0f;
    private int lastIndex = 0;
    private bool hasStarted = false;

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
        //audioSource.Play();
        hasStarted = true;
        Timer.Register(songSpawnData.TimeToHit, () => {
            Debug.Log("Start Playing audio");
            audioSource.Play();
        });
        
    }

    private Transform GetTransformByDomain(FrequencyDomain domain) => transformByFrequencyDomains.First(transFormByFrequencyDomain => transFormByFrequencyDomain.Domain == domain).Transform;

    private void Update()
    {
        if (hasStarted)
        {
            
            int indexToPlot = preProcessAudioData.GetIndexFromTime(timeElapsed) / 1024;
            timeElapsed += Time.deltaTime;
            //Debug.Log(indexToPlot);

            if (lastIndex == indexToPlot)
                return;

            if (!preProcessAudioData.UseFrequencyDomainClassification)
            {
                if (preProcessAudioData.OnsetDetection.SpectralFluxInfoList[indexToPlot].isPeak)
                {
                    SpawnNote();
                }
            }
            else
            {
                for (int i = 0; i < preProcessAudioData.OnsetDetectionFrequencyClassified.FrequencyDomainCount; i++)
                {
                    if (preProcessAudioData.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[indexToPlot].isPeak)
                    {
                        //Instantiate(notePrefab, GetTransformByDomain(preProcessAudioData.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain));
                    }
                }
            }

            lastIndex = indexToPlot;
            
        }
    }

    private void SpawnNote()
    {
        GameObject noteGameObjectInstance = songSpawnData.NotePool.GetPrefabFromPool();

        TrackFollower trackFollowerInstance = noteGameObjectInstance.GetComponent<TrackFollower>();
        trackFollowerInstance.Setup(songSpawnData.Track.PathCreatorToHitNote, songSpawnData.Track.PathCreatorFromHitNoteToPlanet, songSpawnData.NoteSpeed);
        
        Note noteInstance = noteGameObjectInstance.GetComponent<Note>();
        noteInstance.OnMissNote -= NoteInstance_OnMissHitPoint;
        noteInstance.OnMissNote += NoteInstance_OnMissHitPoint;
        noteInstance.Setup(songSpawnData.NotePool, songSpawnData.TimeToHit);

    }

    private void NoteInstance_OnMissHitPoint(NoteHitClassification noteHitClassification)
    {
        OnMissNoteReachedHitPoint?.Invoke(noteHitClassification);
    }
}
