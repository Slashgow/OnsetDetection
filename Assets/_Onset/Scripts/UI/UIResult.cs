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
        Debug.Log("try register on song end event");
        if(FindFirstObjectByType<ScoreManager>() != null)
        {
            Debug.Log("Register score manager on end song");
            ScoreManager.Instance.OnSongEnd -= ScoreManager_OnSondEnd;
            ScoreManager.Instance.OnSongEnd += ScoreManager_OnSondEnd;
        }
    }
    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(Restart);
        backButton.onClick.RemoveListener(GoToMainMenu);
    }

    private void ScoreManager_OnSondEnd(ScoreData scoreData)
    {
        Debug.Log("score manager on song end");
        UIMenuController.Instance.ShowOnly(MenuType.RESULT);

        excellentText.text = scoreData.GetScoreClassified(NoteHitClassification.EXCELLENT).NumberOfHit.ToString();
        greatText.text = scoreData.GetScoreClassified(NoteHitClassification.GREAT).NumberOfHit.ToString();
        earlyText.text = scoreData.GetScoreClassified(NoteHitClassification.EARLY).NumberOfHit.ToString();
        lateText.text = scoreData.GetScoreClassified(NoteHitClassification.LATE).NumberOfHit.ToString();
        missText.text = scoreData.GetScoreClassified(NoteHitClassification.MISS).NumberOfHit.ToString();

        percentOfNoteHitText.text = $"{scoreData.PercentageOfNoteHit.ToString()} %";
        scoreText.text = scoreData.Score.ToString();
        highScoreText.text = scoreData.HighScore.ToString();
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
