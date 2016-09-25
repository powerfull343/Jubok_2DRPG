using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;

public class InventoryManager : 
    Singleton<InventoryManager> {

    private float m_fInvenWeight;
    public float InvenWeight
    {
        get { return m_fInvenWeight; }
        set { m_fInvenWeight = value; }
    }
    private float m_fInvenMaxWeight;
    public float InvenMaxWeight
    {
        get { return m_fInvenMaxWeight; }
        set { m_fInvenMaxWeight = value; }
    }

    private static int m_nItemCreatePosition = 0;
    public static int ItemCreatePosition
    {
        get { return m_nItemCreatePosition; }
        set { m_nItemCreatePosition = value; }
    }

    [SerializeField]
    private Transform m_GridTransform;

    private List<Item_Slot> m_ItemSlotList
        = new List<Item_Slot>();
    public List<Item_Slot> ItemSlotList
    {
        get { return m_ItemSlotList; }
        set { m_ItemSlotList = value; }
    }

    [SerializeField]
    private UI_Inventory_Functions m_InvenFunc;
    public UI_Inventory_Functions InvenFunc
    {
        get { return m_InvenFunc; }
    }

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        MecroMethod.CheckExistComponent<Transform>(m_GridTransform);
        MecroMethod.CheckExistComponent<
            UI_Inventory_Functions>(m_InvenFunc);
        InitBackPackMemory();
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

    private void InitBackPackItems()
    {
        GameObject ItemObject;
        Item_Interface_Comp newItemInfo;

        Dictionary<string, List<Item_Interface>> ItemList =
            ItemManager.GetInstance().InventoryItems;
        List<Item_Interface> KindofItems;

        for (int i = 0; i < ItemList.Count; ++i)
        {
            KindofItems = ItemList.Values.ToList()[i];
            for (int j = 0; j < KindofItems.Count; ++j, ++ItemCreatePosition)
            {
                ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(KindofItems[j]);
                ItemObject.SetActive(false);
                newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
                newItemInfo.ItemInfo = KindofItems[j];
                ItemObject.transform.parent =
                    ItemSlotList[ItemCreatePosition].transform;
                ItemObject.transform.localScale = Vector3.one;
                ItemObject.transform.localPosition = Vector3.zero;
                ItemObject.SetActive(true);
                ItemSlotList[ItemCreatePosition].ChildItem = newItemInfo;
            }
        }
    }

    public float CalcItemWeight(List<Item_Interface> ItemInfo)
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
        if (m_fInvenWeight + fWeightSize > m_fInvenMaxWeight)
            return false;

        return true;
    }

    //Only Input Container
    //return Type - if you Create Instance true
    public Item_Interface AddItem(Item_Interface _OriginItem, int _nItemCount)
    {
        List<Item_Interface> KindofItem = null;
        Item_Interface newItemInterface = null;

        //Input Item Potision
        for (int i = 0; i < m_ItemSlotList.Count; ++i)
        {
            if (!m_ItemSlotList[i].ChildItem)
            {
                m_nItemCreatePosition = i;
                break;
            }
        }

        if (!DataController.GetInstance(
           ).InGameData.Inventory.ContainsKey(_OriginItem.itemName))
           //기존 인벤토리 컬렉션의 일치하는 Key 항목이 존재하지 않는 경우
        {
            //새로운 아이템을 할당한다.
            newItemInterface =
                ItemManager.GetInstance().CreateItemInfo(_OriginItem, _nItemCount);
            //DataController에 추가한다.
            KindofItem = new List<Item_Interface>();
            KindofItem.Add(newItemInterface);
            DataController.GetInstance().InGameData.Inventory.Add(
                newItemInterface.itemName, KindofItem);
        }
        else
        //기존 인벤토리 컬렉션의 일치하는 Key 항목이 존재하는 경우
        {
            KindofItem = DataController.GetInstance(
                ).InGameData.Inventory[_OriginItem.itemName];
            //장비가 아닐 경우에는 갯수만 추가하고 그렇지 않을 경우 공간을 새로 만든다.
            if (_OriginItem.itemType != ITEMTYPEID.ITEM_EQUIP)
            {
                KindofItem[0].itemCount += _nItemCount;
                DataController.GetInstance().Save();
                return null;
            }
            //장비인 경우에는 리스트에 추가한다.
            else
            {
                //새로운 아이템을 할당한다.
                newItemInterface =
                    ItemManager.GetInstance().CreateItemInfo(_OriginItem, _nItemCount);
                KindofItem.Add(newItemInterface);
            }
        }

        DataController.GetInstance().Save();
        return newItemInterface;
    }

    //Create Item Object
    public void CreateItem(Item_Interface ItemComp, int nItemCount)
    {
        Item_Interface CreatedItem = AddItem(ItemComp, nItemCount);
        if (CreatedItem == null)
            return;

        GameObject ItemObject = null;
        Item_Interface_Comp newItemInfo = null;

        ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(ItemComp);
        ItemObject.SetActive(false);
        newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
        newItemInfo.ItemInfo = CreatedItem;

        ItemObject.transform.parent = m_ItemSlotList[m_nItemCreatePosition].transform;
        ItemObject.transform.localScale = Vector3.one;
        ItemObject.transform.localPosition = Vector3.zero;
        ItemObject.SetActive(true);
        m_ItemSlotList[m_nItemCreatePosition].ChildItem = newItemInfo;
    }

    //Destroy Instance
    public void DestroyItem(Item_Interface_Comp ItemComp, int nDeleteCount)
    {
        List<Item_Interface> KindofItem;
        if (!DataController.GetInstance().InGameData.Inventory.ContainsKey(ItemComp.ItemInfo.itemName))
            Debug.LogError("Cannot find item Object");

        string ItemKey = ItemComp.ItemInfo.itemName;
        KindofItem = 
            DataController.GetInstance().InGameData.Inventory[ItemComp.ItemInfo.itemName];

        Debug.Log("Kindofitem Count : " + KindofItem.Count);

        Item_Interface FindObject = KindofItem.Find
        (
            delegate (Item_Interface item)
            {
                return item == ItemComp.ItemInfo;
            }
        );

        if (FindObject == null)
        {
            Debug.LogError("Warning) Item is Cannot existing");
            return;
        }

        Debug.Log(FindObject.itemName);

        FindObject.itemCount -= nDeleteCount;

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

    //Only Remove Instance
    public void RemoveItem(Item_Interface SelectedItem)
    {
        List<Item_Interface> KindofItem;
        if (!DataController.GetInstance(
            ).InGameData.Inventory.ContainsKey(SelectedItem.itemName))
            Debug.LogError("Cannot find item Object");

        string ItemKey = SelectedItem.itemName;
        KindofItem =
            DataController.GetInstance().InGameData.Inventory[SelectedItem.itemName];

        Item_Interface FindObject = KindofItem.Find
        (
            delegate (Item_Interface item)
            {
                return item == SelectedItem;
            }
        );

        KindofItem.Remove(FindObject);
    }

    //Auto Pusing Inventory
    public void PushingInventory(Item_Slot DeletedSlot)
    {
        //Debug.Log("PushingInventory Start");
        int SlotCount = m_ItemSlotList.Count;
        int DeletedPoint = 0;
        int EndPoint = SlotCount;
        bool isFind = false;

        for (int i = 0; i < SlotCount; ++i)
        {
            if(!isFind && m_ItemSlotList[i] == DeletedSlot)
            {
                DeletedPoint = i;
                isFind = true;
                continue;
            }

            if(isFind && !m_ItemSlotList[i].ChildItem)
            {
                EndPoint = i;
                break;
            }
        }

        //조건부 연산 종료 시퀀스 추가
        //Deletenode와 EndNode위치가 같으면 종료

        //아이템 밀기
        GameObject MovedItem;
        for(int i = DeletedPoint + 1; i < EndPoint; ++i)
        {
            //Debug.Log("Index : " + i);
            //Debug.Log("Child Item : " + m_ItemSlotList[i].ChildItem);
            MovedItem = m_ItemSlotList[i].ChildItem.gameObject;
            m_ItemSlotList[i - 1].ChildItem = m_ItemSlotList[i].ChildItem;
            MovedItem.transform.parent = m_ItemSlotList[i - 1].transform;
            MovedItem.transform.localPosition = Vector3.zero;
        }

        m_ItemSlotList[EndPoint - 1].ChildItem = null;
        //Debug.Log(m_ItemSlotList[EndPoint - 1].ChildItem);
    }

    //Use to Equip Item to Move to Inventory Last ItemSlot
    public void MoveItemLastSlot(Item_Slot MovedSlot)
    {
        Item_Interface_Comp SelectedItem = MovedSlot.ChildItem;

        if(m_ItemSlotList.Count <= m_nItemCreatePosition)
        {
            Debug.Log("Item inventory is full");
            return;
        }

        SelectedItem.transform.parent = m_ItemSlotList[m_nItemCreatePosition].transform;
        SelectedItem.transform.localPosition = Vector3.zero;
        SelectedItem.transform.localScale = Vector3.one;
        m_ItemSlotList[m_nItemCreatePosition].ChildItem = SelectedItem;

        ++m_nItemCreatePosition;

        UIWidget SelectedItemWidget =
            Mecro.MecroMethod.CheckGetComponent<UIWidget>(SelectedItem.gameObject);
        SelectedItemWidget.ParentHasChanged();
    }
}
