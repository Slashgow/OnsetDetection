using UnityEngine;
using UnityEngine.UI;

public class UIMenuMain : MonoBehaviour
{
    [SerializeField]
    private Button quitButton;

    public void Start()
    {
        quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        quitButton.onClick.RemoveListener(Quit);
    }

    private void Quit()
    {
        GameManager.Instance.Quit();
    }
}
