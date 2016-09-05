﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class UI_Inventory_CursorManager : MonoBehaviour
{
    private UISprite m_AtlasSprite;
    private UI2DSprite m_NormalSprite;

    [SerializeField]
    private Item_Interface_Comp m_SelectedItem;
    private ITEM_SLOT_TYPE m_SelectedSlotType;

    [SerializeField]
    private GameObject m_ItemSlotBG;

    //Memory Pool
    private GameObject m_SelectedEdgeSquare;

    void OnEnable()
    {
        StartCoroutine("CursorProgress");
    }

    void Start()
    {
        m_AtlasSprite = MecroMethod.CheckGetComponent<UISprite>(gameObject);
        m_NormalSprite = MecroMethod.CheckGetComponent<UI2DSprite>(gameObject);
        if (!m_SelectedEdgeSquare)
        {
            m_SelectedEdgeSquare = 
                Instantiate(Resources.Load("ItemIcons/Sprite - SelectedArea") as GameObject);
            m_SelectedEdgeSquare.SetActive(false);
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
                    SettingItemEdgeSquare(SelectedSlot);
                    SettingDetailItemWindow(SelectedSlot);
                    yield return new WaitForSeconds(0.1f);

                    if (!Input.GetMouseButton(0))
                        continue;

                    ControllDetailItemWindow();
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

    private void SettingItemEdgeSquare(Item_Slot _SelectedSlot)
    {
        if (!_SelectedSlot.ChildItem)
            return;

        m_SelectedEdgeSquare.SetActive(false);
        UIWidget SelectedItemSlotWidget = m_SelectedEdgeSquare.GetComponent<UIWidget>();
        SelectedItemSlotWidget.depth = 4;

        m_SelectedEdgeSquare.transform.parent = _SelectedSlot.transform;
        m_SelectedEdgeSquare.transform.localPosition = Vector3.zero;
        m_SelectedEdgeSquare.transform.localScale = Vector3.one;
        m_SelectedEdgeSquare.transform.rotation = Quaternion.identity;
        m_SelectedEdgeSquare.SetActive(true);
    }

    private void SettingDetailItemWindow(Item_Slot _SelectedSlot)
    {
        if (!_SelectedSlot.ChildItem)
            return;

        InventoryManager.GetInstance(
            ).DetailWindow.OpenDetailItemInfo(_SelectedSlot);
        InventoryManager.GetInstance(
            ).DetailWindow.gameObject.SetActive(true);
        Debug.Log("Open");
    }

    private void ControllDetailItemWindow()
    {
        if(Input.GetMouseButton(0))
        {
            InventoryManager.GetInstance(
            ).DetailWindow.gameObject.SetActive(false);
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

    private void HoveringUpEvent(Item_Slot _SelectedSlot)
    {
        //0. Object 확인
        Item_Slot HoveredUpItemSlot = null;
        bool isHoveredUpArmedCollider = false;
        GameObject HoveredUpObject = UICamera.hoveredObject;


        if (!CheckingHoveringUpEvent(HoveredUpObject, out HoveredUpItemSlot,
            out isHoveredUpArmedCollider))
            return;

        //1. 인벤 -> 인벤
        if (m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
        {
            InvenSlotChange();
        }

        //2. 인벤 -> 장착창
        else if(m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_ARMED)
        {
            if (!ItemEquipOrSwap(_SelectedSlot))
                return;
        }
        //3. 인벤 -> 장착창 내부의 콜라이더
        else if(m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY &&
            isHoveredUpArmedCollider)
        {
            if (!ItemEquipOrSwap(_SelectedSlot))
                return;
        }

        //4. 장착창 -> 인벤
        else if(m_SelectedSlotType == ITEM_SLOT_TYPE.SLOT_ARMED &&
            HoveredUpItemSlot.ItemSlotType == ITEM_SLOT_TYPE.SLOT_INVENTORY)
        {

        }
    }

    private bool CheckingHoveringUpEvent(GameObject _HoveredUpObject, 
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

    private void InvenSlotChange()
    {

    }

    private bool ItemEquipOrSwap(Item_Slot _HoveredSlot)
    {
        if (m_SelectedItem.ItemInfo.itemType != ITEMTYPEID.ITEM_EQUIP)
            return false;

        Dictionary<EQUIPMENTTYPEID, Item_Interface> EquipList =
            DataController.GetInstance().InGameData.ArmedEquip;

        EQUIPMENTTYPEID SelectedEquipId =
            ((EquipMent_Interface)m_SelectedItem.ItemInfo).EqiupmentId;

        //아이템 장착창에 해당 종류의 장비가 장착되어 있을때
        if(EquipList.ContainsKey(SelectedEquipId))
        {
            
        }
        else //장착된 장비가 하나도 존재하지 않을 경우
        {
            EquipList.Add(SelectedEquipId, m_SelectedItem.ItemInfo);
            m_SelectedItem
        }




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
