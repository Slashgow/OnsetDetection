using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializeField]
public class SongSpawnDataCircle : SongSpawnData
{
    [SerializeField]
    private float timeToHit;
    public override float TimeToHitFirstTrack => timeToHit;
}


[Serializable]
public class SongSpawnData
{
    [SerializeField]
    private List<SongSpawnDataByDifficulty> spawnDataByDifficultyList;

    [SerializeField]
    private PoolingSystem notePool;
    public PoolingSystem NotePool => notePool;

    public SongSpawnDataByDifficulty GetSongSpawnDataByDifficulty(Difficulty difficulty) => spawnDataByDifficultyList.First(spawnData =>  spawnData.Difficulty == difficulty);

    protected float timeToHitFirstTrack;
    public virtual float TimeToHitFirstTrack => timeToHitFirstTrack;

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
