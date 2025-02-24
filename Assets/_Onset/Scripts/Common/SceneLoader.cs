using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentMonoSingleton<SceneLoader>
{
    public event Action<int> OnEndLoadScene;

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(LoadAsyncScene(buildIndex));        
    }

    IEnumerator LoadAsyncScene(int buildIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnEndLoadScene?.Invoke(buildIndex);
    }
}
