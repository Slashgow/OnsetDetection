using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SongSpawnDataByDifficulty
{
    [SerializeField]
    private Difficulty difficulty;
    public Difficulty Difficulty => difficulty;

    [SerializeField]
    private List<Track> tracks;
    public List<Track> Tracks => tracks;

    [SerializeField, Range(0f, 2f)]
    private float timeBetweenNotesToChangeTrack;
    public float TimeBetweenNotesToChangeTrack => timeBetweenNotesToChangeTrack;

    [SerializeField, Range(0f, 20f)]
    private float maximumTimeOnTrack = 10f;
    public float MaximumTimeOnTrack => maximumTimeOnTrack;

    [SerializeField, Range(0f, 0.8f)]
    private float minTimeAbsoluteBetweenNoteToChangeTrack;
    public float MinTimeAbsoluteBetweenNoteToChangeTrack => minTimeAbsoluteBetweenNoteToChangeTrack;

    [SerializeField, Range(0f, 20f)]
    private float noteSpeed;
    public float NoteSpeed => noteSpeed;
}
