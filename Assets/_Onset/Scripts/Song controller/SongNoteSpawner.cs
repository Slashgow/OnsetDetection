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
    private SongLoader songLoader;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField, Range(0f,2f)]
    private float timeBetweenNotesToChangeTrack;

    [SerializeField]
    private List<TransFormByFrequencyDomain> transformByFrequencyDomains;

    public event Action<NoteHitClassification> OnMissNoteReachedHitPoint;

    public float timeElapsed = 0.0f;
    private int lastIndex = 0;
    private bool hasStarted = false;
    private int currentTrackIndex = 0;
    private int lastPeakIndex;

    private void Awake() => songLoader.OnFinishLoadingSong += SongLoader_OnFinishLoadingSong;

    private void OnDestroy() => songLoader.OnFinishLoadingSong -= SongLoader_OnFinishLoadingSong;
    private void SongLoader_OnFinishLoadingSong()
    {
        audioSource.clip = songLoader.AudioClip;
        hasStarted = true;
        Timer.Register(songSpawnData.TimeToHit(songSpawnData.Tracks[0]), () => {
            Debug.Log("Start Playing audio");
            audioSource.Play();
        });
        
    }

    private Transform GetTransformByDomain(FrequencyDomain domain) => transformByFrequencyDomains.First(transFormByFrequencyDomain => transFormByFrequencyDomain.Domain == domain).Transform;

    private void Update()
    {
        if (hasStarted)
        {
            
            int indexToPlot = songLoader.GetIndexFromTime(timeElapsed) / 1024;
            timeElapsed += Time.deltaTime;
            //Debug.Log(indexToPlot);

            if (lastIndex == indexToPlot || indexToPlot >= songLoader.OnsetDetection.SpectralFluxInfoList.Count)
                return;

            if (!songLoader.UseFrequencyDomainClassification)
            {
                if(songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].isPeak)
                {
                    ChooseTrack(indexToPlot);
                    SpawnNote();
                    lastPeakIndex = indexToPlot;
                }
            }
            else
            {
               //for (int i = 0; i < songLoader.OnsetDetectionFrequencyClassified.FrequencyDomainCount; i++)
               //{
               //    if (songLoader.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[indexToPlot].isPeak)
               //    {
               //        //Instantiate(notePrefab, GetTransformByDomain(preProcessAudioData.OnsetDetectionFrequencyClassified.SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain));
               //    }
               //}
            }

            lastIndex = indexToPlot;
            
        }
    }

    private void ChooseTrack(int indexToPlot)
    {
        float timeBetweenNotes = songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time - songLoader.OnsetDetection.SpectralFluxInfoList[lastPeakIndex].time;
        if (timeBetweenNotes >= timeBetweenNotesToChangeTrack)
        {
            currentTrackIndex = (currentTrackIndex + 1) % songSpawnData.Tracks.Count;
        }
    }

    private void SpawnNote()
    {
        GameObject noteGameObjectInstance = songSpawnData.NotePool.GetPrefabFromPool();

        TrackFollower trackFollowerInstance = noteGameObjectInstance.GetComponent<TrackFollower>();
        float noteSpeed = currentTrackIndex == 0 ? songSpawnData.NoteSpeed : songSpawnData.GetSpeedToMatchFirstTrack(songSpawnData.Tracks[currentTrackIndex]); 
        trackFollowerInstance.Setup(songSpawnData.Tracks[currentTrackIndex].PathCreatorToHitNote, songSpawnData.Tracks[currentTrackIndex].PathCreatorFromHitNoteToPlanet, noteSpeed);
        
        Note noteInstance = noteGameObjectInstance.GetComponent<Note>();
        noteInstance.OnMissNote -= NoteInstance_OnMissHitPoint;
        noteInstance.OnMissNote += NoteInstance_OnMissHitPoint;
        noteInstance.Setup(songSpawnData.NotePool, songSpawnData.TimeToHit(songSpawnData.Tracks[currentTrackIndex]));

    }

    private void NoteInstance_OnMissHitPoint(NoteHitClassification noteHitClassification)
    {
        OnMissNoteReachedHitPoint?.Invoke(noteHitClassification);
    }
}
