using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class EquipMent_Interface : Item_Interface
{
    public EquipMent_Interface() : base()
    {
        itemType = ITEMTYPEID.ITEM_EQUIP;
    }

    protected EQUIPMENTTYPEID m_EqiupmentId;
    public EQUIPMENTTYPEID EqiupmentId
    {
        get { return m_EqiupmentId; }
        set { m_EqiupmentId = value; }
    }

    protected int m_Attack;
    public int Attack
    {
        get { return m_Attack; }
        set { m_Attack = value; }
    }

    protected int m_Hp;
    public int Hp
    {
        get { return m_Hp; }
        set { m_Hp = value; }
    }

    /// <summary>
    /// if(SpriteTypeId == NORMAL_SPRITE) you must setting SpriteIconType, SpriteIconNumber
    /// else if(SpriteTypeId == NGUI_SPRITE) setting AtalsName, AtlasSpriteName
    /// </summary>
    public static EquipMent_Interface CreateEquipMent(string _itemName, int _itemValue,
        float _itemWeight, int _Attack, int _Hp, 
        ITEMTYPEID _ItemTypeId, ITEMGRADEID _ItemGradeId,
        EQUIPMENTTYPEID _EquipmentId,
        SPRITE_TYPEID _SpriteTypeId,
        //SpriteTypeId == NORMAL_SPRITE
        NORMAL_SPRITE_ICONS _SpriteIconType, int _SpriteIconNumber,
        //SpriteTypeId == NGUI_SPRITE
        string _AtlasName = "", string _AtlasSpriteName = "")
    {
        EquipMent_Interface Item = new EquipMent_Interface();
        Item_Interface.SetItemInfo((Item_Interface)Item, _itemName, _itemValue,
            _itemWeight, 1, _ItemTypeId, _ItemGradeId);
        Item.Attack = _Attack;
        Item.Hp = _Hp;
        Item.EqiupmentId = _EquipmentId;

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

        this.m_EqiupmentId = ((EquipMent_Interface)original).EqiupmentId;
        this.Hp = ((EquipMent_Interface)original).Hp;
        this.Attack = ((EquipMent_Interface)original).Attack;
    }

}