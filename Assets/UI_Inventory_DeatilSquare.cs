using UnityEngine;
using System;
using System.Collections;
using System.Reflection;


public class UI_Inventory_DeatilSquare : MonoBehaviour {

    [SerializeField]
    private UI2DSprite m_ItemIcon2DSprite;
    [SerializeField]
    private UISprite m_ItemIconNGUISprite;
    [SerializeField]
    private UILabel m_ItemNameLabel;
    [SerializeField]
    private UILabel m_ItemCountLabel;
    [SerializeField]
    private UILabel m_ItemTypeLabel;
    [SerializeField]
    private UILabel m_ItemAbliltyLabel;
    [SerializeField]
    private UILabel m_ItemWeightLabel;
    [SerializeField]
    private UILabel m_ItemValueLabel;
    [SerializeField]
    private UILabel m_ItemExpressionLabel;

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemNameLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemCountLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemTypeLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemAbliltyLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemWeightLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemValueLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ItemExpressionLabel);
    }

    private void ResetWindow()
    {
        m_ItemIcon2DSprite.enabled = false;
        m_ItemIconNGUISprite.enabled = false;
    }

    public void OpenDetailItemInfo(Item_Slot SelectedItemSlot)
    {
        if (!SelectedItemSlot.ChildItem)
            return;

        this.gameObject.SetActive(false);
        SettingPosition(SelectedItemSlot.RowIndex);
        SettingStatWindow(SelectedItemSlot.ChildItem);
        //this.gameObject.SetActive(true);
    }

    private void SettingPosition(int GridPosition)
    {
        //아이템 그리드의 위치에 따라서 포지션을 셋팅한다.
        float fHeightPositionY = 0f;

        switch(GridPosition)
        {
            case 0:
                fHeightPositionY = -60f;
                break;

            case 1:
                fHeightPositionY = -100f;
                break;

            case 2:
                fHeightPositionY = 80f;
                break;

            case 3:
                fHeightPositionY = 40f;
                break;

            case 4:
                fHeightPositionY = 0f;
                break;

            case 5:
                fHeightPositionY = -40f;
                break;

            default:
                break;
        }

        transform.localPosition = new Vector3(0f, fHeightPositionY, 0f);
    }

    private void SettingStatWindow(Item_Interface_Comp ItemComp)
    {
        SettingItemIcon(ItemComp);
        SettingLabels(ItemComp);
    }

    private void SettingItemIcon(Item_Interface_Comp _ItemComp)
    {
        if (_ItemComp.ItemInfo.SpriteType == SPRITE_TYPEID.SPRITE_NORMAL)
        {
            m_ItemIcon2DSprite.sprite2D = ItemManager.GetInstance(
                ).ItemInfoToNormalSprite(_ItemComp.ItemInfo);
            m_ItemIcon2DSprite.enabled = true;
        }
        else if (_ItemComp.ItemInfo.SpriteType == SPRITE_TYPEID.SPRITE_NGUISPRITE)
        {
            //셋팅이 덜되었다.
            //m_ItemIconNGUISprite = ItemManager.GetInstance(
            //    ).ItemIcon2Sprites(ItemComp.ItemInfo);
            //m_ItemIcon2DSprite.enabled = true;
        }
    }

    private void SettingLabels(Item_Interface_Comp _ItemComp)
    {
        m_ItemNameLabel.text = _ItemComp.ItemInfo.itemName;
        m_ItemCountLabel.text = "X " + _ItemComp.ItemInfo.itemCount.ToString();
        
        float fCurrentWeight = _ItemComp.ItemInfo.itemWeight * _ItemComp.ItemInfo.itemCount;
        m_ItemWeightLabel.text = _ItemComp.ItemInfo.itemWeight.ToString() + " (kg) " +
            "[00ff00]/[-] " + 
            "[ff00ff]" + fCurrentWeight.ToString() + " (kg)[-]";

        m_ItemValueLabel.text = "$ " + string.Format("{0:#,###}", _ItemComp.ItemInfo.ItemValue);
        m_ItemExpressionLabel.text = _ItemComp.ItemInfo.Explainitem;

        string ItemTypeString = _ItemComp.ItemInfo.itemType.ToString();
        ItemTypeString = ItemTypeString.Substring(5);
        m_ItemTypeLabel.text = ItemTypeString;
        //((EquipMent_Interface)_ItemComp.ItemInfo).EqiupmentId.ToString();

        SetAbillityLabel(_ItemComp.ItemInfo);
    }

    private void SetAbillityLabel(Item_Interface _ItemInfo)
    {
        if (_ItemInfo.itemType == ITEMTYPEID.ITEM_EQUIP)
        {
            if (((EquipMent_Interface)_ItemInfo).Hp == 0)
            {//Attack
                m_ItemAbliltyLabel.text
                    = "Attack : " + ((EquipMent_Interface)_ItemInfo).Attack.ToString();
            }
            else
            {//Hp
                m_ItemAbliltyLabel.text
                    = "Hp : " + ((EquipMent_Interface)_ItemInfo).Hp.ToString();
            }
        }
        else if(_ItemInfo.itemType == ITEMTYPEID.ITEM_POTION ||
            _ItemInfo.itemType == ITEMTYPEID.ITEM_FOOD)
        {
            string EffectType = ((Supplies_Interface)_ItemInfo).SupplieEffectId.ToString();
            //EFFECT_ 
            EffectType = EffectType.Substring(7);

            m_ItemAbliltyLabel.text = EffectType + " : " + ((Supplies_Interface)_ItemInfo).EffectAmount.ToString();
        }
        else
            m_ItemAbliltyLabel.text = "None";
    }


}


