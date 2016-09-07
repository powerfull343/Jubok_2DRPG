using UnityEngine;
using System.Collections;
using Mecro;

public class UI_DetailPlayerStat_EquipStat : MonoBehaviour {

    [SerializeField]
    private Transform m_InventoryTrans;

    void OnEnable()
    {
        MecroMethod.CheckGetComponent<Transform>(m_InventoryTrans);
        PlayerDataManager.GetInstance().InvenInst_HideAndShow(m_InventoryTrans);
    }

    void OnDisable()
    {
        Debug.Log("disable");
        PlayerDataManager.GetInstance().InvenInst_HideAndShow(m_InventoryTrans);
    }
        

}
