using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMenuController : MonoSingleton<UIMenuController>
{
    [SerializeField]
    private List<UIMenu> menus;

    private void Start()
    {
        ShowOnly(MenuType.MAIN);
    }
    public void Back()
    {
        ShowOnly(MenuType.MAIN);
    }

    public void ShowCredit() => ShowOnly(MenuType.CREDIT);
    public void ShowSounds() => ShowOnly(MenuType.VOLUME);
    public void ShowChooseSong() => ShowOnly(MenuType.CHOOSE_SONG);
    public void ShowImportSong() => ShowOnly(MenuType.IMPORT_SONG);

    public void ShowMenu() => ShowOnly(MenuType.MAIN);

    public void ShowOnly(MenuType menuType)
    {
        HideAllMenus();
        menus.First(menu => menu.MenuType == menuType).Show();
    }

    public void HideAllMenus() => menus.ForEach(menu => menu.Hide());
}
