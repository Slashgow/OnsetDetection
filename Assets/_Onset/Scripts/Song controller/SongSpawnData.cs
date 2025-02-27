using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SongSpawnData
{
    [SerializeField]
    private List<SongSpawnDataByDifficulty> spawnDataByDifficultyList;

    [SerializeField]
    private PoolingSystem notePool;
    public PoolingSystem NotePool => notePool;

    public SongSpawnDataByDifficulty GetSongSpawnDataByDifficulty(Difficulty difficulty) => spawnDataByDifficultyList.First(spawnData =>  spawnData.Difficulty == difficulty);

    private float timeToHitFirstTrack;
    public float TimeToHitFirstTrack => timeToHitFirstTrack;

    private SongSpawnDataByDifficulty currentSongSpawnData;
    public SongSpawnDataByDifficulty CurrentSongSpawnData => currentSongSpawnData;

    public void Initialize()
    {
        currentSongSpawnData = GetSongSpawnDataByDifficulty(DifficultyManager.Instance.CurrentDifficulty);
        timeToHitFirstTrack = currentSongSpawnData.Tracks[0].PathCreatorToHitNote.path.length / currentSongSpawnData.NoteSpeed;
 
        currentSongSpawnData.Tracks.ForEach(track => track.InitTarget()); 
    }

    public float TimeToHit(Track track) => track.PathCreatorToHitNote.path.length / GetSpeedToMatchFirstTrack(track);

    public float GetSpeedToMatchFirstTrack(Track track) => track.PathCreatorToHitNote.path.length / timeToHitFirstTrack;

    
}
