using System;
using DG.Tweening;
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

    [SerializeField]
    private Ease easing;

    [SerializeField, Range(0f, 1f)]
    private float duration = 0.5f;

    [SerializeField, Range(1f, 5f)]
    private float maxScale = 3f;

    [SerializeField, Range(0f, 180f)]
    private float maxZRotation = 30f;

    private Sequence sequenceScore;
    private Tween rotateTweenScore;

    private Sequence sequenceClassification;
    private Tween rotateTweenClassification;

    private Sequence sequenceMultiplier;
    private Tween rotateTweenMultiplier;

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
        AnimateMultiplier();
    }

    private void ScoreManager_OnScoreUpdated(int score, NoteHitClassification noteHitClassification)
    {
        textScore.text = score.ToString();
        textClassification.text = noteHitClassification.ToString();

        AnimateScore();
        AnimateClassification();
    }

    private void AnimateScore()
    {
        if (sequenceScore != null)
            sequenceScore.Kill();

        if (rotateTweenScore != null)
            rotateTweenScore.Kill();

        sequenceScore = DOTween.Sequence();
        sequenceScore.Append(textScore.transform.DOScale(maxScale, duration).SetEase(easing));
        sequenceScore.Append(textScore.transform.DOScale(1, duration).SetEase(easing));

        rotateTweenScore = textScore.transform.DOLocalRotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, maxZRotation)), duration).SetEase(easing).OnComplete(() => transform.DORewind());
    }

    private void AnimateClassification()
    {
        if (sequenceClassification != null)
            sequenceClassification.Kill();

        if (rotateTweenClassification != null)
            rotateTweenClassification.Kill();

        sequenceClassification = DOTween.Sequence();
        sequenceClassification.Append(textClassification.transform.DOScale(maxScale, duration).SetEase(easing));
        sequenceClassification.Append(textClassification.transform.DOScale(1, duration).SetEase(easing));

        rotateTweenClassification = textClassification.transform.DOLocalRotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, maxZRotation)), duration).SetEase(easing).OnComplete(() => transform.DORewind());
    }
    private void AnimateMultiplier()
    {
        if (sequenceMultiplier != null)
            sequenceMultiplier.Kill();

        if (rotateTweenMultiplier != null)
            rotateTweenMultiplier.Kill();

        sequenceMultiplier = DOTween.Sequence();
        sequenceMultiplier.Append(textMutliplier.transform.DOScale(maxScale, duration).SetEase(easing));
        sequenceMultiplier.Append(textMutliplier.transform.DOScale(1, duration).SetEase(easing));

        rotateTweenMultiplier = textMutliplier.transform.DOLocalRotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, maxZRotation)), duration).SetEase(easing).OnComplete(() => transform.DORewind());
    }

}
