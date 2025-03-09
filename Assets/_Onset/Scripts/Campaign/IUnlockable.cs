public interface IUnlockable
{
    public void Unlock(int totalScore);
    public int TotalScoreToUnlock { get; }
    public bool IsUnlocked { get; }
}
