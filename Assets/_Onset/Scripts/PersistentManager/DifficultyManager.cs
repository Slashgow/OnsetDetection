using UnityEngine;

public class DifficultyManager : PersistentMonoSingleton<DifficultyManager>
{
    [SerializeField]
    private Difficulty defaultDifficulty;

    public const string DIFFICULTY_ID = "DIFFICULTY_ID";
    public Difficulty CurrentDifficulty { get; private set; }
    public int CurrentDifficultyIndex { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey(DIFFICULTY_ID))
            CurrentDifficultyIndex = PlayerPrefs.GetInt(DIFFICULTY_ID);

        SetDifficulty(CurrentDifficultyIndex);
    }

    public void SetDifficulty(int difficultyIndex)
    {
        CurrentDifficultyIndex = difficultyIndex;
        switch (difficultyIndex)
        {
            case 0:
                CurrentDifficulty = Difficulty.BEGINNER;
                break;
            case 1:
                CurrentDifficulty = Difficulty.MEDIUM;
                break;
            case 2:
                CurrentDifficulty = Difficulty.HARD;
                break;
            case 3:
                CurrentDifficulty = Difficulty.EXTREME;
                break;
            default:
                CurrentDifficulty = Difficulty.BEGINNER;
                break;
        }
        PlayerPrefs.SetInt(DIFFICULTY_ID, difficultyIndex);
    }
}
