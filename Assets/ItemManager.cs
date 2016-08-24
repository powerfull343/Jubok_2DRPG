using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public enum NORMAL_SPRITE_ICONS
{
    NORMAL_SPRITE_NONE = -1,
    NORMAL_SPRITE_ICON2,
    NORMAL_SPRITE_MAX,
};

[Serializable]
public class ItemManager : 
    Singleton<ItemManager>
{
    private Sprite[] m_ItemIcon2Sprite;
    public Sprite[] ItemIcon2Sprites
    {
        get { return m_ItemIcon2Sprite; }
    }

    private GameObject m_ItemSpritePrefab;
    public GameObject ItemSpritePrefab
    {
        get { return m_ItemSpritePrefab; }
    }

    private Dictionary<string, List<Item_Interface>> m_InventoryItems;
    public Dictionary<string, List<Item_Interface>> InventoryItems
    {
        get { return m_InventoryItems; }
        set { m_InventoryItems = value; }
    }

    void Awake()
    {
        CreateInstance();
        Debug.Log("ItemManager Awaking");
        m_ItemIcon2Sprite = Resources.LoadAll<Sprite>("ItemIcons/ItemIcon02");
        m_ItemSpritePrefab =
            Resources.Load("ItemIcons/Sprite - ItemIcon") as GameObject;
        if (m_ItemSpritePrefab == null)
            Debug.LogError("Item Sprite Icon Cannot Load");

        m_InventoryItems = DataController.GetInstance().InGameData.Inventory;

        if(m_InventoryItems.Count == 0)
            Debug.LogError("m_InventoryItems Cannot Load");
    }

    public GameObject ItemInfoToGameObject(Item_Interface ItemInfo)
    {
        GameObject newItemObject = Instantiate(m_ItemSpritePrefab) as GameObject;
        Mecro.MecroMethod.CheckExistObejct<GameObject>(newItemObject);

        switch (ItemInfo.SpriteType)
        {
            case SPRITE_TYPEID.SPRITE_NORMAL:
                UI2DSprite icon2DSprite = Mecro.MecroMethod.CheckGetComponent<UI2DSprite>(newItemObject);
                if (!icon2DSprite.enabled)
                    icon2DSprite.enabled = true;
                icon2DSprite.sprite2D = ItemInfoToNormalSprite(ItemInfo);
                break;

            case SPRITE_TYPEID.SPRITE_NGUISPRITE:
                break;
        }


        return newItemObject;
    }

    public Sprite ItemInfoToNormalSprite(Item_Interface ItemInfo)
    {
        Sprite ResultSprite = null;
        if (ItemInfo.SpriteType != SPRITE_TYPEID.SPRITE_NORMAL)
        {
            Debug.LogError("Cannot Correctly Type");
            return ResultSprite;
        }

        switch(ItemInfo.NormalSprite_iconType)
        {
            case NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2:
                ResultSprite = m_ItemIcon2Sprite[ItemInfo.NormalSprite_iconNumber];
                break;
        }

        return ResultSprite;
    }
}
