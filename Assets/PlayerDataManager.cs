using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataManager : 
    Singleton<PlayerDataManager>
{
    //Add Inventory Stat, what else
    private PlayerData m_AddedStat;
    //Final Calced Datas
    private PlayerData m_ResultStat;
    public PlayerData ResultStat
    {
        get { return m_ResultStat; }
        set { m_ResultStat = value; }
    }

    private GameObject m_InventoryInstance;
    public GameObject InventoryInst
    { get { return m_InventoryInstance; } }
    public bool m_AttachedInvenInst = false;
    
    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        m_AddedStat = new PlayerData();
        m_ResultStat = new PlayerData();

        UpdateEquipedStat();
        InitInventoryInst();
        InitStat();
    }

    void InitInventoryInst()
    {
        m_InventoryInstance = Instantiate(
            Resources.Load("UIPanels/InventoryContainer") as GameObject);
        m_InventoryInstance.transform.parent = this.transform;
        m_InventoryInstance.SetActive(false);
        m_AttachedInvenInst = false;
    }

    public void InvenInst_HideAndShow(Transform TargetTrans)
    {
        if (!m_AttachedInvenInst)
            InventoryInstMoving_Target(TargetTrans);
        else
            InventoryInstMoving_Origin();
    }

    private void InventoryInstMoving_Target(Transform TargetTrans)
    {
        m_InventoryInstance.transform.parent = TargetTrans;
        m_InventoryInstance.transform.localPosition = Vector3.zero;
        m_InventoryInstance.transform.localScale = Vector3.one;

        m_AttachedInvenInst = true;
        m_InventoryInstance.gameObject.SetActive(true);
    }

    private void InventoryInstMoving_Origin()
    {
        //m_InventoryInstance.transform.parent = this.transform;
        m_AttachedInvenInst = false;
        m_InventoryInstance.gameObject.SetActive(false);
    }

    public void UpdateEquipedStat()
    {

    }

    public void InitStat()
    {
        UpdateHealth();
        UpdateMana();
        UpdateStamina();
    }

    public void UpdateStat(out int _Hp, out int _Mp, out int _Stamina)
    {
        _Hp = m_ResultStat.Health;
        _Mp = m_ResultStat.Mana;
        _Stamina = m_ResultStat.Stamina;
    }

    public void UpdateMoney(int Money)
    {
        DataController.GetInstance().InGameData.Money += Money;
    }

    public void UpdateHealth()
    {
        int AddedHealth = m_AddedStat.Health +
            (m_AddedStat.tStat.Str * 10);


        //Debug.Log("DataController Health : " + DataController.GetInstance().InGameData.Health);
        //Debug.Log(AddedHealth);

        m_ResultStat.Health =
            DataController.GetInstance().InGameData.Health +
            (DataController.GetInstance().InGameData.tStat.Str * 10) +
            AddedHealth;

        //Debug.Log(m_ResultStat.Health);
    }

    public void UpdateMana()
    {
        int AddedMana = m_AddedStat.Mana +
            (m_AddedStat.tStat.Int * 10);

        //Debug.Log("DataController Mana : " + DataController.GetInstance().InGameData.Mana);
        //Debug.Log(AddedMana);

        m_ResultStat.Mana =
            DataController.GetInstance().InGameData.Mana +
            (DataController.GetInstance().InGameData.tStat.Int * 10) +
            AddedMana;

        //Debug.Log(m_ResultStat.Mana);
    }

    public void UpdateStamina()
    {
        int AddedStamina = m_AddedStat.Stamina +
            (m_AddedStat.tStat.Dex * 1);

        //Debug.Log("DataController Stamina : " + DataController.GetInstance().InGameData.Stamina);
        //Debug.Log(AddedStamina);

        m_ResultStat.Stamina =
            DataController.GetInstance().InGameData.Stamina +
            (DataController.GetInstance().InGameData.tStat.Dex * 1) +
            AddedStamina;

        //Debug.Log(m_ResultStat.Stamina);
    }

}
