using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMenuController : MonoSingleton<UIMenuController>
{
    [SerializeField]
    private MenuType startupMenu;

    [SerializeField]
    private List<UIMenu> menus;

    private void Start()
    {
        ShowOnly(startupMenu);
    }
    public void Back()
    {
        Debug.Log($"is in game : {GameManager.Instance.IsInGame}");
        //if(!GameManager.Instance.IsInGame)
            ShowOnly(MenuType.MAIN);
        //else
        //    ShowOnly(MenuType.IN_GAME);
    }

    public void ShowCredit() => ShowOnly(MenuType.CREDIT);
    public void ShowOptions() => ShowOnly(MenuType.OPTIONS);
    public void ShowChooseSong() => ShowOnly(MenuType.CHOOSE_SONG);
    public void ShowImportSong() => ShowOnly(MenuType.IMPORT_SONG);

    public void ShowMenu() => ShowOnly(MenuType.MAIN);

    public void ShowOnly(MenuType menuType)
    {
        if(menuType != MenuType.IN_GAME)
            GameManager.Instance.Pause();
        else
            GameManager.Instance.Resume();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        HideAllMenus();
        menus.First(menu => menu.MenuType == menuType).Show();
    }

    public void HideAllMenus() => menus.ForEach(menu => menu.Hide());
}
