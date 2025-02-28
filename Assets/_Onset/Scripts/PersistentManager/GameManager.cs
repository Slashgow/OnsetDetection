using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public bool IsPaused {  get; private set; }
    public bool IsInGame { get; private set; }
    public AudioClipData CurrentAudioClipData { get; set; }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
            IsInGame = false;

        SceneLoader.Instance.OnEndLoadScene -= SceneLoader_OnEndLoadScene;
        SceneLoader.Instance.OnEndLoadScene += SceneLoader_OnEndLoadScene;
    }

    private void SceneLoader_OnEndLoadScene(int buildIndex)
    {
        if(buildIndex > 0)
            IsInGame = true;
        else
            IsInGame= false;
        Resume();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
    }

    public void Resume()
    {
        IsPaused=false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
