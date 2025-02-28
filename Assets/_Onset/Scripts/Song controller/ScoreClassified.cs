using System;
using UnityEngine;

[Serializable]
public class ScoreClassified
{
    [SerializeField]
    private NoteHitClassification hitClassification;
    public NoteHitClassification HitClassification => hitClassification;

    [SerializeField, Range(0f,50f)]
    private int scorePerHitClassified;
    public int ScorePerHitClassified => scorePerHitClassified;
    public int NumberOfHit { get; set; } 
}
