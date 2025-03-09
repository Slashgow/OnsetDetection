using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public abstract class SongSpawnData
{
    [SerializeField]
    private List<SongSpawnDataByDifficulty> spawnDataByDifficultyList;

    [SerializeField]
    private PoolingSystem notePool;
    public PoolingSystem NotePool => notePool;

    public SongSpawnDataByDifficulty GetSongSpawnDataByDifficulty(Difficulty difficulty) => spawnDataByDifficultyList.First(spawnData =>  spawnData.Difficulty == difficulty);

    protected float timeToHitFirstTrack;
    public virtual float TimeToHitFirstTrack => timeToHitFirstTrack;

    protected SongSpawnDataByDifficulty currentSongSpawnData;
    public SongSpawnDataByDifficulty CurrentSongSpawnData => currentSongSpawnData;

    public virtual void Initialize()
    {
        currentSongSpawnData = GetSongSpawnDataByDifficulty(DifficultyManager.Instance.CurrentDifficulty);
       
    }

    public abstract float TimeToHit(Track track);

    public float GetSpeedToMatchFirstTrack(Track track) => track.DistanceToHitPoint / timeToHitFirstTrack;


}
