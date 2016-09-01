using UnityEngine;
using System.Collections;
using Mecro;

public class UI_GroceryStore_SellList_DetailWindow : MonoBehaviour {

    private UIPanel m_OwnPanel;
    [SerializeField]
    private UILabel m_ItemNameLabel;
    [SerializeField]
    private UI2DSprite m_ItemNormalSprite;
    [SerializeField]
    private UISprite m_ItemAtlasSprite;
    [SerializeField]
    private UILabel m_ItemTypeLabel;
    [SerializeField]
    private UILabel m_ItemStat;

    void OnEnable()
    {
        if(!m_OwnPanel)
            m_OwnPanel = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);

        if(m_OwnPanel.alpha <= 0f)
            TweenAlpha.Begin(this.gameObject, 0.2f, 1f);
    }

	// Use this for initialization
	void Start () {
        
        MecroMethod.CheckExistComponent<UILabel>(m_ItemNameLabel);
        MecroMethod.CheckExistComponent<UI2DSprite>(m_ItemNormalSprite);
        MecroMethod.CheckExistComponent<UISprite>(m_ItemAtlasSprite);
        MecroMethod.CheckExistComponent<UILabel>(m_ItemTypeLabel);
        MecroMethod.CheckExistComponent<UILabel>(m_ItemStat);
    }

    public void SetItemData(GameObject SelectedItem)
    {
        Item_Interface_Comp ItemComp =
            MecroMethod.CheckGetComponent<Item_Interface_Comp>(SelectedItem);

        //Item Name Label Setting
        m_ItemNameLabel.text = ItemComp.ItemInfo.itemName;

        //Item icon Setting
        SetItemIcon(ItemComp);

        //Item Type Label Setting
        string ItemType = ItemComp.ItemInfo.itemType.ToString();
        m_ItemTypeLabel.text = ItemType.Substring(5);

        SetItemAbillty(ItemComp.ItemInfo);
    }

    private void SetItemIcon(Item_Interface_Comp SelectedItem)
    {
        if(SelectedItem.ItemInfo.SpriteType ==
            SPRITE_TYPEID.SPRITE_NORMAL)
        {
            UI2DSprite NormalSprite =
                MecroMethod.CheckGetComponent<UI2DSprite>(SelectedItem.gameObject);

            m_ItemNormalSprite.sprite2D = NormalSprite.sprite2D;
        }
        else if(SelectedItem.ItemInfo.SpriteType ==
            SPRITE_TYPEID.SPRITE_NGUISPRITE)
        {
            UISprite AtlasSprite =
                MecroMethod.CheckGetComponent<UISprite>(SelectedItem.gameObject);

            m_ItemAtlasSprite.atlas = AtlasSprite.atlas;
            m_ItemAtlasSprite.spriteName = AtlasSprite.spriteName;
        }
    }

    private void SetItemAbillty(Item_Interface SelectedItem)
    {
        if (SelectedItem.itemType == ITEMTYPEID.ITEM_EQUIP)
            AbilltySetting_EquipMent(SelectedItem);
        else if (SelectedItem.itemType == ITEMTYPEID.ITEM_FOOD ||
            SelectedItem.itemType == ITEMTYPEID.ITEM_POTION)
            AbilltySetting_Supplies(SelectedItem);


    }

    private void AbilltySetting_EquipMent(Item_Interface SelectedItem)
    {
        if (((EquipMent_Interface)SelectedItem).Attack > 0)
        {
            m_ItemStat.text = "[00ff00]Attack : [-]" +
                ((EquipMent_Interface)SelectedItem).Attack.ToString();
        }
        else
        {
            m_ItemStat.text = "[ff0000]Hp : [-]" +
                ((EquipMent_Interface)SelectedItem).Hp.ToString();
        }
    }

    private void AbilltySetting_Supplies(Item_Interface SelectedItem)
    {
        SUPPLIESEFFECTID Selecteditemtype =
            ((Supplies_Interface)SelectedItem).SupplieEffectId;

        string EffectType = Selecteditemtype.ToString();
        EffectType = EffectType.Substring(7);

        switch(Selecteditemtype)
        {
            case SUPPLIESEFFECTID.EFFECT_HP:
                m_ItemStat.text = "[ff0000]" + EffectType + "[-]" +
                    ((Supplies_Interface)SelectedItem).EffectAmount;
                break;

            case SUPPLIESEFFECTID.EFFECT_MANA:
                m_ItemStat.text = "[0000ff]" + EffectType + "[-]" +
                    ((Supplies_Interface)SelectedItem).EffectAmount;
                break;

            case SUPPLIESEFFECTID.EFFECT_STAMINA:
                m_ItemStat.text = "[00ff00]" + EffectType + "[-]" +
                    ((Supplies_Interface)SelectedItem).EffectAmount;
                break;

            default:
                m_ItemStat.text = "Cannot Find Ablil type";
                break;
        }
    }

}
