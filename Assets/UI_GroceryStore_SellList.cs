using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_GroceryStore_SellList : 
    Singleton<UI_GroceryStore_SellList>{

    //실제 판매될 아이템 목록
    private Dictionary<string, Item_Interface> m_SellList
        = new Dictionary<string, Item_Interface>();
    public Dictionary<string, Item_Interface> SellList
    {
        get { return m_SellList; }
        set { m_SellList = value; }
    }

    //상인이 소지 할 수 있는 아이템 공간들
    private List<Item_Slot> m_MerchantBackpack
        = new List<Item_Slot>();
    public List<Item_Slot> MerchantBackpack
    {
        get { return m_MerchantBackpack; }
        set { m_MerchantBackpack = value; }
    }
    [SerializeField]
    private Transform m_GridTransform;

    [SerializeField]
    private UILabel m_WeightAmount;

    [SerializeField]
    private UILabel m_ValueAmount;

    private Item_Slot m_SelectedItemSlot;
    public Item_Slot SelectedItemSlot
    {
        get { return m_SelectedItemSlot; }
        set { m_SelectedItemSlot = value; }
    }

    [SerializeField]
    private UI_GroceryStore_SubTradeUI m_SubUI;
    public UI_GroceryStore_SubTradeUI SubUI
    {
        get { return m_SubUI; }
        set { m_SubUI = value; }
    }

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        Mecro.MecroMethod.CheckGetComponent<Transform>(m_GridTransform);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_WeightAmount);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ValueAmount);
        Mecro.MecroMethod.CheckExistComponent<UI_GroceryStore_SubTradeUI>(m_SubUI);
        InitMerchantBackPack();
        InitSellList();
    }

    private void InitMerchantBackPack()
    {
        int nVerticalGridChildCount = m_GridTransform.childCount;
        int nHorizontalGridChildCount = 0;
        for(int i = 0; i < nVerticalGridChildCount; ++i)
        {
            Transform HorizontalGridTransform = m_GridTransform.GetChild(i);
            nHorizontalGridChildCount = HorizontalGridTransform.childCount;
            for(int j = 0; j < nHorizontalGridChildCount; ++j)
            {
                Item_Slot EmptyItemSlot =
                    Mecro.MecroMethod.CheckGetComponent<Item_Slot>(HorizontalGridTransform.GetChild(j));
                EmptyItemSlot.isSelleritem = true;
                EmptyItemSlot.RowIndex = i;
                m_MerchantBackpack.Add(EmptyItemSlot);
            }
        }
    }

    private void InitSellList()
    {
        //Testing Method
        List<Item_Interface> LoadItemList = new List<Item_Interface>();

        Item_Interface Potion01 = new Item_Interface();
        Potion01.itemName = "Potion01";
        Potion01.ItemValue = 10;
        Potion01.itemWeight = 0.5f;
        Potion01.itemType = ITEMTYPEID.ITEM_POTION;
        Potion01.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Potion01.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Potion01.NormalSprite_iconNumber = 37;
        LoadItemList.Add(Potion01);

        Item_Interface Potion02 = new Item_Interface();
        Potion02.itemName = "Potion02";
        Potion02.ItemValue = 30;
        Potion02.itemWeight = 0.3f;
        Potion02.itemType = ITEMTYPEID.ITEM_POTION;
        Potion02.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Potion02.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Potion02.NormalSprite_iconNumber = 38;
        LoadItemList.Add(Potion02);

        Item_Interface Potion03 = new Item_Interface();
        Potion03.itemName = "Potion03";
        Potion03.ItemValue = 50;
        Potion03.itemWeight = 0.7f;
        Potion03.itemType = ITEMTYPEID.ITEM_POTION;
        Potion03.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Potion03.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Potion03.NormalSprite_iconNumber = 39;
        LoadItemList.Add(Potion03);

        Item_Interface Sword = new EquipMent_Interface();
        Sword.itemName = "Sword";
        Sword.ItemValue = 100;
        Sword.itemWeight = 3f;
        Sword.itemType = ITEMTYPEID.ITEM_EQUIP;
        ((EquipMent_Interface)Sword).EqiupmentId = EQUIPMENTTYPEID.EQUIP_WEAPON;
        ((EquipMent_Interface)Sword).Attack = 3;
        Sword.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Sword.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Sword.NormalSprite_iconNumber = 2;
        LoadItemList.Add(Sword);

        Item_Interface Sword2 = new EquipMent_Interface();
        Sword2.itemName = "Sword2";
        Sword2.ItemValue = 300;
        Sword2.itemWeight = 6f;
        Sword2.itemType = ITEMTYPEID.ITEM_EQUIP;
        ((EquipMent_Interface)Sword2).EqiupmentId = EQUIPMENTTYPEID.EQUIP_WEAPON;
        ((EquipMent_Interface)Sword2).Attack = 5;
        Sword2.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Sword2.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Sword2.NormalSprite_iconNumber = 5;
        LoadItemList.Add(Sword2);

        Item_Interface Robe = new EquipMent_Interface();
        Robe.itemName = "Robe";
        Robe.ItemValue = 500;
        Robe.itemWeight = 10f;
        Robe.itemType = ITEMTYPEID.ITEM_EQUIP;
        ((EquipMent_Interface)Robe).EqiupmentId = EQUIPMENTTYPEID.EQUIP_ARMOR;
        ((EquipMent_Interface)Robe).Hp = 10;
        Robe.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Robe.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Robe.NormalSprite_iconNumber = 130;
        LoadItemList.Add(Robe);

        Item_Interface Staff01 = new EquipMent_Interface();
        Staff01.itemName = "Staff01";
        Staff01.ItemValue = 100;
        Staff01.itemWeight = 2f;
        Staff01.itemType = ITEMTYPEID.ITEM_EQUIP;
        ((EquipMent_Interface)Staff01).EqiupmentId = EQUIPMENTTYPEID.EQUIP_WEAPON;
        ((EquipMent_Interface)Staff01).Attack = 5;
        Staff01.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Staff01.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Staff01.NormalSprite_iconNumber = 48;
        LoadItemList.Add(Staff01);

        Item_Interface Apple = new Item_Interface();
        Apple.itemName = "Apple";
        Apple.ItemValue = 5;
        Apple.itemWeight = 0.1f;
        Apple.itemType = ITEMTYPEID.ITEM_FOOD;
        Apple.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Apple.NormalSprite_iconType = NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2;
        Apple.NormalSprite_iconNumber = 78;
        LoadItemList.Add(Apple);
        
        AddMerchantSellList(LoadItemList);
    }

    private void AddMerchantSellList(List<Item_Interface> itemInstances)
    {
        GameObject itemObject;
        Item_Interface_Comp newItemInfo;
        for (int i = 0; i < itemInstances.Count; ++i)
        {
            itemObject = ItemManager.GetInstance().ItemInfoToGameObject(itemInstances[i]);
            itemObject.SetActive(false);
            newItemInfo = itemObject.AddComponent<Item_Interface_Comp>();
            newItemInfo.ItemInfo = itemInstances[i];
            itemObject.transform.parent = m_MerchantBackpack[i].transform;
            itemObject.transform.localScale = Vector3.one;
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.SetActive(true);
            m_MerchantBackpack[i].ChildItem = newItemInfo;
        }
    }
    
    public void RenderItemInfo(float fWeight, int nValue)
    {
        m_WeightAmount.text = fWeight.ToString();
        m_ValueAmount.text = nValue.ToString();
    }

    public void ClickBuyButton()
    {
        if (!m_SelectedItemSlot || !m_SelectedItemSlot.isSelleritem)
            return;

        if(m_SelectedItemSlot.ChildItem.ItemInfo.itemType == ITEMTYPEID.ITEM_EQUIP)
        {
            InventoryManager.GetInstance().BuyItem(m_SelectedItemSlot, 1);
            return;
        }

        m_SubUI.HideAndShowTradeMenu();
        m_SubUI.InitSubTradeMenu(m_SelectedItemSlot, true);
    }

    public void ClickSellButton()
    {
        if (!m_SelectedItemSlot || m_SelectedItemSlot.isSelleritem)
            return;

        if (m_SelectedItemSlot.ChildItem.ItemInfo.itemType == ITEMTYPEID.ITEM_EQUIP)
        {
            InventoryManager.GetInstance().SellItem(m_SelectedItemSlot, 1);
            return;
        }

        m_SubUI.HideAndShowTradeMenu();
        m_SubUI.InitSubTradeMenu(m_SelectedItemSlot, false);
    }
}
