using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField]
    private MenuType menuType;

    public MenuType MenuType => menuType;

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
