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

    private double timeToHitAudioSettings;
    private double timeElapsedAudio = 0;
    private double lastFrameDSPTime;
    private bool isAudioScheduled = false;

    private void Awake() => songLoader.OnFinishLoadingSong += SongLoader_OnFinishLoadingSong;

    private void OnDestroy() => songLoader.OnFinishLoadingSong -= SongLoader_OnFinishLoadingSong;
    private void SongLoader_OnFinishLoadingSong()
    {
        audioSource.clip = songLoader.AudioClip;
        audioSource.clip.LoadAudioData();
        timeToHitAudioSettings = AudioSettings.dspTime + songSpawnData.timeToHitFirstTrack;
        hasStarted = true;

    }

    private Transform GetTransformByDomain(FrequencyDomain domain) => transformByFrequencyDomains.First(transFormByFrequencyDomain => transFormByFrequencyDomain.Domain == domain).Transform;

    private void Update()
    {
        if (hasStarted)
        {
            int indexToPlot = songLoader.GetIndexFromTime((float)timeElapsed) / 1024;
            timeElapsed += Time.deltaTime;

            //if(lastFrameDSPTime > 0.001f)
            //{
            //    timeElapsedAudio = timeElapsedAudio + (AudioSettings.dspTime - lastFrameDSPTime);
            //    Debug.Log("incrementing elapsed time audio");
            //}
                
            //Debug.Log(indexToPlot);

            double time = AudioSettings.dspTime;
            if (!isAudioScheduled && time + 3f > timeToHitAudioSettings)
            {
                audioSource.PlayScheduled(timeToHitAudioSettings);
                //Debug.Log("scheduled audio");
                isAudioScheduled = true;
            }

            if (lastIndex == indexToPlot || indexToPlot >= songLoader.OnsetDetection.SpectralFluxInfoList.Count)
                return;

            if (!songLoader.UseFrequencyDomainClassification)
            {
                if(songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].isPeak)
                {
                    ChooseTrack(indexToPlot);
                    SpawnNote();
                    lastPeakIndex = indexToPlot;

                    //Debug.Log($"Audio source time {audioSource.time}");
                    //Debug.Log($"Peak Time : {songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time}");
                    float delay = (songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time - audioSource.time) - songSpawnData.TimeToHit(songSpawnData.Tracks[currentTrackIndex]);

                    if (delay > 0.2f)
                        audioSource.time += delay;
                    Debug.Log($"décalage : {delay}");
                }
            }

            lastIndex = indexToPlot;
            //lastFrameDSPTime = AudioSettings.dspTime;
            
        }
    }

    private void ChooseTrack(int indexToPlot)
    {
        float timeBetweenNotes = songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time - songLoader.OnsetDetection.SpectralFluxInfoList[lastPeakIndex].time;
        if (timeBetweenNotes >= timeBetweenNotesToChangeTrack && lastPeakIndex != 0)
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
