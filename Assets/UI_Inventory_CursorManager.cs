﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;
using LobbyManager;

public class UI_Inventory_CursorManager : MonoBehaviour
{
    private UISprite m_AtlasSprite;
    private UI2DSprite m_NormalSprite;

    [SerializeField]
    private Item_Interface_Comp m_SelectedItem;
    private ITEM_SLOT_TYPE m_SelectedSlotType;

    [SerializeField]
    private GameObject m_ItemSlotBG;

    private delegate void MouseUpEvent(Item_Slot _SelectedSlot);
    private MouseUpEvent HoveringUpEvent;

    void OnEnable()
    {
        StartCoroutine("CursorProgress");
        SetHoveringEvent();
    }

    void Start()
    {
        Debug.Log("Start");
        m_AtlasSprite = MecroMethod.CheckGetComponent<UISprite>(gameObject);
        m_NormalSprite = MecroMethod.CheckGetComponent<UI2DSprite>(gameObject);
    }

    private void SetHoveringEvent()
    {
        switch(LobbyController.GetInstance().OpenedPanel)
        {
            case LobbyButtonFunc.IDSUBPANEL.PANELID_GROCERYSTORE:
                HoveringUpEvent = StoreInventory_Event;
                Debug.Log("StoreHoveringEvent");
                break;

            case LobbyButtonFunc.IDSUBPANEL.PANELID_PLAYERSTAT:
                HoveringUpEvent = EquipInventory_Event;
                Debug.Log("EquipHoveringEvent");
                break;

            default:
                Debug.LogError("Warning! it is not find panel id : " + LobbyController.GetInstance().OpenedPanel);
                break;
        }
    }

    IEnumerator CursorProgress()
    {
        bool SelectedItem = false;
        Item_Slot SelectedSlot = null;

        while (true)
        {
            if (!SelectedItem)
            {
                if (Input.GetMouseButton(0) &&
                    CheckingItemSlot(out SelectedSlot))
                {
                    InventoryManager.GetInstance().InvenFunc.ShowDetailSquare(SelectedSlot);
                    SettingDetailItemWindow(SelectedSlot);
                    yield return new WaitForSeconds(0.15f);

                    if (!Input.GetMouseButton(0))
                    {
                        AttachDetailWindow_HideFunc();
                        continue;
                    }

                    DisappearDetailItemWindow();
                    AttachItem(SelectedSlot);
                    CursorTransformSetting();
                    AttachItemImage(SelectedSlot);
                    SelectedItem = true;
                }
            }
            else
            {
                CursorTransformSetting();
                if (Input.GetMouseButtonUp(0))
                {
                    //Event Add On
                    HoveringUpEvent(SelectedSlot);
                    HoveringReset(out SelectedSlot);
                    SelectedItem = false;
                }
            }
            yield return null;
        }

        yield return null;
    }

    private bool CheckingItemSlot(out Item_Slot _SelectedSlot)
    {
        GameObject SelectedObject = UICamera.hoveredObject;
        _SelectedSlot = null;

        if (!SelectedObject)
        {
            Debug.Log("Cannot Find Selected Object");
            return false;
        }

        _SelectedSlot = SelectedObject.GetComponent<Item_Slot>();
        if (!_SelectedSlot)
        {
            Debug.Log("Not Item Slot Object");
            return false;
        }

        if(!_SelectedSlot.ChildItem)
        {
            Debug.Log("Item_Slot Cannot have Child Item");
            return false;
        }

        m_SelectedSlotType = _SelectedSlot.ItemSlotType;
        return true;
    }

    private void SettingDetailItemWindow(Item_Slot _SelectedSlot)
    {
        if (_SelectedSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
        {
            InventoryManager.GetInstance(
                ).InvenFunc.OpenItemDetailWindow(_SelectedSlot);
        }
        else
        {
            _SelectedSlot.ApplyItemToMerchantInfo();
             UI_Inventory_LeftItemWindow LeftItemWin =
                 InventoryManager.GetInstance().InvenFunc.LeftItemWindow;

             LeftItemWin.SetItemData(_SelectedSlot.ChildItem.gameObject);
             if (!LeftItemWin.gameObject.activeSelf)
                 LeftItemWin.gameObject.SetActive(true);
        }
    }

    private void AttachDetailWindow_HideFunc()
    {
        if (m_SelectedSlotType != ITEM_SLOT_TYPE.SLOT_INVENTORY)
            return;

        InventoryManager.GetInstance(
            ).InvenFunc.DetailWindow.m_OwnExtension.StartHidingClickAnotherArea();
        
    }

    private void DisappearDetailItemWindow()
    {
        if(Input.GetMouseButton(0))
        {
            InventoryManager.GetInstance(
            ).InvenFunc.DetailWindow.gameObject.SetActive(false);
        }
    }

    private void AttachItem(Item_Slot _SelectedSlot)
    {
        m_SelectedItem = _SelectedSlot.ChildItem;
        if (!m_SelectedItem)
            Debug.Log("Cannot Attach");
    }

    private void CursorTransformSetting()
    {
        transform.position =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void AttachItemImage(Item_Slot _SelectedSlot)
    {
        SPRITE_TYPEID ItemSpriteType =
            _SelectedSlot.ChildItem.ItemInfo.SpriteType;

        UISprite AtlasSprite = null;
        UI2DSprite NormalSprite = null;

        switch (ItemSpriteType)
        {
            case SPRITE_TYPEID.SPRITE_NORMAL:
                NormalSprite =
                    MecroMethod.CheckGetComponent<UI2DSprite>
                        (_SelectedSlot.ChildItem.gameObject);
                m_NormalSprite.enabled = true;
                m_NormalSprite.sprite2D = NormalSprite.sprite2D;
                m_NormalSprite.color = new Color(1f, 1f, 1f, 0.5f);
                break;

            case SPRITE_TYPEID.SPRITE_NGUISPRITE:
                break;

            default:
                break;
        }
    }

    private void EquipInventory_Event(Item_Slot _SelectedSlot)
    {
        //0. Object 확인
        Item_Slot HoveredUpItemSlot = null;
        bool isHoveredUpArmedCollider = false;
        GameObject HoveredUpObject = UICamera.hoveredObject;

        if (!CheckingEquip_HoveringUpEvent(HoveredUpObject, out HoveredUpItemSlot,
            out isHoveredUpArmedCollider))
            return;

        //1. 인벤 -> 장착창 내부의 콜라이더
        if (m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            isHoveredUpArmedCollider)
        {
            if (!ItemEquipOrSwap(_SelectedSlot))
                return;
        }
        //2. 인벤 -> 장착창
        else if (m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_ARMED)
        {
            if (!ItemEquipOrSwap(_SelectedSlot))
                return;
        }
        //3. 인벤 -> 인벤
        else if (m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
            return;

        //4. 장착창 -> 인벤
        else if (m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_ARMED &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
        {

        }
    }

    private void StoreInventory_Event(Item_Slot _SelectedSlot)
    {
        //0. Object 확인
        Item_Slot HoveredUpItemSlot = null;
        GameObject HoveredUpObject = UICamera.hoveredObject;

        HoveredUpItemSlot = HoveredUpObject.GetComponent<Item_Slot>();
        if (!HoveredUpItemSlot)
            return;
        
        //1. Store -> Store, Inventory -> Inventory
        //haven't Event
        if (_SelectedSlot.ItemSlotType == HoveredUpItemSlot.ItemSlotType)
            return;

        //2. Inventory -> Store
        else if(_SelectedSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_STORE)
        {
            Debug.Log("Inventory -> Store");
            UI_GroceryStore_SellList.GetInstance().SelectedItemSlot =
                _SelectedSlot;
            UI_GroceryStore_SellList.GetInstance().ClickSellButton();
        }

        //3. Store -> Inventory
        else if (_SelectedSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_STORE &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
        {
            Debug.Log("Store -> Inventory");
            UI_GroceryStore_SellList.GetInstance().SelectedItemSlot =
                _SelectedSlot;
            UI_GroceryStore_SellList.GetInstance().ClickBuyButton();
        }

    }

    //
    private bool CheckingEquip_HoveringUpEvent(GameObject _HoveredUpObject, 
        out Item_Slot _HoveredUpSlot, out bool _isHoveredUpArmedCollider)
    {
        _HoveredUpSlot = _HoveredUpObject.GetComponent<Item_Slot>();
        _isHoveredUpArmedCollider = false;

        if (_HoveredUpSlot != null)
            return true;

        if(_HoveredUpObject.name == "Collider - ArmedSlotArea")
        {
            _isHoveredUpArmedCollider = true;
            return true;
        }

        return false;
    }

    private bool ItemEquipOrSwap(Item_Slot _HoveredSlot)
    {
        if (m_SelectedItem.ItemInfo.itemType != ITEMTYPEID.ITEM_EQUIP)
        {
            Debug.Log("Not Equip Type");
            return false;
        }

        //아이템 장착창에 해당 종류의 장비가 장착되어 있을때
        UI_EquipStat_PlayerArmed.EquipItem(m_SelectedItem, _HoveredSlot);
        return true;   
    }

    private void HoveringReset(out Item_Slot _SelectedSlot)
    {
        _SelectedSlot = null;
        m_SelectedItem = null;
        m_AtlasSprite.enabled = false;
        m_NormalSprite.enabled = false;
    }
        
}
