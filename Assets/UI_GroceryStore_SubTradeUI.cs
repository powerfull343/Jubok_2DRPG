using UnityEngine;
using System.Collections;
using Mecro;

public class UI_GroceryStore_SubTradeUI : MonoBehaviour {

    /// <summary>
    /// true = buy , false = Sell
    /// </summary>
    private bool m_isKindOfTrade;

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

    void Start()
    {
        MecroMethod.CheckExistComponent<UILabel>(m_TradeTitleText);
        MecroMethod.CheckExistComponent<UIScrollBar>(m_ItemCountScrollBar);
        MecroMethod.CheckExistComponent<UILabel>(m_ItemCountLabel);
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
        m_isKindOfTrade = _isBought;
        SetTitleText();
        SetMaxCount();
        ResetSubTradeUI();

        int CurrentDepth = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject).depth;
        LobbyManager.LobbyController.GetInstance().SettingBlockPanel(CurrentDepth - 1);
    }

    private void SetTitleText()
    {
        m_TradeTitleText.text = (m_isKindOfTrade == true) ? "Buy" : "Sell";
        m_AccessMenuText.text = m_TradeTitleText.text;
    }

    private void SetMaxCount()
    {
        if (m_isKindOfTrade)
        {
            m_MaxItemCount =
                DataController.GetInstance().InGameData.Money /
                    m_ItemTarget.ChildItem.ItemInfo.ItemValue;
        }
        else
            m_MaxItemCount = m_ItemTarget.ChildItem.ItemInfo.itemCount;
    }

    private void ResetSubTradeUI()
    {
        m_ItemCountScrollBar.value = 0f;
        m_CurrentItemCount = 1;
        UpdateItemCountLabel();
    }

    public void UpdateItemCountLabel()
    {
        m_ItemCountLabel.text = m_CurrentItemCount.ToString();
    }

    public void UpdateSliderCtrlToCount()
    {
        m_CurrentItemCount = (int)(m_MaxItemCount * m_ItemCountScrollBar.value) + 1;
        if (m_CurrentItemCount == m_MaxItemCount)
        {
            m_ItemCountScrollBar.value = 1f;
            m_CurrentItemCount = m_MaxItemCount;
        }

        UpdateItemCountLabel();
    }

    public void PlusItemCount()
    {
        if (m_ItemCountScrollBar.value >= 1f)
            return;

        ++m_CurrentItemCount;
        m_ItemCountScrollBar.value = (float)(m_CurrentItemCount - 1) / m_MaxItemCount;
        UpdateItemCountLabel();
    }

    public void MinusItemCount()
    {
        if (m_ItemCountScrollBar.value <= 0f)
            return;

        --m_CurrentItemCount;
        m_ItemCountScrollBar.value = (float)(m_CurrentItemCount - 1) / m_MaxItemCount;
        UpdateItemCountLabel();
    }
    
    public void MinItemCount()
    {
        m_ItemCountScrollBar.value = 0f;
        m_CurrentItemCount = 1;
        UpdateItemCountLabel();
    }

    public void MaxItemCount()
    {
        m_ItemCountScrollBar.value = 1f;
        m_CurrentItemCount = m_MaxItemCount;
        UpdateItemCountLabel();
    }

    public void AccessTrade()
    {
        if(m_isKindOfTrade)
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
