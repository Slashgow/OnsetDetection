using System;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textScore;

    [SerializeField]
    private TextMeshProUGUI textMutliplier;

    [SerializeField]
    private TextMeshProUGUI textClassification;

    private void Start()
    {
        ScoreManager.Instance.OnScoreUpdated -= ScoreManager_OnScoreUpdated;
        ScoreManager.Instance.OnScoreUpdated += ScoreManager_OnScoreUpdated;

        ScoreManager.Instance.OnMultiplierUpdated -= ScoreManager_OnMultiplierUpdated;
        ScoreManager.Instance.OnMultiplierUpdated += ScoreManager_OnMultiplierUpdated;

        textScore.text = "0";
        textMutliplier.text = "X 1";
    }

    private void ScoreManager_OnMultiplierUpdated(int multiplier)
    {
        textMutliplier.text = $"X {multiplier}";
    }

    private void ScoreManager_OnScoreUpdated(int score, NoteHitClassification noteHitClassification)
    {
        textScore.text = score.ToString();
        textClassification.text = noteHitClassification.ToString();
    }
}
