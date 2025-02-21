using System;
using UnityEngine;

[Serializable]
public class NoteHitClassificationData 
{
    [SerializeField]
    private NoteHitClassification hitClassification;
    public NoteHitClassification HitClassification => hitClassification;

    [SerializeField, Range(-2f, 15f)]
    private float startTimeRemaming;
    public float StartTimeRemaining => startTimeRemaming;

    [SerializeField, Range(-2f, 15f)]
    private float endTimeRemaming;
    public float EndTimeRemaining => endTimeRemaming;

}
