using UnityEngine;

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public AudioClipData CurrentAudioClipData { get; set; }
}
