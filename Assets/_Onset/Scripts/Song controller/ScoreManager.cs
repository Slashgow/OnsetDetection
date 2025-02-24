using System;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField]
    private SongNoteSpawner noteSpawner;

    [SerializeField]
    private GunController gunController;

    [SerializeField]
    private ScoreData scoreData;

    public event Action<int, NoteHitClassification> OnScoreUpdated;
    public event Action<int> OnMultiplierUpdated;
    public event Action<ScoreData> OnSondEnd;


    private void Start()
    {
        noteSpawner.OnMissNoteReachedHitPoint += NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote += GunController_OnHitNote;
        noteSpawner.OnSongEnd += NoteSpawner_OnSongEnd;
    }

    private void NoteSpawner_OnSongEnd() => OnSondEnd?.Invoke(scoreData);

    private void OnDestroy()
    {
        noteSpawner.OnMissNoteReachedHitPoint -= NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote -= GunController_OnHitNote;
        noteSpawner.OnSongEnd -= NoteSpawner_OnSongEnd;
    }

    private void GunController_OnHitNote(Note note) => UpdateScore(note.NoteHitClassification);

    private void NoteSpawner_OnNoteReachedHitPoint(NoteHitClassification noteHitClassification) => UpdateScore(noteHitClassification);

    private void IncreaseHit(NoteHitClassification hitClassification) => scoreData.ScoreClassifiedList.First(scoreClassified => scoreClassified.HitClassification == hitClassification).NumberOfHit++;

    private void UpdateScore(NoteHitClassification noteHitClassification)
    {
        if (noteHitClassification == NoteHitClassification.MISS)
        {
            scoreData.CurrentMultiplier = 1;

            if(scoreData.LastMultiplier != scoreData.CurrentMultiplier)
                OnMultiplierUpdated?.Invoke(scoreData.CurrentMultiplier);

            scoreData.NumberOfSuccessiveNotes = 0;
            scoreData.LastMultiplier = scoreData.CurrentMultiplier;
        }
        else
        {
            scoreData.NumberOfSuccessiveNotes++;

            if (scoreData.NumberOfSuccessiveNotes >= scoreData.NumberOfNoteToNextMultiplier * scoreData.CurrentMultiplier)
            {
                scoreData.CurrentMultiplier++;
                scoreData.CurrentMultiplier = Mathf.Clamp(scoreData.CurrentMultiplier, 1, 4);
                
                if(scoreData.LastMultiplier != scoreData.CurrentMultiplier)
                    OnMultiplierUpdated?.Invoke(scoreData.CurrentMultiplier);

                scoreData.LastMultiplier = scoreData.CurrentMultiplier;
            }

            scoreData.Score += scoreData.ScorePerHitBase * scoreData.CurrentMultiplier;
            OnScoreUpdated?.Invoke(scoreData.Score, noteHitClassification);
        }

        IncreaseHit(noteHitClassification);
        scoreData.TotalNumberOfNotes++;
      
    }

}
