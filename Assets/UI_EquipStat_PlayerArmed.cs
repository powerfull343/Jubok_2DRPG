using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_EquipStat_PlayerArmed : MonoBehaviour {

    private Dictionary<EQUIPMENTTYPEID, Item_Slot> m_ItemSlotObjects
         = new Dictionary<EQUIPMENTTYPEID, Item_Slot>();
    public Dictionary<EQUIPMENTTYPEID, Item_Slot> AttachSlot
    { get { return m_ItemSlotObjects; } }

    public static void AttachItemSlot(Item_Interface_Comp _SelectedItem)
    {

    }

}
