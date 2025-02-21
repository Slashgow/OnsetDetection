using System;
using UnityEngine;

[Serializable]
public class SongSpawnData
{
    [SerializeField]
    private Track track;
    public Track Track => track;

    [SerializeField, Range(0f, 10f)]
    private float noteSpeed;
    public float NoteSpeed => noteSpeed;

    [SerializeField]
    private PoolingSystem notePool;
    public PoolingSystem NotePool => notePool;
    public float TimeToHit => track.PathCreatorToHitNote.path.length / noteSpeed;
}
