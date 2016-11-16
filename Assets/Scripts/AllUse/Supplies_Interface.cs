using System;
using System.Collections;

[Serializable]
public class Supplies_Interface : Item_Interface {

    public Supplies_Interface() : base()
    {
        itemType = ITEMTYPEID.ITEM_FOOD;
    }

    protected SUPPLIESEFFECTID m_SupplieEffectId;
    public SUPPLIESEFFECTID SupplieEffectId
    {
        get { return m_SupplieEffectId; }
        set { m_SupplieEffectId = value; }
    }

    protected int m_EffectAmount;
    public int EffectAmount
    {
        get { return m_EffectAmount; }
        set { m_EffectAmount = value; }
    }

    /// <summary>
    /// if(SpriteTypeId == NORMAL_SPRITE) you must setting SpriteIconType, SpriteIconNumber
    /// else if(SpriteTypeId == NGUI_SPRITE) setting AtalsName, AtlasSpriteName
    /// </summary>
    public static Supplies_Interface CreateSupplies(string _itemName, int _itemValue,
        float _itemWeight, SUPPLIESEFFECTID _EffectId, int _EffectAmount,
        ITEMTYPEID _ItemTypeId, ITEMGRADEID _ItemGradeId,
        SPRITE_TYPEID _SpriteTypeId,
        //SpriteTypeId == NORMAL_SPRITE
        NORMAL_SPRITE_ICONS _SpriteIconType, int _SpriteIconNumber,
        //SpriteTypeId == NGUI_SPRITE
        string _AtlasName = "", string _AtlasSpriteName = "")
    {
        Supplies_Interface Item = new Supplies_Interface();
        Item_Interface.SetItemInfo((Item_Interface)Item, _itemName, _itemValue,
            _itemWeight, 1, _ItemTypeId, _ItemGradeId);
        Item.SupplieEffectId = _EffectId;
        Item.EffectAmount = _EffectAmount;

        Item.SpriteType = _SpriteTypeId;
        Item.NormalSprite_iconType = _SpriteIconType;
        Item.NormalSprite_iconNumber = _SpriteIconNumber;
        Item.AtlasName = _AtlasName;
        Item.AtlasSpriteName = _AtlasSpriteName;

        return Item;
    }

    public override void Copyinstance(Item_Interface original)
    {
        base.Copyinstance(original);

        this.m_SupplieEffectId = ((Supplies_Interface)original).SupplieEffectId;
        this.m_EffectAmount = ((Supplies_Interface)original).EffectAmount;
    }
}
