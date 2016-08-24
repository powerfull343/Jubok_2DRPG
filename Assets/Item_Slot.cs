using UnityEngine;
using System.Collections;

public class Item_Slot : MonoBehaviour {

    //private bool m_isExistItem = false;
    //public bool ExistItem
    //{
    //    get { return m_isExistItem; }
    //    set { m_isExistItem = value; }
    //}

    private bool m_isSellerItem = false;
    public bool isSelleritem
    {
        get { return m_isSellerItem; }
        set { m_isSellerItem = value; }
    }

    private Item_Interface_Comp m_ChildItem;
    public Item_Interface_Comp ChildItem
    {
        get { return m_ChildItem; }
        set { m_ChildItem = value; }
    }

    private int m_nRowIndex = 0;
    public int RowIndex
    {
        get { return m_nRowIndex; }
        set { m_nRowIndex = value; }
    }

    private Transform m_ItemIconTrans;
    public Transform ItemIconTrans
    {
        get { return m_ItemIconTrans; }
        set { m_ItemIconTrans = value; }
    }
    
    //public Item_Interface_Comp GetChildItem()
    //{
    //    if (!m_isExistItem)
    //        return null;

    //    Transform SelectedItemTrans = transform.FindChild("Sprite - ItemIcon(Clone)");
    //    if (SelectedItemTrans == null)
    //    {
    //        m_isExistItem = false;
    //        Debug.Log("No Exist item But m_isExistItem is ture");
    //        return null;
    //    }

    //    Item_Interface_Comp SelectedItem =
    //        SelectedItemTrans.GetComponent<Item_Interface_Comp>();

    //    if(SelectedItem == null)
    //    {
    //        m_isExistItem = false;
    //        Debug.Log("No Exist item But m_isExistItem is ture");
    //        return null;
    //    }

    //    return SelectedItem;
    //}

    public void ApplyItemToMerchantInfo()
    {
        //if (!m_isExistItem)
        //    return;

        //Item_Interface_Comp SelectedItem = 
        //    transform.FindChild("Sprite - ItemIcon(Clone)").GetComponent<Item_Interface_Comp>();

        //if (SelectedItem == null)
        //    Debug.LogError("Cannot Find Item Transform but ExistItem is True!!");

        if(m_isSellerItem)
        UI_GroceryStore_SellList.GetInstance().RenderItemInfo(
            this.ChildItem.ItemInfo.itemWeight, this.ChildItem.ItemInfo.ItemValue);
    }
}
