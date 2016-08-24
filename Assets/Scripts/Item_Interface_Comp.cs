using UnityEngine;
using System.Collections;

public class Item_Interface_Comp : MonoBehaviour {

    private Item_Interface m_ItemInfo;
    public Item_Interface ItemInfo
    {
        get { return m_ItemInfo; }
        set { m_ItemInfo = value; }
    }
}
