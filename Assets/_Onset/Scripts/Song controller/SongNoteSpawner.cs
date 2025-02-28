using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongNoteSpawner : MonoBehaviour
{
    [SerializeField]
    private SongSpawnData songSpawnData;

    [SerializeField]
    private SongLoader songLoader;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<TransFormByFrequencyDomain> transformByFrequencyDomains;

    public event Action<NoteHitClassification> OnMissNoteReachedHitPoint;
    public event Action OnSongEnd;

    public float timeElapsed = 0.0f;
    private int lastIndex = 0;
    private bool hasStarted = false;
    private int currentTrackIndex = 0;
    private int lastPeakIndex;


    private double timeToHitAudioSettings;
    private bool isAudioScheduled = false;
    private bool hasEnded;

    private float timeElapsedOnSameTrack = 0.0f;
    private bool startTrackingTimeOnTrack = false;

    private void Awake() => songLoader.OnFinishLoadingSong += SongLoader_OnFinishLoadingSong;
    private void Start() => songSpawnData.Initialize();

    private void OnDestroy() => songLoader.OnFinishLoadingSong -= SongLoader_OnFinishLoadingSong;
    private void SongLoader_OnFinishLoadingSong()
    {
        audioSource.clip = songLoader.AudioClip;
        audioSource.clip.LoadAudioData();
        Debug.Log(audioSource.clip.length);
        timeToHitAudioSettings = AudioSettings.dspTime + songSpawnData.TimeToHitFirstTrack;
        hasStarted = true;

    }

    private Transform GetTransformByDomain(FrequencyDomain domain) => transformByFrequencyDomains.First(transFormByFrequencyDomain => transFormByFrequencyDomain.Domain == domain).Transform;

    private void Update()
    {
        if (hasStarted && !hasEnded && timeElapsed >= audioSource.clip.length + songSpawnData.TimeToHitFirstTrack)
        {
            Debug.Log("on song end");
            OnSongEnd?.Invoke();
            hasEnded = true;
        }

        if (hasStarted && !hasEnded)
        {
            int indexToPlot = songLoader.GetIndexFromTime((float)timeElapsed) / 1024;
            timeElapsed += Time.deltaTime;

            if (startTrackingTimeOnTrack)
                timeElapsedOnSameTrack += Time.deltaTime;

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
                    float delay = (songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time - audioSource.time) - 
                        songSpawnData.TimeToHit(songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex]);

                    if (delay > 0.1f)
                        audioSource.time += delay;
                    //Debug.Log($"décalage : {delay}");
                }
            }

            lastIndex = indexToPlot;

        }

        
    }

    private void ChooseTrack(int indexToPlot)
    {
        startTrackingTimeOnTrack = true;
        float timeBetweenNotes = songLoader.OnsetDetection.SpectralFluxInfoList[indexToPlot].time - songLoader.OnsetDetection.SpectralFluxInfoList[lastPeakIndex].time;

        if (timeBetweenNotes >= songSpawnData.CurrentSongSpawnData.TimeBetweenNotesToChangeTrack && lastPeakIndex != 0)
        {
            currentTrackIndex = (currentTrackIndex + 1) % songSpawnData.CurrentSongSpawnData.Tracks.Count;
        }
        else if(timeElapsedOnSameTrack >= songSpawnData.CurrentSongSpawnData.MaximumTimeOnTrack && timeBetweenNotes >= songSpawnData.CurrentSongSpawnData.MinTimeAbsoluteBetweenNoteToChangeTrack)
        {
            //Debug.Log("too much time on same track");
            currentTrackIndex = (currentTrackIndex + 1) % songSpawnData.CurrentSongSpawnData.Tracks.Count;
            timeElapsedOnSameTrack = 0.0f;
        }

       
    }

    private void SpawnNote()
    {
        GameObject noteGameObjectInstance = songSpawnData.NotePool.GetPrefabFromPool();

        TrackFollower trackFollowerInstance = noteGameObjectInstance.GetComponent<TrackFollower>();
        float noteSpeed = currentTrackIndex == 0 ? songSpawnData.CurrentSongSpawnData.NoteSpeed : songSpawnData.GetSpeedToMatchFirstTrack(songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex]); 
        trackFollowerInstance.Setup(songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex].PathCreatorToHitNote, songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex].PathCreatorFromHitNoteToPlanet, noteSpeed);
        
        Note noteInstance = noteGameObjectInstance.GetComponent<Note>();
        noteInstance.OnMissNote -= NoteInstance_OnMissHitPoint;
        noteInstance.OnMissNote += NoteInstance_OnMissHitPoint;
        noteInstance.Setup(songSpawnData.NotePool, songSpawnData.TimeToHit(songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex]));

    }

    private void NoteInstance_OnMissHitPoint(NoteHitClassification noteHitClassification)
    {
        OnMissNoteReachedHitPoint?.Invoke(noteHitClassification);
    }
}
