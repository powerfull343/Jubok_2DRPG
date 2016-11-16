using System;
using System.Collections;

public enum SPRITE_TYPEID
{
    SPRITE_NORMAL,
    SPRITE_NGUISPRITE,
    SPRITE_MAX,
};

[Serializable]
public class Item_Interface
{
    protected SPRITE_TYPEID m_SpriteType;
    public SPRITE_TYPEID SpriteType
    {
        get { return m_SpriteType; }
        set { m_SpriteType = value; }
    }

    //If you use normal_Sprite_Type you must setting these property
    protected NORMAL_SPRITE_ICONS m_NormalSprite_iconType;
    public NORMAL_SPRITE_ICONS NormalSprite_iconType
    {
        get { return m_NormalSprite_iconType; }
        set { m_NormalSprite_iconType = value; }
    }

    protected int m_NormalSprite_iconNumber = -1;
    public int NormalSprite_iconNumber
    {
        get { return m_NormalSprite_iconNumber; }
        set { m_NormalSprite_iconNumber = value; }
    }

    //If you use ngui_Sprite_Type you must setting these property
    protected string m_AtlasName = String.Empty;
    public string AtlasName
    {
        get { return m_AtlasName; }
        set { m_AtlasName = value; }
    }

    protected string m_AtlasSpriteName = String.Empty;
    public string AtlasSpriteName
    {
        get { return m_AtlasSpriteName; }
        set { m_AtlasSpriteName = value; }
    }
   
    protected string m_itemName;
    public string itemName
    {
        get { return m_itemName; }
        set { m_itemName = value; }
    }

    protected float m_itemWeight;
    public float itemWeight
    {
        get { return m_itemWeight; }
        set { m_itemWeight = value; }
    }

    protected ITEMTYPEID m_itemType;
    public ITEMTYPEID itemType
    {
        get { return m_itemType; }
        set { m_itemType = value; }
    }

    protected ITEMGRADEID m_itemGrade;
    public ITEMGRADEID itemGrade
    {
        get { return m_itemGrade; }
        set { m_itemGrade = value; }
    }

    protected int m_itemCount;
    public int itemCount
    {
        get { return m_itemCount; }
        set
        {
            m_itemCount = value;
        }
    }

    protected int m_itemValue;
    public int ItemValue
    {
        get { return m_itemValue; }
        set { m_itemValue = value; }
    }

    protected int m_DropItemRate;
    public int DropItemRate
    {
        get { return m_DropItemRate; }
        set { m_DropItemRate = value;}
    }

    protected string m_Explainitem;
    public string Explainitem
    {
        get { return m_Explainitem; }
        set { m_Explainitem = value; }
    }

    public Item_Interface() { }
    public virtual void Copyinstance(Item_Interface original)
    {
        //this.m_itemSprite = original.m_itemSprite;
        //this.m_itemUISprite = original.m_itemUISprite;
        this.m_SpriteType = original.m_SpriteType;
        this.m_NormalSprite_iconType = original.m_NormalSprite_iconType;
        this.m_NormalSprite_iconNumber = original.m_NormalSprite_iconNumber;
        this.m_AtlasName = original.m_AtlasName;
        this.m_AtlasSpriteName = original.m_AtlasSpriteName;

        this.m_itemName = original.m_itemName;
        this.m_itemWeight = original.m_itemWeight;
        this.m_itemType = original.m_itemType;
        this.m_itemGrade = original.m_itemGrade;
        this.m_itemCount = original.m_itemCount;
        this.m_itemValue = original.m_itemValue;
        this.m_DropItemRate = original.m_DropItemRate;
        this.m_Explainitem = original.m_Explainitem;
    }

    public static void SetItemInfo(Item_Interface ItemInfo, string _ItemName,
        int _ItemValue, float _ItemWeight, int _ItemCount, ITEMTYPEID _ItemtypeId, ITEMGRADEID _ItemGradeId)
    {
        ItemInfo.itemName = _ItemName;
        ItemInfo.ItemValue = _ItemValue;
        ItemInfo.itemWeight = _ItemWeight;
        ItemInfo.itemCount = _ItemCount;
        ItemInfo.itemType = _ItemtypeId;
        ItemInfo.itemGrade = _ItemGradeId;
    }
    
    public static Item_Interface CreateNormalSpriteItem(string _itemName, int _itemValue,
        float _itemWeight, int _itemCount, ITEMTYPEID _ItemtypeId, ITEMGRADEID _ItemGradeId,
        NORMAL_SPRITE_ICONS _NormalSprite_IconType,
        int _NormalSprite_IconNumber)
    {
        Item_Interface Item = new Item_Interface();
        SetItemInfo(Item, _itemName, _itemValue, _itemWeight, _itemCount, _ItemtypeId, _ItemGradeId);
        Item.SpriteType = SPRITE_TYPEID.SPRITE_NORMAL;
        Item.NormalSprite_iconType = _NormalSprite_IconType;
        Item.NormalSprite_iconNumber = _NormalSprite_IconNumber;

        return Item;
    }

    public static Item_Interface CreateNGUISpriteItem(string _itemName, int _itemValue,
        float _itemWeight, int _itemCount, ITEMTYPEID _ItemtypeId, ITEMGRADEID _ItemGradeId,
        string _AtlasName, string _AtlasSpriteName)
    {
        Item_Interface Item = new Item_Interface();
        SetItemInfo(Item, _itemName, _itemValue, _itemWeight, _itemCount, _ItemtypeId, _ItemGradeId);
        Item.SpriteType = SPRITE_TYPEID.SPRITE_NGUISPRITE;
        Item.AtlasName = _AtlasName;
        Item.AtlasSpriteName = _AtlasSpriteName;

        return Item;
    }
}