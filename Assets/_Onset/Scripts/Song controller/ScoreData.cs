using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScoreData
{
    [SerializeField, Range(1, 100)]
    private int scorePerHitBase = 10;
    public int ScorePerHitBase => scorePerHitBase;

    [SerializeField, Range(0, 30)]
    private int numberOfNoteToNextMultiplier = 10;
    public int NumberOfNoteToNextMultiplier => numberOfNoteToNextMultiplier;

    public int Score { get; set; }
    public int CurrentMultiplier { get; set; }
    public int NumberOfSuccessiveNotes { get; set; }
    public float PercentageOfNoteHit
    {
        get
        {
            int sumTotalHit = 0;
            foreach (ScoreClassified scoreClassified in scoreClassifiedList)
            {
                if(scoreClassified.HitClassification != NoteHitClassification.MISS)
                {
                    sumTotalHit++;
                }
            }
            return sumTotalHit / TotalNumberOfNotes;
        }
    }

    public int TotalNumberOfNotes { get; set; }

    public int LastMultiplier { get; set; }

    [SerializeField]
    private List<ScoreClassified> scoreClassifiedList;
    public List<ScoreClassified> ScoreClassifiedList 
    { 
        get => scoreClassifiedList;
        set
        {
            scoreClassifiedList = value;
        } 
    }

    public ScoreData()
    {
        this.Score = 0;
        this.CurrentMultiplier = 1;
        this.NumberOfSuccessiveNotes = 0;
    }

    public ScoreClassified GetScoreClassified(NoteHitClassification noteHitClassification) => scoreClassifiedList.First(scoreClassified => scoreClassified.HitClassification == noteHitClassification);
}
