using System;
using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField]
    private SongNoteSpawner noteSpawner;

    [SerializeField]
    private GunController gunController;

    [SerializeField, Range(1,100)]
    private int scorePerHitBase = 10;

    [SerializeField, Range(0,30)]
    private int numberOfNoteToNextMultiplier = 10;

    private int score = 0;
    private int currentMultiplier = 1;
    private int numberOfSuccessiveNotes = 0;

    public event Action<int, NoteHitClassification> OnScoreUpdated;
    public event Action<int> OnMultiplierUpdated;


    private void Start()
    {
        noteSpawner.OnMissNoteReachedHitPoint += NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote += GunController_OnHitNote;
    }
    private void OnDestroy()
    {
        noteSpawner.OnMissNoteReachedHitPoint -= NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote -= GunController_OnHitNote;
    }

    private void GunController_OnHitNote(Note note) => UpdateScore(note.NoteHitClassification);

    private void NoteSpawner_OnNoteReachedHitPoint(NoteHitClassification noteHitClassification) => UpdateScore(noteHitClassification);

    private void UpdateScore(NoteHitClassification noteHitClassification)
    {
        if (noteHitClassification == NoteHitClassification.MISS)
        {
            currentMultiplier = 1;
            OnMultiplierUpdated?.Invoke(currentMultiplier);
            numberOfSuccessiveNotes = 0;
        }
        else
        {
            numberOfSuccessiveNotes++;

            if (numberOfSuccessiveNotes >= numberOfNoteToNextMultiplier * currentMultiplier)
            {
                currentMultiplier++;
                currentMultiplier = Mathf.Clamp(currentMultiplier, 1, 4);
                OnMultiplierUpdated?.Invoke(currentMultiplier);
            }

            score += scorePerHitBase * currentMultiplier;
        }
        OnScoreUpdated?.Invoke(score, noteHitClassification);
    }

}
