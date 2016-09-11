using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI_EquipStat_PlayerArmed : MonoBehaviour {

    [SerializeField]
    private Item_Slot[] m_ItemSlots;

    private static Dictionary<EQUIPMENTTYPEID, Item_Slot> m_AttachedItemSlot
        = new Dictionary<EQUIPMENTTYPEID, Item_Slot>();
    public static Dictionary<EQUIPMENTTYPEID, Item_Slot> AttachedSlot
    { get { return m_AttachedItemSlot; } }

    void Start()
    {
        AttachItemSlot();
        InitLoadedEquips();
    }

    //First. Attach inspector Item_Slot to Collections
    private void AttachItemSlot()
    {
        foreach (Item_Slot slot in m_ItemSlots)
            m_AttachedItemSlot.Add(slot.EquipMentId, slot);
    }

    //Second. Load Save Datas
    private void InitLoadedEquips()
    {
        GameObject LoadedItemInstance = null;
        EquipMent_Interface LoadedItem = null;
        Item_Interface_Comp LoadedItemComp = null;

        Dictionary<EQUIPMENTTYPEID, EquipMent_Interface> LoadedEquipList =
            DataController.GetInstance().InGameData.ArmedEquip;

        if (LoadedEquipList.Count == 0)
            return;

        for (int i = 0; i < LoadedEquipList.Count; ++i)
        {
            LoadedItem = LoadedEquipList.ToList()[i].Value;
            LoadedItemInstance = 
                ItemManager.GetInstance().ItemInfoToGameObject(LoadedItem);
            LoadedItemComp = 
                LoadedItemInstance.AddComponent<Item_Interface_Comp>();
            LoadedItemComp.ItemInfo = LoadedItem;

            m_AttachedItemSlot[LoadedItem.EqiupmentId].ChildItem =
                LoadedItemComp;

            Debug.Log("LoadedItemComp : " + LoadedItemComp.name);
            Debug.Log("ChildItem : " + m_AttachedItemSlot[LoadedItem.EqiupmentId].ChildItem);

            LoadedItemInstance.transform.parent = 
                m_AttachedItemSlot[LoadedItem.EqiupmentId].transform;
            LoadedItemInstance.transform.localPosition = Vector3.zero;
            LoadedItemInstance.transform.localScale = Vector3.one;
        }
    }

    public static Item_Interface_Comp EquipItem(
        Item_Interface_Comp _SelectedEquip, Item_Slot _HoveredSlot)
    {
        //실제 Rendering되는 Transform에 Child로 붙여주고
        //저장되는 데이터에 붙여준다.
        Item_Interface_Comp ChangedItem = null;

        Dictionary<EQUIPMENTTYPEID, EquipMent_Interface> EquipList =
            DataController.GetInstance().InGameData.ArmedEquip;

        EquipMent_Interface SelectedEquip =
            ((EquipMent_Interface)_SelectedEquip.ItemInfo);

        //Debug.Log(m_AttachedItemSlot[SelectedEquip.EqiupmentId].ChildItem.name);

        //이미 해당 부위에 아이템을 착용되있는 경우
        if (m_AttachedItemSlot[SelectedEquip.EqiupmentId].ChildItem != null)
        {
            //1. Item_Slot 내에 있는 아이템의 위치를 교체해준다.
            Item_Slot.SwapItem(_HoveredSlot, m_AttachedItemSlot[SelectedEquip.EqiupmentId]);

            //2. Inventory에 있는 아이템과 ArmedEquip에 있는 아이템을 교체한다.
            Item_Slot.SwapItemCollectionPosition(m_AttachedItemSlot[SelectedEquip.EqiupmentId],
                _HoveredSlot);

        }
        //착용 부위에 아이템을 착용하지 않은 경우
        else
        {
            //1. Item_Slot의 Child로 묶는다.
            m_AttachedItemSlot[SelectedEquip.EqiupmentId].ChildItem =
                _SelectedEquip;

            //2. 실제 Rendering이 되도록 Transform을 맞춰둔다.
            _SelectedEquip.gameObject.SetActive(false);
            _SelectedEquip.transform.parent = 
                m_AttachedItemSlot[SelectedEquip.EqiupmentId].transform;
            _SelectedEquip.transform.localPosition = Vector3.zero;
            _SelectedEquip.transform.localScale = Vector3.one;

            //3. 장착창에 등록한다.
            EquipList.Add(SelectedEquip.EqiupmentId, 
                ((EquipMent_Interface)_SelectedEquip.ItemInfo));

            //4. Inventory내에 존재하는 아이템을 제거한다.
            InventoryManager.GetInstance().RemoveItem(
                _HoveredSlot.ChildItem, _HoveredSlot);

            _SelectedEquip.gameObject.SetActive(true);

        }

        DataController.GetInstance().Save();
        return ChangedItem;
    }

    

}
