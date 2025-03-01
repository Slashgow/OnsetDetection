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

    public int NumberOfNoteToStarPower => scoreData.NumberOfNoteToStarPower;

    public event Action<int, int> OnNumberOfSuccessiveNoteUpdated;
    public event Action<int, NoteHitClassification> OnScoreUpdated;
    public event Action<int> OnMultiplierUpdated;
    public event Action<ScoreData> OnSongEnd;
    public event Action OnUnlockedStarPower;

    private bool isStarPowerUnlocked;

    private void Start()
    {
        noteSpawner.OnMissNoteReachedHitPoint += NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote += GunController_OnHitNote;
        gunController.OnShootNoNote += GunController_OnShootNoNote;
        noteSpawner.OnSongEnd += NoteSpawner_OnSongEnd;
    }
    private void OnDestroy()
    {
        noteSpawner.OnMissNoteReachedHitPoint -= NoteSpawner_OnNoteReachedHitPoint;
        gunController.OnHitNote -= GunController_OnHitNote;
        gunController.OnShootNoNote -= GunController_OnShootNoNote;
        noteSpawner.OnSongEnd -= NoteSpawner_OnSongEnd;
    }

    private void GunController_OnShootNoNote()
    {
        scoreData.Score -= scoreData.ScoreDecreasePerShoot;
        OnScoreUpdated?.Invoke(scoreData.Score, NoteHitClassification.MISS);
        
        scoreData.CurrentMultiplier = 1;

        if (scoreData.CurrentMultiplier != scoreData.LastMultiplier)
            OnMultiplierUpdated?.Invoke(scoreData.CurrentMultiplier);

        if(scoreData.NumberOfSuccessiveNotes > 0)
        {
            scoreData.NumberOfSuccessiveNotes = 0;
            scoreData.NumberOfSuccessiveNotesToStarPower = 0;
            OnNumberOfSuccessiveNoteUpdated?.Invoke(scoreData.NumberOfSuccessiveNotes, scoreData.NumberOfSuccessiveNotesToStarPower);
        }
    
        scoreData.LastMultiplier = scoreData.CurrentMultiplier;
    }

    private void NoteSpawner_OnSongEnd()
    {
        int lastHighScore = SaveDataPaths.GetHighScore(GameManager.Instance.CurrentAudioClipData);
        if (scoreData.Score > lastHighScore)
        {
            scoreData.HighScore = scoreData.Score;
            SaveDataPaths.SetHighScore(GameManager.Instance.CurrentAudioClipData, scoreData.HighScore);
        }
        else
        {
            scoreData.HighScore = lastHighScore;
        }

        OnSongEnd?.Invoke(scoreData);
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

            if (scoreData.NumberOfSuccessiveNotes > 0)
            {
                scoreData.NumberOfSuccessiveNotes = 0;
                scoreData.NumberOfSuccessiveNotesToStarPower = 0;
                OnNumberOfSuccessiveNoteUpdated?.Invoke(scoreData.NumberOfSuccessiveNotes, scoreData.NumberOfSuccessiveNotesToStarPower);
            }
    
            scoreData.LastMultiplier = scoreData.CurrentMultiplier;
            OnScoreUpdated?.Invoke(scoreData.Score, noteHitClassification);
        }
        else
        {
            scoreData.NumberOfSuccessiveNotes++;
            scoreData.NumberOfSuccessiveNotesToStarPower++;
            OnNumberOfSuccessiveNoteUpdated?.Invoke(scoreData.NumberOfSuccessiveNotes, scoreData.NumberOfSuccessiveNotesToStarPower);
            if(scoreData.NumberOfSuccessiveNotes >= NumberOfNoteToStarPower && !isStarPowerUnlocked)
            {
                Debug.Log("star power is unlocked");
                OnUnlockedStarPower?.Invoke();
            }

            if (scoreData.NumberOfSuccessiveNotes >= scoreData.NumberOfNoteToNextMultiplier * scoreData.CurrentMultiplier)
            {
                scoreData.CurrentMultiplier++;
                scoreData.CurrentMultiplier = Mathf.Clamp(scoreData.CurrentMultiplier, 1, 4);
                
                if(scoreData.LastMultiplier != scoreData.CurrentMultiplier)
                    OnMultiplierUpdated?.Invoke(scoreData.CurrentMultiplier);

                scoreData.LastMultiplier = scoreData.CurrentMultiplier;
            }

            scoreData.Score += scoreData.GetScoreClassified(noteHitClassification).ScorePerHitClassified * scoreData.CurrentMultiplier;
            OnScoreUpdated?.Invoke(scoreData.Score, noteHitClassification);
        }

        IncreaseHit(noteHitClassification);
        scoreData.TotalNumberOfNotes++;
      
    }

    public void UseStarPower()
    {
        Debug.Log("star power was used");
        isStarPowerUnlocked = false;
        scoreData.NumberOfSuccessiveNotesToStarPower = 0;
        scoreData.CurrentMultiplier *= 2;
        OnMultiplierUpdated?.Invoke(scoreData.CurrentMultiplier);
    }

}
