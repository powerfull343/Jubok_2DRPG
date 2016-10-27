using UnityEngine;
using System.Collections;
using Mecro;

public class BlcakSmithmanager : MonoBehaviour {

    private BLACKSMITISUBMENUID SubMenuID;

    public GameObject[] SubMenu;
    public GameObject MainMenu;

    void OnEnable()
    {
        if (SubMenu.Length == 0)
            Debug.Log("SubMenu Objects Null");

        if (MainMenu == null)
            Debug.Log(MainMenu.name + "is null");

    }

    public void ClickBuy()
    {
        SubMenuID = BLACKSMITISUBMENUID.ID_BUY;
        OpenSubMenu();
    }

    public void ClickUpgrade()
    {
        SubMenuID = BLACKSMITISUBMENUID.ID_UPGRADE;
        OpenSubMenu();
    }

    public void ClickSocket()
    {
        SubMenuID = BLACKSMITISUBMENUID.ID_SOCKET;
        OpenSubMenu();
    }
    
    public void OpenMainMenu()
    {
        SubMenu[(int)SubMenuID].SetActive(false);
        MainMenu.SetActive(true);
        MecroMethod.CheckGetComponent<UIPanel>(MainMenu).alpha = 1;
    }

    public void OpenSubMenu()
    {
        SubMenu[(int)SubMenuID].SetActive(true);
        MainMenu.SetActive(false);
    }
}
