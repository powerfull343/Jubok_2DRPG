using UnityEngine;
using System.Collections;
using Mecro;

public class UI_GroceryStore_SubTradeUI : MonoBehaviour {

    /// <summary>
    /// true = buy , false = Sell
    /// </summary>
    private bool m_isBuy;

    private Item_Slot m_ItemTarget;
    public Item_Slot ItemTarget
    {
        get { return m_ItemTarget; }
        set { m_ItemTarget = value; }
    }

    [SerializeField]
    private UILabel m_TradeTitleText;
    [SerializeField]
    private UILabel m_AccessMenuText;
    [SerializeField]
    private UIScrollBar m_ItemCountScrollBar;
    [SerializeField]
    private UILabel m_ItemCountLabel;

    private GameObject_Extension m_OwnGameObjectInfo;

    private int m_CurrentItemCount = 1;
    private int m_MaxItemCount = 1;
    private bool m_isSliderClick = true;


    [SerializeField]
    private GameObject m_PlusCollider;
    [SerializeField]
    private GameObject m_MinusCollider;
    private delegate void PressEvent();

    void OnEnable()
    {
        Debug.Log("Onenable");
        StartCoroutine("ClickPressEvent");
    }

    void Start()
    {
        Debug.Log("Start");
        MecroMethod.CheckExistComponent<UILabel>(m_TradeTitleText);
        MecroMethod.CheckExistComponent<UIScrollBar>(m_ItemCountScrollBar);
        MecroMethod.CheckExistComponent<UILabel>(m_ItemCountLabel);
        MecroMethod.CheckExistObejct<GameObject>(m_PlusCollider);
        MecroMethod.CheckExistObejct<GameObject>(m_MinusCollider);
    }

    IEnumerator ClickPressEvent()
    {
        float fEventDelay = 0.5f;
        PressEvent PressingEvent = null;

        while (true)
        {
            if (!ColliderChecking(ref PressingEvent))
            {
                fEventDelay = 0.5f;
                PressingEvent = null;
                yield return new WaitForEndOfFrame();
            }

            if (Input.GetMouseButton(0) && PressingEvent != null)
            {
                fEventDelay *= 0.8f;
                if (fEventDelay <= 0.1f)
                    fEventDelay = 0.1f;

                PressingEvent();
            }

            yield return new WaitForSeconds(fEventDelay);
        }

        yield return null;
    }

    private bool ColliderChecking(ref PressEvent _PressEvent)
    {
        if (UICamera.hoveredObject == m_PlusCollider)
        {
            _PressEvent = PlusItemCount;
            return true;
        }
        else if (UICamera.hoveredObject == m_MinusCollider)
        {
            _PressEvent = MinusItemCount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HideAndShowTradeMenu()
    {
        if (!m_OwnGameObjectInfo)
        {
            m_OwnGameObjectInfo =
                MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
        }
        m_OwnGameObjectInfo.HideAndShow();
    }
    /// <summary>
    /// true = buy , false = Sell
    /// </summary>
    public void InitSubTradeMenu(Item_Slot _ItemTarget, bool _isBought)
    {
        m_ItemTarget = _ItemTarget;
        m_isBuy = _isBought;
        SetTitleText();
        SetMaxCount(_ItemTarget.ChildItem.ItemInfo.itemWeight);
        ResetSubTradeUI();

        int CurrentDepth = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject).depth;
        LobbyManager.LobbyController.GetInstance().SettingBlockPanel(CurrentDepth - 1);
    }

    private void SetTitleText()
    {
        m_TradeTitleText.text = (m_isBuy == true) ? "Buy" : "Sell";
        m_AccessMenuText.text = m_TradeTitleText.text;
    }

    private void SetMaxCount(float fItemWeight)
    {
        int nMoneyMaxCount = 0, nWeightMaxCount = 0;
        if (m_isBuy)
        {
            nMoneyMaxCount =
                DataController.GetInstance().InGameData.Money /
                    m_ItemTarget.ChildItem.ItemInfo.ItemValue;

            nWeightMaxCount =
                (int)((InventoryManager.GetInstance().InvenMaxWeight
                    - InventoryManager.GetInstance().InvenWeight) / 
                    fItemWeight);

            Debug.Log(nMoneyMaxCount);
            Debug.Log(nWeightMaxCount);

            m_MaxItemCount = nMoneyMaxCount > nWeightMaxCount ?
                nWeightMaxCount - 1: nMoneyMaxCount - 1;

        }
        else
            m_MaxItemCount = m_ItemTarget.ChildItem.ItemInfo.itemCount;
    }

    private void ResetSubTradeUI()
    {
        m_ItemCountScrollBar.value = 0f;

        float fBarSize = 1f / m_MaxItemCount;
        if (fBarSize <= 0.1f)
            fBarSize = 0.1f;

        m_ItemCountScrollBar.barSize = fBarSize;
        m_CurrentItemCount = 1;
        UpdateItemCountLabel();
    }

    public void UpdateItemCountLabel()
    {
        m_ItemCountLabel.text = m_CurrentItemCount.ToString();
    }

    public void UpdateSliderCtrlToCount()
    {
        if (!m_isSliderClick)
        {
            m_isSliderClick = true;
            return;
        }

        Debug.Log("UpdateSlider");
        m_CurrentItemCount = (int)(m_MaxItemCount * m_ItemCountScrollBar.value) + 1;
        UpdateItemCountLabel();
    }

    public void SetCannotValueToChangeMethod()
    {
        m_isSliderClick = false;
    }

    public void PlusItemCount()
    {
        Debug.Log("ClickButton");
        if (m_ItemCountScrollBar.value >= 1f)
        {
            Debug.Log("Value is full");
            return;
        }

        SetCannotValueToChangeMethod();
        ++m_CurrentItemCount;
        float fScrollBarValue = (float)m_CurrentItemCount / m_MaxItemCount;
        Debug.Log("m_CurrentItemCount : " + m_CurrentItemCount);
        Debug.Log("m_MaxItemCount : " + m_MaxItemCount);
        Debug.Log("fScrollBarValue : " + fScrollBarValue);

        m_ItemCountScrollBar.value = fScrollBarValue;
        UpdateItemCountLabel();
    }

    public void MinusItemCount()
    {
        Debug.Log("ClickButton"); 
        if (m_ItemCountScrollBar.value <= 0f)
        {
            Debug.Log("Value is Low");
            return;
        }

        SetCannotValueToChangeMethod();
        --m_CurrentItemCount;
        float fScrollBarValue = (float)m_CurrentItemCount / m_MaxItemCount;
        Debug.Log("m_CurrentItemCount : " + m_CurrentItemCount);
        Debug.Log("m_MaxItemCount : " + m_MaxItemCount);
        Debug.Log("fScrollBarValue : " + fScrollBarValue);

        m_ItemCountScrollBar.value = fScrollBarValue;
        UpdateItemCountLabel();
    }
    
    public void MinItemCount()
    {
        SetCannotValueToChangeMethod();
        m_ItemCountScrollBar.value = 0f;
        m_CurrentItemCount = 1;
        UpdateItemCountLabel();
    }

    public void MaxItemCount()
    {
        SetCannotValueToChangeMethod();
        m_ItemCountScrollBar.value = 1f;
        m_CurrentItemCount = m_MaxItemCount + 1;
        UpdateItemCountLabel();
    }

    public void AccessTrade()
    {
        if(m_isBuy)
            InventoryManager.GetInstance().BuyItem(m_ItemTarget, m_CurrentItemCount);
        else
            InventoryManager.GetInstance().SellItem(m_ItemTarget, m_CurrentItemCount);

        HideSubTradeUI();
    }

    public void CancelTrade()
    {
        HideSubTradeUI();
    }

    private void HideSubTradeUI()
    {
        ResetSubTradeUI();
        LobbyManager.LobbyController.GetInstance().HidingBlockPanel();
        HideAndShowTradeMenu();
    }
}
