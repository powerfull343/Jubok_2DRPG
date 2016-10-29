using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;
using LobbyButtonFunc;

public class UI_Inventory_Functions : MonoBehaviour {

    [SerializeField]
    private UILabel m_Weightlabel;

    private UI_Inventory_DeatilSquare m_DetailWindow;
    public UI_Inventory_DeatilSquare DetailWindow
    {
        get { return m_DetailWindow; }
    }

    private UI_Inventory_LeftItemWindow m_LeftItemWindow;
    public UI_Inventory_LeftItemWindow LeftItemWindow
    { get { return m_LeftItemWindow; } }

    [SerializeField]
    private UI_Inventory_CursorManager m_CursorManager;
    public UI_Inventory_CursorManager CursorManager
    { get { return m_CursorManager; } }

    //Memory Pool
    private GameObject m_SelectedEdgeSquare;

    // Use this for initialization
    void Start () {
        MecroMethod.CheckExistComponent<UILabel>(m_Weightlabel);
        MecroMethod.CheckExistComponent<UI_Inventory_CursorManager>(m_CursorManager);

        GameObject DetailItemWindow = Instantiate(Resources.Load("UIPanels/Inventory - ItemDetailWindow") as GameObject);
        MecroMethod.SetPartent(DetailItemWindow.transform, this.transform);
        m_DetailWindow = MecroMethod.CheckGetComponent<UI_Inventory_DeatilSquare>(DetailItemWindow);

        GameObject LeftItemWindow = Instantiate(Resources.Load("UIPanels/Inventory - LeftItemWindow") as GameObject);
        MecroMethod.SetPartent(LeftItemWindow.transform, this.transform);
        LeftItemWindow.transform.localPosition = new Vector3(-540f, 0f, 0f);
        m_LeftItemWindow = MecroMethod.CheckGetComponent<UI_Inventory_LeftItemWindow>(LeftItemWindow);


        InitWeightLabel();
        m_SelectedEdgeSquare = Instantiate(Resources.Load("ItemIcons/Sprite - SelectedArea") as GameObject);
        m_SelectedEdgeSquare.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (m_DetailWindow && m_DetailWindow.gameObject.activeSelf)
            m_DetailWindow.gameObject.SetActive(false);
        if (m_LeftItemWindow && m_LeftItemWindow.gameObject.activeSelf)
            m_LeftItemWindow.gameObject.SetActive(false);
        if (m_SelectedEdgeSquare && m_SelectedEdgeSquare.gameObject.activeSelf)
            m_SelectedEdgeSquare.gameObject.SetActive(false);
    }

   

    private void InitWeightLabel()
    {
        //Max Weight Count
        PlayerData LoadedPlayData = DataController.GetInstance().InGameData;

        InventoryManager.GetInstance().InvenMaxWeight = 
          100f + LoadedPlayData.tStat.Str * 10f;

        Debug.Log("Loaded Inventory Data : " + LoadedPlayData.Inventory.Count);

        for (int i = 0; i < LoadedPlayData.Inventory.Count; ++i)
        {
            InventoryManager.GetInstance().InvenWeight +=
                InventoryManager.GetInstance().CalcItemWeight(LoadedPlayData.Inventory.Values.ToList()[i]);
        }

        UpdateWeightLabel();
    }

    

    private void SetWeight(Item_Interface_Comp SelectedItem, int nItemCount, bool isGetItem)
    {
        int nPlustoMinus = (isGetItem == true) ? 1 : -1;

        //소지될 무게 감소
        float fChangeAmount = (SelectedItem.ItemInfo.itemWeight * nItemCount * nPlustoMinus);
        InventoryManager.GetInstance().InvenWeight += fChangeAmount;

        //"N1" -> 소수점 1자리까지만 출력함.
        string strWeight = InventoryManager.GetInstance().InvenWeight.ToString("N1");
        InventoryManager.GetInstance().InvenWeight = float.Parse(strWeight);

        UpdateWeightLabel();
    }

    private void UpdateWeightLabel()
    {
        m_Weightlabel.text = InventoryManager.GetInstance().InvenWeight.ToString() + " / " +
            InventoryManager.GetInstance().InvenMaxWeight.ToString();
    }

    public void ShowDetailSquare(Item_Slot SlotTarget)
    {
        if (!m_SelectedEdgeSquare)
            Debug.LogError(m_SelectedEdgeSquare == null);

        m_SelectedEdgeSquare.SetActive(false);
        UIWidget SelectedItemSlotWidget = m_SelectedEdgeSquare.GetComponent<UIWidget>();
        SelectedItemSlotWidget.depth = 4;

        m_SelectedEdgeSquare.transform.parent = SlotTarget.transform;
        m_SelectedEdgeSquare.transform.localPosition = Vector3.zero;
        m_SelectedEdgeSquare.transform.localScale = Vector3.one;
        m_SelectedEdgeSquare.transform.rotation = Quaternion.identity;
        m_SelectedEdgeSquare.SetActive(true);

        if (LobbyController.GetInstance().OpenedPanel ==
            IDSUBPANEL.PANELID_GROCERYSTORE)
        {
            UI_GroceryStore_SellList.GetInstance().SelectedItemSlot =
                SlotTarget;
        }
    }

    public string BuyItem(Item_Slot SelectedItemSlot, int nBuyItemCount)
    {
        string StatusMessage = string.Empty;
        Debug.Log("nBuyItemCount : " + nBuyItemCount);
        //슬롯에 존재하는 아이템을 가지고온다.
        Item_Interface_Comp SelectedItem =
            SelectedItemSlot.ChildItem;

        //아이템이 존재하지 않을시 실행하지 않음.
        if (SelectedItem == null)
        {
            StatusMessage = "[ff0000]System Error) Not Have Item[-]";
            Debug.Log(StatusMessage);
            return StatusMessage;
        }

        //슬롯에 있는 아이템과 현재 소지금을 비교한다.
        if (!LobbyController.GetInstance().UpperStatusPanel.CompareMoney(
            -SelectedItem.ItemInfo.ItemValue * nBuyItemCount))
        {
            StatusMessage = "[ff0000]Cannot Enough Money[-]";
            Debug.Log(StatusMessage);
            return StatusMessage;
        }

        //무게 제한을 검사한다.
        if (!InventoryManager.GetInstance().CompareWeight(SelectedItem.ItemInfo.itemWeight * nBuyItemCount))
        {
            StatusMessage = "Cannot have item, it's so Heavy";
            Debug.Log(StatusMessage);
            return StatusMessage;
        }
        
        //소지가짓수 체크
        if (InventoryManager.GetInstance().ItemSlotList.Last().ChildItem)
        {
            StatusMessage = "Cannot have item, Bag is Full";
            Debug.Log(StatusMessage);
            return StatusMessage;
        }

        //실질적 소지금 감소
        int nVariationMoney = -(SelectedItem.ItemInfo.ItemValue * nBuyItemCount);

        DataController.GetInstance().InGameData.Money += nVariationMoney;

        //소지금 감소 표시
        LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(nVariationMoney);

        //소지될 무게 추가
        SetWeight(SelectedItem, nBuyItemCount, true);

        Debug.Log("Additem");
        //아이템 추가
        InventoryManager.GetInstance().CreateItem(SelectedItem.ItemInfo, nBuyItemCount);
        ++InventoryManager.ItemCreatePosition;
        return StatusMessage;
    }

    public string SellItem(Item_Slot SelectedItemSlot, int nSellItemCount)
    {
        string StatusMessage = string.Empty;

        //슬롯에 존재하는 아이템을 가지고온다.
        Item_Interface_Comp SelectedItem =
            SelectedItemSlot.transform.FindChild("Sprite - ItemIcon(Clone)").GetComponent<Item_Interface_Comp>();

        //아이템이 존재하지 않을시 실행하지 않음.
        if (SelectedItem == null)
        {
            StatusMessage = "System Error) Not Have Item";
            Debug.Log(StatusMessage);
            return StatusMessage;
        }

        int nGetPrice = (int)SelectedItem.ItemInfo.ItemValue * 3 / 10;
        //실질적 소지금 증가
        int nVariationMoney = nGetPrice * nSellItemCount;

        DataController.GetInstance().InGameData.Money += nVariationMoney;

        //소지금 증가
        LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(nVariationMoney);

        --InventoryManager.ItemCreatePosition;
        SetWeight(SelectedItem, nSellItemCount, false);
        //아이템 삭제
        InventoryManager.GetInstance(
            ).DestroyItem(SelectedItem, nSellItemCount);

        return StatusMessage;
    }

    public void ControllCursorManager()
    {
        if(CursorManager.gameObject.activeSelf)
            CursorManager.gameObject.SetActive(false);
        else
            CursorManager.gameObject.SetActive(true);
    }

    public void OpenItemDetailWindow(Item_Slot _SelectedSlot)
    {
        if (!_SelectedSlot.ChildItem)
        {
            Debug.Log("Cannot Find Child Item");
            return;
        }
        m_DetailWindow.gameObject.SetActive(false);

        m_DetailWindow.OpenDetailItemInfo(_SelectedSlot);
        m_DetailWindow.gameObject.SetActive(true);
        m_DetailWindow.m_OwnExtension.enabled = true;
    }
}
