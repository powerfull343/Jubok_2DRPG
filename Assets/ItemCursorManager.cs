using UnityEngine;
using System.Collections;

public class ItemCursorManager : MonoBehaviour
{
    private Camera m_CamComp;

    //MouseCursorManager
    private UISprite m_Spritecomp;
    private UI2DSprite m_2DSpriteComp;

    //Hovered Position
    private UISprite m_HoveredSprite;
    private UI2DSprite m_Hovered2DSprite;

    //OneClickSelected
    private Item_Slot m_SelectedSlotPosition;
    //Memory Pool
    private GameObject m_SelectedEdgeSquare;

    private bool m_isSelected = false;
    private bool m_isSelectedSellerItem = false;

    void Awake()
    {
        m_CamComp = Camera.main;
    }

    void Start()
    {
        m_Spritecomp = Mecro.MecroMethod.CheckGetComponent<UISprite>(this.gameObject);
        m_Spritecomp.enabled = false;
        m_HoveredSprite = m_Spritecomp;

        m_2DSpriteComp = Mecro.MecroMethod.CheckGetComponent<UI2DSprite>(this.gameObject);
        m_Hovered2DSprite = m_2DSpriteComp;
        m_2DSpriteComp.enabled = false;

        m_SelectedEdgeSquare = Resources.Load("ItemIcons/Sprite - SelectedArea") as GameObject;
    }

    void OnEnable()
    {
        StartCoroutine("CursorEventProgress");
    }


    IEnumerator CursorEventProgress()
    {
        Item_Slot SelectedItemSlot;
        while (true)
        {
            //DebugingObjectInfo();
            //HoverObject();

            if (!m_isSelected)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //== Click Item Once ==//
                    ShowDetailItemData(out SelectedItemSlot);
                    SelectedItemSlotEdgeSquareRendering(SelectedItemSlot);
                    yield return new WaitForSeconds(0.1f);
                    //== Still Click Item over the 0.1 Sec==//
                    HighLightObject(SelectedItemSlot);
                }
            }
            else
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //-
                if (Input.GetMouseButtonUp(0))
                {
                    HoveringToMouseUp();
                    m_isSelected = false;
                }
            }
            yield return null;
        }
    }

    private void SelectedItemSlotEdgeSquareRendering(Item_Slot SelectedItemSlot)
    {
        if (SelectedItemSlot == null || !SelectedItemSlot.ChildItem)
            return;

        //같은 위치를 선택시 연산하지 않음
        if (m_SelectedSlotPosition == SelectedItemSlot)
            return;

        if (m_SelectedSlotPosition != null)
        {
            Transform PreviousSelectedEdgeSquare = 
                m_SelectedSlotPosition.transform.FindChild("Sprite - SelectedArea(Clone)");

            if (PreviousSelectedEdgeSquare != null)
                Destroy(PreviousSelectedEdgeSquare.gameObject);
        }

        m_SelectedSlotPosition = SelectedItemSlot;
        UI_GroceryStore_SellList.GetInstance().SelectedItemSlot = 
            m_SelectedSlotPosition;

        GameObject SelectedEdgeSquare =
            Instantiate(m_SelectedEdgeSquare) as GameObject;

        SelectedEdgeSquare.SetActive(false);
        UIWidget SelectedItemSlotWidget = SelectedEdgeSquare.GetComponent<UIWidget>();
        SelectedItemSlotWidget.depth = 4;

        SelectedEdgeSquare.transform.parent = SelectedItemSlot.transform;
        SelectedEdgeSquare.transform.localPosition = Vector3.zero;
        SelectedEdgeSquare.transform.localScale = Vector3.one;
        SelectedEdgeSquare.transform.rotation = Quaternion.identity;
        SelectedEdgeSquare.SetActive(true);
    }

    private void ShowDetailItemData(out Item_Slot SelectedSlot)
    {
        SelectedSlot = CheckItemSlot(UICamera.selectedObject);

        if (SelectedSlot == null || !SelectedSlot.ChildItem)
        {
            //Debug.Log("SelectedSlot : " + SelectedSlot.name);
            //Debug.Log("SelectedSlot.ChildItem : " + SelectedSlot.ChildItem);
            return;
        }

        if(SelectedSlot.isSelleritem)
        {//상점 아이템 간편 UI에 무게와 가격을 표시한다.
            SelectedSlot.ApplyItemToMerchantInfo();
        }
        else
        {//인벤토리 아이템 말풍선을 띄운다.
            InventoryManager.GetInstance(
                ).DetailWindow.OpenDetailItemInfo(SelectedSlot);
        }
    }

    private void HighLightObject(Item_Slot SelectedItemSlot)
    {
        // 아이템이 존재하지 않는다고하면 이벤트 발생하지 않는다.
        if (SelectedItemSlot == null || !SelectedItemSlot.ChildItem)
            return;

        if (UICamera.selectedObject != null && Input.GetKey(KeyCode.Mouse0))
        {
            //1. 아이템 슬롯인지 확인 슬롯이 아닐경우 이벤트 발생하지 않는다.
            //Item_Slot SlotInfo = CheckItemSlot(UICamera.selectedObject);
            //if (SlotInfo == null)
            //    return;

            //3. 인벤토리인지 상점 인벤토리인지 확인
            m_isSelectedSellerItem = SelectedItemSlot.isSelleritem;

            //4. 아이템 슬롯에 아이템이 존재하는지 확인 존재하지 않으면 이벤트 발생하지 않는다.
            Transform SelectedItem =
                UICamera.selectedObject.transform.FindChild("Sprite - ItemIcon(Clone)");

            if (SelectedItem == null)
                return;

            //5. UISprite , UI2DSprite인지 확인함. 
            UI2DSprite Item2DSprite =
                Mecro.MecroMethod.CheckGetComponent<UI2DSprite>(SelectedItem);

            if (Item2DSprite.enabled)
                Get2DSprite(Item2DSprite);
            else
            {
                UISprite ItemAtlasSprite =
                    Mecro.MecroMethod.CheckGetComponent<UISprite>(SelectedItem);

                if (!ItemAtlasSprite.enabled)
                {
                    Debug.LogError("Nothing have Image Comp");
                    return;
                }

                GetAtlasSprite(ItemAtlasSprite);
            }

            //6. 클릭시 위치 강제 이동.
            transform.localPosition = Input.mousePosition -
                new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            //7. 드래그 모드로 전환
            m_isSelected = true;
        }
        else
        {
            //0. 인벤토리 아이템 상세정보창을 띄웠을때 이를 해지한다.
            if(!SelectedItemSlot.isSelleritem)
                InventoryManager.GetInstance().DetailWindow.gameObject.SetActive(true);
        }

    }

    private Item_Slot CheckItemSlot(GameObject CheckingObject)
    {
        if (CheckingObject == null)
        {
            Debug.Log("UICamera Checking Object is Null");
            return null;
        }

        Item_Slot Result =
            CheckingObject.GetComponent<Item_Slot>();

        if (Result == null)
        {
            Debug.Log("UICamera Checking Object Cannot have Item_Slot Comp");
            return null;
        }

        return Result;
    }

    private void Get2DSprite(UI2DSprite CopyedSprite)
    {
        m_Hovered2DSprite = CopyedSprite;
        m_2DSpriteComp.sprite2D = CopyedSprite.sprite2D;
        m_2DSpriteComp.color = new Color(
            m_2DSpriteComp.color.r, m_2DSpriteComp.color.g, 
            m_2DSpriteComp.color.b, 0.5f);

        //마우스 포인터에 있는 sprite 활성
        if (!m_2DSpriteComp.enabled)
            m_2DSpriteComp.enabled = true;
        //기존에 존재하던 곳의 sprite 비활성
        if (CopyedSprite.enabled)
            CopyedSprite.enabled = false;
    }

    private void GetAtlasSprite(UISprite Copyedsprite)
    {
        m_HoveredSprite = Copyedsprite;
        m_Spritecomp.atlas = Copyedsprite.atlas;
        m_Spritecomp.spriteName = Copyedsprite.spriteName;

        //마우스 포인터에 있는 sprite 활성
        if (!m_Spritecomp.enabled)
            m_Spritecomp.enabled = true;
        //기존에 존재하던 곳의 sprite 비활성
        if (Copyedsprite.enabled)
            Copyedsprite.enabled = false;
    }
    
    private void HoveringToMouseUp()
    {
        Item_Slot HoveredItemSlot = CheckItemSlot(UICamera.hoveredObject);
        //아이템 슬롯이 선택되지 않은 경우.
        if (HoveredItemSlot == null)
        {
            //리셋하고 끝을 낸다.
            Debug.Log(CheckingHoveringObject().transform.parent.name);
            HoveringReset();
            return;
        }
        
        if (m_SelectedSlotPosition == null)
        {
            //선택된 아이템 자체가 없다.
            Debug.Log("Cannot find Item SlotPosition");
            HoveringReset();
            return;
        }

        HoveringEvent(HoveredItemSlot);

        HoveringReset();
    }

    private GameObject CheckingHoveringObject()
    {
        Ray ray = m_CamComp.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObj;
        if (Physics.Raycast(ray, out hitObj, Mathf.Infinity))
            return hitObj.collider.gameObject;

        return null;
    }

    private void HoveringEvent(Item_Slot newSelectedItemPosition)
    {
        //1.상점 -> 상점은 아무런 이벤트가 발생하지 않는다.
        if (m_isSelectedSellerItem && newSelectedItemPosition.isSelleritem)
            return;

        //2.상점 -> 인벤토리는 상점에 있는 물건을 인벤토리에 추가시키며 인벤토리 컨테이너에 추가한다.

        //무게가 가득 차거나 공간이 남아있지 않은 경우는 메시지를 띄우며 구매하지 않는다.
        else if (m_isSelectedSellerItem && !newSelectedItemPosition.isSelleritem)
        {
            //InventoryManager.GetInstance().BuyItem(m_SelectedSlotPosition);
            UI_GroceryStore_SellList.GetInstance().ClickBuyButton();
        }

        //3.인벤토리 -> 상점은 인벤토리에 있는 물건을 제거시키며 Value의 30%값의 금액을 
        //플레이어 매니져에 추가시키고 무게도 감소시킨다. 
        else if (!m_isSelectedSellerItem && newSelectedItemPosition.isSelleritem)
        {
            //InventoryManager.GetInstance().SellItem(m_SelectedSlotPosition);
            UI_GroceryStore_SellList.GetInstance().ClickSellButton();
        }

        //4. 둘다 false 일경우에는 인벤토리에 있는 아이템 끼리 이동하는것과 비슷하다.
        else if (m_SelectedSlotPosition != null &&
            (!m_isSelectedSellerItem && !newSelectedItemPosition.isSelleritem))
        {
            if (newSelectedItemPosition.ChildItem)
            {//새로운 포지션에 아이템이 존재해야 바꿀수있다.
                //UISprite newHoveringSprite = HoveringItemTrans.GetComponent<UISprite>();
                Item_Slot.SwapItem(m_SelectedSlotPosition, newSelectedItemPosition);
            }
        }
    }

    private void HoveringReset()
    {
        if (!m_Hovered2DSprite.enabled)
        {
            m_Hovered2DSprite.enabled = true;
            m_2DSpriteComp.enabled = false;
        }
        else if (!m_HoveredSprite.enabled)
        {
            m_HoveredSprite.enabled = true;
            m_Spritecomp.enabled = false;
        }
    }


    private void DebugingObjectInfo()
    {
        if(UICamera.selectedObject != null)
            Debug.Log("Seleted Object : " + UICamera.selectedObject.name);
        else
            Debug.Log("Selected Object is Null");

        if (UICamera.hoveredObject != null)
            Debug.Log("hovered Object : " + UICamera.hoveredObject.name);
        else
            Debug.Log("hovered Object is Null");

        Ray ray = m_CamComp.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObj;
        if(Physics.Raycast(ray, out hitObj, Mathf.Infinity))
            Debug.Log("Mouse hovered Object : " + hitObj.collider.gameObject.name);

        Debug.Log("Mouse Position : " + Input.mousePosition);
    }
}
