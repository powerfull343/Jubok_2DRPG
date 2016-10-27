using UnityEngine;
using System.Collections;

public class UI_GroceryStore_Func : MonoBehaviour {

    private UIPanel OwnPanel;

    [SerializeField]
    private GameObject_Extension m_InventoryObject;
    [SerializeField]
    private GameObject_Extension m_SellerItemListGameObject;
    //[SerializeField]
    //private GameObject_Extension m_CursorManager;

    void OnEnable()
    {
        Invoke("InitGroceryFunc", 1.5f);
    }

    void Start()
    {
        Mecro.MecroMethod.ShowSceneLogConsole("Start", true);
        OwnPanel = Mecro.MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);
        OwnPanel.alpha = 0f;
        Mecro.MecroMethod.ShowSceneLogConsole("OwnPanel : " + (OwnPanel == null), 
            true);

        Mecro.MecroMethod.CheckExistComponent<GameObject_Extension>(m_InventoryObject);
        Mecro.MecroMethod.CheckExistComponent<GameObject_Extension>(m_SellerItemListGameObject);
    }

    void Update()
    {
        if (OwnPanel.alpha < 1f && OwnPanel.alpha > 0f)
            Mecro.MecroMethod.ShowSceneLogConsole(
                "GroceryPanel.Alpha : " + OwnPanel.alpha.ToString("N3"),
            true);
    }

    void InitGroceryFunc()
    {
        Mecro.MecroMethod.ShowSceneLogConsole("initGroceryFunc", true);
        GroceryStoreFuncsHideAndShow();
        TweenAlpha.Begin(this.gameObject, 1f, 1f);
    }

    public void EndGroceryFunc()
    {
        TweenAlpha.Begin(this.gameObject, 1f, 0f);
        Invoke("GroceryStoreFuncsHideAndShow", 1f);
    }

    void GroceryStoreFuncsHideAndShow()
    {
        PlayerDataManager.GetInstance().InvenInst_HideAndShow(
            m_InventoryObject.transform);
        m_InventoryObject.HideAndShow();
        m_SellerItemListGameObject.HideAndShow();
    }
    
}
