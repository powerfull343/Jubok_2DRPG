using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Item_Slot : MonoBehaviour {

    //private bool m_isSellerItem = false;
    //public bool isSelleritem
    //{
    //    get { return m_isSellerItem; }
    //    set { m_isSellerItem = value; }
    //}
    [SerializeField]
    private ITEM_SLOT_TYPE m_ItemSlotType = ITEM_SLOT_TYPE.SLOT_INVENTORY;
    public ITEM_SLOT_TYPE ItemSlotType
    {
        get { return m_ItemSlotType; }
        set { m_ItemSlotType = value; }
    }

    [SerializeField]
    private Item_Interface_Comp m_ChildItem;
    public Item_Interface_Comp ChildItem
    {
        get { return m_ChildItem; }
        set { m_ChildItem = value; }
    }

    private int m_nRowIndex = 0;
    public int RowIndex
    {
        get { return m_nRowIndex; }
        set { m_nRowIndex = value; }
    }

    private Transform m_ItemIconTrans;
    public Transform ItemIconTrans
    {
        get { return m_ItemIconTrans; }
        set { m_ItemIconTrans = value; }
    }

    [SerializeField]
    private EQUIPMENTTYPEID m_EquipMentSlotId = EQUIPMENTTYPEID.EQUIP_NONE;
    public EQUIPMENTTYPEID EquipMentSlotId
    {
        get { return m_EquipMentSlotId; }
        set { m_EquipMentSlotId = value; }
    }
    
    public Item_Slot(Item_Slot origin)
    {
        //this.m_isSellerItem = origin.m_isSellerItem;
        this.m_ItemSlotType = origin.m_ItemSlotType;
        this.m_ChildItem = origin.m_ChildItem;
        this.m_nRowIndex = origin.m_nRowIndex;
        this.m_ItemIconTrans = origin.m_ItemIconTrans;
        this.m_EquipMentSlotId = origin.m_EquipMentSlotId;
    }
    
    public void ApplyItemToMerchantInfo()
    {
        if (m_ItemSlotType == ITEM_SLOT_TYPE.SLOT_STORE)
        {
            UI_GroceryStore_SellList.GetInstance().RenderItemInfo(
                this.ChildItem.ItemInfo.itemWeight, this.ChildItem.ItemInfo.ItemValue);
        }
    }

    public static void SwapItem(Item_Slot Slot1, Item_Slot Slot2)
    {
        Slot1.ChildItem.gameObject.SetActive(false);
        Slot2.ChildItem.gameObject.SetActive(false);

        Transform Slot1ChildItemTrans =
            Slot1.transform.FindChild("Sprite - ItemIcon(Clone)");
        Transform Slot2ChildItemTrans =
            Slot2.transform.FindChild("Sprite - ItemIcon(Clone)");

        Slot1ChildItemTrans.parent = Slot2.transform;
        Slot1ChildItemTrans.localPosition = Vector3.zero;
        Slot2.ChildItem = Slot1ChildItemTrans.GetComponent<Item_Interface_Comp>();
        
        Slot2ChildItemTrans.parent = Slot1.transform;
        Slot2ChildItemTrans.localPosition = Vector3.zero;
        Slot1.ChildItem = Slot1ChildItemTrans.GetComponent<Item_Interface_Comp>();

        Slot1ChildItemTrans.gameObject.SetActive(true);
        Slot2ChildItemTrans.gameObject.SetActive(true);
    }

    public static void SwapItemCollectionPosition(Item_Slot InventorySlot,
        Item_Slot EquipSlot)
    {
        EquipMent_Interface InventoryItem = null, Equipitem = null;

        EQUIPMENTTYPEID Equipid = 
            ((EquipMent_Interface)InventorySlot.ChildItem.ItemInfo).EqiupmentId;
        string ItemName = InventorySlot.ChildItem.ItemInfo.itemName;

        //Inventory Data Remove
        Dictionary<string, List<Item_Interface>> InvenList =
            DataController.GetInstance().InGameData.Inventory;
        InventoryItem = 
            ((EquipMent_Interface)InvenList[ItemName].Find(
            delegate (Item_Interface item)
            {return item == InventorySlot.ChildItem.ItemInfo;}));
        InventoryManager.GetInstance().RemoveItem(InventoryItem);

        //Equip Data Remove
        Dictionary < EQUIPMENTTYPEID, EquipMent_Interface> EquipList =
            DataController.GetInstance().InGameData.ArmedEquip;
        Equipitem = EquipList[((EquipMent_Interface)InventoryItem).EqiupmentId];
        EquipList.Remove(Equipitem.EqiupmentId);

        //Data Swaping
        InventoryManager.GetInstance().AddItem(Equipitem, 1);
        EquipList.Add(Equipid, InventoryItem);

        DataController.GetInstance().Save();
    }
}
