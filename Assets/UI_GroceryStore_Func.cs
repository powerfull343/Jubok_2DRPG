using UnityEngine;
using System.Collections;

public class UI_GroceryStore_Func : MonoBehaviour {

    private UIPanel OwnPanel;

    [SerializeField]
    private GameObject_Extension m_InvenToryObject;
    [SerializeField]
    private GameObject_Extension m_SellerItemListGameObject;
    [SerializeField]
    private GameObject_Extension m_CursorManager;

    void Start()
    {
        OwnPanel = Mecro.MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);
        OwnPanel.alpha = 0f;

        Mecro.MecroMethod.CheckExistComponent<GameObject_Extension>(m_InvenToryObject);
        Mecro.MecroMethod.CheckExistComponent<GameObject_Extension>(m_SellerItemListGameObject);
        Mecro.MecroMethod.CheckExistComponent<GameObject_Extension>(m_CursorManager);
    }
    
    void OnEnable()
    {
        Invoke("InitGroceryFunc", 1.5f);
    }

    void InitGroceryFunc()
    {
        GroceryStroeFuncsHideAndShow();
        TweenAlpha.Begin(this.gameObject, 1f, 1f);
    }

    public void EndGroceryFunc()
    {
        TweenAlpha.Begin(this.gameObject, 1f, 0f);
        Invoke("GroceryStroeFuncsHideAndShow", 1f);
    }

    void GroceryStroeFuncsHideAndShow()
    {
        PlayerDataManager.GetInstance().InvenInst_HideAndShow(
            m_InvenToryObject.transform);
        m_InvenToryObject.HideAndShow();
        m_SellerItemListGameObject.HideAndShow();
        m_CursorManager.HideAndShow();
    }
    
}
