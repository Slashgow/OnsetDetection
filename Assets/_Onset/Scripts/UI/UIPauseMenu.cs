using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button restartButton, backToMainMenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(Restart);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(Restart);
        backToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void BackToMainMenu() => SceneLoader.Instance.LoadScene(0);

    private void Restart() => SceneLoader.Instance.LoadScene(1);
}
