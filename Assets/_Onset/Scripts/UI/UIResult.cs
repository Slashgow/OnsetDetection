using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText, highScoreText;

    [SerializeField]
    private TextMeshProUGUI excellentText, greatText, earlyText, lateText, missText;

    [SerializeField]
    private TextMeshProUGUI percentOfNoteHitText;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button backButton;

    private void Start()
    {
        restartButton.onClick.AddListener(Restart);
        backButton.onClick.AddListener(GoToMainMenu);

        if(FindFirstObjectByType<ScoreManager>() != null)
        {
            ScoreManager.Instance.OnSondEnd -= ScoreManager_OnSondEnd;
            ScoreManager.Instance.OnSondEnd += ScoreManager_OnSondEnd;
        }
    }
    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(Restart);
        backButton.onClick.RemoveListener(GoToMainMenu);
    }

    private void ScoreManager_OnSondEnd(ScoreData scoreData)
    {
        UIMenuController.Instance.ShowOnly(MenuType.RESULT);

        excellentText.text = scoreData.GetScoreClassified(NoteHitClassification.EXCELLENT).NumberOfHit.ToString();
        greatText.text = scoreData.GetScoreClassified(NoteHitClassification.GREAT).NumberOfHit.ToString();
        earlyText.text = scoreData.GetScoreClassified(NoteHitClassification.EARLY).NumberOfHit.ToString();
        lateText.text = scoreData.GetScoreClassified(NoteHitClassification.LATE).NumberOfHit.ToString();
        missText.text = scoreData.GetScoreClassified(NoteHitClassification.MISS).NumberOfHit.ToString();

        percentOfNoteHitText.text = scoreData.PercentageOfNoteHit.ToString();
        scoreText.text = scoreData.Score.ToString();
    }

    private void Restart()
    {
        SceneLoader.Instance.LoadScene(1);
    }

    private void GoToMainMenu()
    {
        SceneLoader.Instance.LoadScene(0);
    }

}
