using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : 
    Singleton<InventoryManager> {

    [SerializeField]
    private UILabel m_Weightlabel;
    
    private float m_fItemWeight;
    private float m_fItemMaxWeight;

    [SerializeField]
    private Transform m_GridTransform;

    private List<Item_Slot> m_ItemSlotList
        = new List<Item_Slot>();

    private int m_nItemCount = 0;

    [SerializeField]
    private UI_Inventory_DeatilSquare m_DetailWindow;
    public UI_Inventory_DeatilSquare DetailWindow
    {
        get { return m_DetailWindow; }
        set { m_DetailWindow = value; }
    }

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_Weightlabel);
        Mecro.MecroMethod.CheckExistComponent<Transform>(m_GridTransform);
        Mecro.MecroMethod.CheckExistComponent<UI_Inventory_DeatilSquare>(m_DetailWindow);
        InitBackPackMemory();
        InitWeightLabel();
        InitBackPackItems();
    }

    void InitBackPackMemory()
    {
        Item_Slot SlotInstance;
        for (int i = 0; i < m_GridTransform.childCount; ++i)
        {
            Transform SecendGridTrans = m_GridTransform.GetChild(i);
            for (int j = 0; j < SecendGridTrans.childCount; ++j)
            {
                SlotInstance =
                    Mecro.MecroMethod.CheckGetComponent<Item_Slot>(SecendGridTrans.GetChild(j));
                SlotInstance.RowIndex = i;
                m_ItemSlotList.Add(SlotInstance);
            }
        }
    }

    void InitWeightLabel()
    {
        //Max Weight Count
        PlayerData LoadedPlayData = DataController.GetInstance().InGameData;

        m_fItemMaxWeight = 100f +
            LoadedPlayData.tStat.Str * 10f;

        Debug.Log("Loaded Inventory Data : " + LoadedPlayData.Inventory.Count);

        for (int i = 0; i < LoadedPlayData.Inventory.Count; ++i)
        {
            m_fItemWeight += 
                CalcItemWeight(LoadedPlayData.Inventory.Values.ToList()[i]);
        }

        UpdateWeightLabel();
    }

    void InitBackPackItems()
    {
        GameObject ItemObject;
        Item_Interface_Comp newItemInfo;

        Dictionary<string, List<Item_Interface>> ItemList =
            ItemManager.GetInstance().InventoryItems;
        List<Item_Interface> KindofItems;
        for(int i = 0; i < ItemList.Count; ++i)
        {
            KindofItems = ItemList.Values.ToList()[i];
            for(int j = 0; j < KindofItems.Count; ++j, ++m_nItemCount)
            {
                ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(KindofItems[j]);
                ItemObject.SetActive(false);
                newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
                newItemInfo.ItemInfo = KindofItems[j];
                ItemObject.transform.parent = m_ItemSlotList[m_nItemCount].transform;
                ItemObject.transform.localScale = Vector3.one;
                ItemObject.transform.localPosition = Vector3.zero;
                ItemObject.SetActive(true);
                m_ItemSlotList[m_nItemCount].ChildItem = newItemInfo;
            }
        }
    }

    private void SetWeight(Item_Interface_Comp SelectedItem, int nItemCount, bool isGetItem)
    {
        int nPlustoMinus = (isGetItem == true) ? 1 : -1;

        //소지될 무게 감소
        float fChangeAmount = (SelectedItem.ItemInfo.itemWeight * nItemCount * nPlustoMinus);
        m_fItemWeight += fChangeAmount;

        //"N1" -> 소수점 1자리까지만 출력함.
        string strWeight = m_fItemWeight.ToString("N1");
        m_fItemWeight = float.Parse(strWeight);

        UpdateWeightLabel();
    }

    private void UpdateWeightLabel()
    {
        Debug.Log("Update");
        m_Weightlabel.text = m_fItemWeight.ToString() + " / " + m_fItemMaxWeight.ToString();
    }


    float CalcItemWeight(List<Item_Interface> ItemInfo)
    {
        float fWeightAmount = 0f;

        foreach(Item_Interface item in ItemInfo)
        {
            Debug.Log("ItemName : " + item.itemName + 
                " /  ItemWeight : " + item.itemWeight +
                " /  ItemCount : " + item.itemCount);
            fWeightAmount += (item.itemWeight * item.itemCount);
        }
        return fWeightAmount;
    }

    public bool CompareWeight(float fWeightSize)
    {
        if (m_fItemWeight + fWeightSize > m_fItemMaxWeight)
        {
            //Debug.Log(m_fItemWeight);
            //Debug.Log(fWeightSize);
            //Debug.Log(m_fItemMaxWeight);
            return false;
        }

        return true;
    }

    public void BuyItem(Item_Slot SelectedItemSlot, int nBuyItemCount)
    {
        //슬롯에 존재하는 아이템을 가지고온다.
        Item_Interface_Comp SelectedItem =
            SelectedItemSlot.ChildItem;

        //아이템이 존재하지 않을시 실행하지 않음.
        if (SelectedItem == null)
        {
            Debug.Log("Not Have Item");
            return;
        }

        //슬롯에 있는 아이템과 현재 소지금을 비교한다.
        if (!LobbyManager.LobbyController.GetInstance().UpperStatusPanel.CompareMoney(
            -SelectedItem.ItemInfo.ItemValue * nBuyItemCount))
        {
            Debug.Log("Not Have Money");
            return;
        }

        //무게 제한을 검사한다.
        if (!CompareWeight(SelectedItem.ItemInfo.itemWeight * nBuyItemCount))
        {
            Debug.Log("Heavy");
            return;
        }

        //소지금 감소
        LobbyManager.LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(-(SelectedItem.ItemInfo.ItemValue * nBuyItemCount));

        //소지될 무게 추가
        SetWeight(SelectedItem, nBuyItemCount, true);

        //소지가짓수 체크
        if(m_ItemSlotList.Last().ChildItem)
        {
            Debug.Log("Full Bag");
            return;
        }

        Debug.Log("Additem");
        //아이템 추가
        AddItem(SelectedItem, nBuyItemCount);
    }

    public void SellItem(Item_Slot SelectedItemSlot, int nSellItemCount)
    {
        //슬롯에 존재하는 아이템을 가지고온다.
        Item_Interface_Comp SelectedItem =
            SelectedItemSlot.transform.FindChild("Sprite - ItemIcon(Clone)").GetComponent<Item_Interface_Comp>();

        //아이템이 존재하지 않을시 실행하지 않음.
        if (SelectedItem == null)
        {
            Debug.Log("Not Have Item");
            return;
        }

        int nGetPrice = (int)SelectedItem.ItemInfo.ItemValue * 3 / 10;

        //소지금 증가
        LobbyManager.LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(nGetPrice * nSellItemCount);

        SetWeight(SelectedItem, nSellItemCount, false);
        //아이템 삭제
        DeleteItem(SelectedItem, nSellItemCount);
    }
    private void AddItem(Item_Interface_Comp ItemComp, int nItemCount)
    {
        GameObject ItemObject = null;
        List<Item_Interface> KindofItem;
        Item_Interface_Comp newItemInfo;

        for (int i = 0; i < m_ItemSlotList.Count; ++i)
        {
            if (!m_ItemSlotList[i].ChildItem)
            {
                m_nItemCount = i;
                Debug.Log(m_nItemCount);
                break;
            }
        }

        //현재 아이템이 목록에 없는경우
        if (!DataController.GetInstance().InGameData.Inventory.ContainsKey(ItemComp.ItemInfo.itemName))
        {
            //DataController에 추가한다.
            KindofItem = new List<Item_Interface>();
            KindofItem.Add(ItemComp.ItemInfo);
            DataController.GetInstance().InGameData.Inventory.Add(
                ItemComp.ItemInfo.itemName, KindofItem);

            //InventoryManager에 아이템 목록을 추가한다.
            ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(ItemComp.ItemInfo);
            ItemObject.SetActive(false);
            newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
            newItemInfo.ItemInfo = ItemComp.ItemInfo;
            newItemInfo.ItemInfo.itemCount = nItemCount;

            ItemObject.transform.parent = m_ItemSlotList[m_nItemCount].transform;
            ItemObject.transform.localScale = Vector3.one;
            ItemObject.transform.localPosition = Vector3.zero;
            ItemObject.SetActive(true);
            m_ItemSlotList[m_nItemCount].ChildItem = newItemInfo;
        }
        else
        {
            KindofItem = DataController.GetInstance().InGameData.Inventory[ItemComp.ItemInfo.itemName];
            //장비가 아닐 경우에는 갯수만 추가하고 그렇지 않을 경우 공간을 새로 만든다.
            if (ItemComp.ItemInfo.itemType != ITEMTYPEID.ITEM_EQUIP)
                KindofItem[0].itemCount += nItemCount;
            else
            {
                KindofItem.Add(ItemComp.ItemInfo);
                ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(ItemComp.ItemInfo);
                ItemObject.SetActive(false);
                newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
                newItemInfo.ItemInfo = ItemComp.ItemInfo;
                ItemObject.transform.parent = m_ItemSlotList[m_nItemCount].transform;
                ItemObject.transform.localScale = Vector3.one;
                ItemObject.transform.localPosition = Vector3.zero;
                ItemObject.SetActive(true);
                m_ItemSlotList[m_nItemCount].ChildItem = newItemInfo;
            }
        }
        
        DataController.GetInstance().Save();
    }

    private void DeleteItem(Item_Interface_Comp ItemComp, int nSellItemCount)
    {
        List<Item_Interface> KindofItem;
        if (!DataController.GetInstance().InGameData.Inventory.ContainsKey(ItemComp.ItemInfo.itemName))
            Debug.LogError("Cannot find item Object");

        string ItemKey = ItemComp.ItemInfo.itemName;
        KindofItem = 
            DataController.GetInstance().InGameData.Inventory[ItemComp.ItemInfo.itemName];

        Item_Interface FindObject = KindofItem.Find
        (
            delegate (Item_Interface item)
            {
                return item == ItemComp.ItemInfo;
            }
        );

        if (FindObject == null)
        {
            Debug.LogError("Warning Item is Cannot existing");
            return;
        }

        FindObject.itemCount -= nSellItemCount;

        //갯수가 1개 이상 남아있을시 여기서 연산을 끝낸다.
        if (FindObject.itemCount >= 1)
            return;

        //갯수가 0개가 되었을시 연산을 지속한다.
        KindofItem.Remove(FindObject);
        Item_Slot ParentSlot = ItemComp.transform.parent.GetComponent<Item_Slot>();
        if(ParentSlot == null)
        {
            Debug.Log("Cannot Find " + ParentSlot.name);
            return;
        }

        ParentSlot.ChildItem = null;
        Destroy(ItemComp.gameObject);
        if (KindofItem.Count == 0)
        {
            KindofItem.Clear();
            DataController.GetInstance().InGameData.Inventory.Remove(ItemKey);
        }

        PushingInventory(ParentSlot);

        Debug.Log("Save");
        DataController.GetInstance().Save();
    }

    private void PushingInventory(Item_Slot DeletedSlot)
    {
        int SlotCount = m_ItemSlotList.Count;
        int DeletedPoint = 0;
        int EndPoint = SlotCount;
        bool isFind = false;

        for(int i = 0; i < SlotCount; ++i)
        {
            if(!isFind && m_ItemSlotList[i] == DeletedSlot)
            {
                DeletedPoint = i;
                Debug.Log(DeletedPoint);
                isFind = true;
                continue;
            }

            if(isFind && !m_ItemSlotList[i].ChildItem)
            {
                EndPoint = i;
                Debug.Log(EndPoint);
                break;
            }
        }

        //조건부 연산 종료 시퀀스 추가
        //Deletenode와 EndNode위치가 같으면 종료

        //아이템 밀기
        GameObject MovedItem;
        for(int i = DeletedPoint + 1; i < EndPoint; ++i)
        {
            Debug.Log("Index : " + i);
            MovedItem = m_ItemSlotList[i].ChildItem.gameObject;
            m_ItemSlotList[i - 1].ChildItem = m_ItemSlotList[i].ChildItem;
            MovedItem.transform.parent = m_ItemSlotList[i - 1].transform;
            MovedItem.transform.localPosition = Vector3.zero;
        }

        m_ItemSlotList[EndPoint - 1].ChildItem = null;
        Debug.Log(m_ItemSlotList[EndPoint - 1].ChildItem);
    }
}
