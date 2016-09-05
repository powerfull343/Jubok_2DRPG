using UnityEngine;
using System.Collections;
using Mecro;

public class UI_Inventory_CursorManager : MonoBehaviour
{

    private UISprite m_AtlasSprite;
    private UI2DSprite m_NormalSprite;

    [SerializeField]
    private Item_Interface_Comp m_SelectedItem;

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
                    HoveringUpEvent();
                    SelectReset();
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

    private void HoveringUpEvent()
    {
        //0. Object 확인
        GameObject HoveredObject = UICamera.hoveredObject;
        
        //1. 인벤 -> 인벤
        
    }

    private void SelectReset()
    {
        m_SelectedItem = null;
        m_AtlasSprite.enabled = false;
        m_NormalSprite.enabled = false;
    }
        
}
