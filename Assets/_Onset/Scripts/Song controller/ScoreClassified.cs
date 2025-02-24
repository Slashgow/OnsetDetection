using System;
using UnityEngine;

[Serializable]
public class ScoreClassified
{
    [SerializeField]
    private NoteHitClassification hitClassification;
    public NoteHitClassification HitClassification => hitClassification;
    public int NumberOfHit { get; set; } 
}
