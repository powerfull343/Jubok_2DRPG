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
    public bool AddItem(Item_Interface ItemComp, int nItemCount)
    {
        List<Item_Interface> KindofItem;

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
           ).InGameData.Inventory.ContainsKey(ItemComp.itemName))
        {
            //DataController에 추가한다.
            KindofItem = new List<Item_Interface>();
            KindofItem.Add(ItemComp);
            DataController.GetInstance().InGameData.Inventory.Add(
                ItemComp.itemName, KindofItem);
        }
        else
        {
            KindofItem = DataController.GetInstance(
                ).InGameData.Inventory[ItemComp.itemName];
            //장비가 아닐 경우에는 갯수만 추가하고 그렇지 않을 경우 공간을 새로 만든다.
            if (ItemComp.itemType != ITEMTYPEID.ITEM_EQUIP)
            {
                KindofItem[0].itemCount += nItemCount;
                DataController.GetInstance().Save();
                return false;
            }
            //장비인 경우에는 리스트에 추가한다.
            else
                KindofItem.Add(ItemComp);
        }

        DataController.GetInstance().Save();
        return true;
    }

    //Create Item Object
    public void CreateItem(Item_Interface ItemComp, int nItemCount)
    {
        if (!AddItem(ItemComp, nItemCount))
            return;

        GameObject ItemObject = null;
        Item_Interface_Comp newItemInfo;

        ItemObject = ItemManager.GetInstance().ItemInfoToGameObject(ItemComp);
        ItemObject.SetActive(false);
        newItemInfo = ItemObject.AddComponent<Item_Interface_Comp>();
        newItemInfo.ItemInfo = ItemComp;
        newItemInfo.ItemInfo.itemCount = nItemCount;

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


    public void PushingInventory(Item_Slot DeletedSlot)
    {
        Debug.Log("PushingInventory Start");
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
            Debug.Log("Index : " + i);
            Debug.Log("Child Item : " + m_ItemSlotList[i].ChildItem);
            MovedItem = m_ItemSlotList[i].ChildItem.gameObject;
            m_ItemSlotList[i - 1].ChildItem = m_ItemSlotList[i].ChildItem;
            MovedItem.transform.parent = m_ItemSlotList[i - 1].transform;
            MovedItem.transform.localPosition = Vector3.zero;
        }

        m_ItemSlotList[EndPoint - 1].ChildItem = null;
        Debug.Log(m_ItemSlotList[EndPoint - 1].ChildItem);
    }
}
