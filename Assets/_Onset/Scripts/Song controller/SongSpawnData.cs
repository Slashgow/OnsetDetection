using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SongSpawnData
{
    [SerializeField]
    private List<Track> tracks;
    public List<Track> Tracks => tracks;

    [SerializeField, Range(0f, 10f)]
    private float noteSpeed;
    public float NoteSpeed => noteSpeed;

    [SerializeField]
    private PoolingSystem notePool;
    public PoolingSystem NotePool => notePool;

    public float timeToHitFirstTrack => tracks[0].PathCreatorToHitNote.path.length / noteSpeed;
    public float TimeToHit(Track track) => track.PathCreatorToHitNote.path.length / GetSpeedToMatchFirstTrack(track);

    public float GetSpeedToMatchFirstTrack(Track track) => track.PathCreatorToHitNote.path.length / timeToHitFirstTrack;
}
