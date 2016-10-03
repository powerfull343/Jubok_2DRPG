using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

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
    [HideInInspector]
    public bool m_AttachedInvenInst = false;
    
    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        Debug.Log("PlayerDataManager Start");
        MecroMethod.ShowSceneLogConsole("PlayerDataManager Start");
        _Instance.m_AddedStat = new PlayerData();
        _Instance.m_ResultStat = new PlayerData();

        InitEquipedStat();
        InitInventoryInst();
        InitStat();
        MecroMethod.ShowSceneLogConsole("PlayerDataManager End");
    }

    /// <summary>
    /// Setting InventoryManager Prefab
    /// </summary>
    void InitInventoryInst()
    {
        MecroMethod.ShowSceneLogConsole("InitInventoryInst Start");

        _Instance.m_InventoryInstance = Instantiate(
            Resources.Load("UIPanels/InventoryContainer") as GameObject);
        _Instance.m_InventoryInstance.transform.SetParent(this.transform);
        _Instance.m_InventoryInstance.SetActive(false);
        _Instance.m_AttachedInvenInst = false;

        MecroMethod.ShowSceneLogConsole("PlayerDataManager / m_InventoryInstance Null : " + (m_InventoryInstance == null));
        MecroMethod.ShowSceneLogConsole("InitInventoryInst End");
    }

    public void InvenInst_HideAndShow(Transform TargetTrans)
    {
        if (!_Instance.m_AttachedInvenInst)
            InventoryInstMoving_Target(TargetTrans);
        else
            InventoryInstMoving_Origin();
    }

    private void InventoryInstMoving_Target(Transform TargetTrans)
    {
        //MecroMethod.SetPartent(m_InventoryInstance.transform,
        //    TargetTrans);

        //MecroMethod.ShowSceneLogConsole("PlayerDataManager / InventoryInstMoving_Target ReParent Start");
        MecroMethod.ShowSceneLogConsole("PlayerDataManager / m_InventoryInstance Null : " + (_Instance.m_InventoryInstance == null));
        //m_InventoryInstance.transform.SetParent(TargetTrans, false);
        _Instance.m_InventoryInstance.transform.parent = TargetTrans.transform;
        _Instance.m_InventoryInstance.transform.localPosition = Vector3.zero;
        _Instance.m_InventoryInstance.transform.localScale = Vector3.one;
        //MecroMethod.ShowSceneLogConsole("PlayerDataManager / InventoryInstMoving_Target ReParent End");


        UIWidget ChildWidget = _Instance.m_InventoryInstance.GetComponent<UIWidget>();
        if (ChildWidget != null)
            ChildWidget.ParentHasChanged();

        _Instance.m_AttachedInvenInst = true;

        //MecroMethod.ShowSceneLogConsole("PlayerDataManager / InventoryInstMoving_Target / m_InventoryInstance Active : "
        //    + m_InventoryInstance.gameObject.activeSelf);
        _Instance.m_InventoryInstance.gameObject.SetActive(true);

        //MecroMethod.ShowSceneLogConsole("PlayerDataManager / InventoryInstMoving_Target end");
    }

    private void InventoryInstMoving_Origin()
    {
        //m_InventoryInstance.transform.parent = this.transform;
        _Instance.m_AttachedInvenInst = false;
        _Instance.m_InventoryInstance.gameObject.SetActive(false);
    }

    /// <summary>
    /// AddedStat Setting
    /// </summary>
    public void InitEquipedStat()
    {
        Dictionary<EQUIPMENTTYPEID, EquipMent_Interface> SavedEquip =
            DataController.GetInstance().InGameData.ArmedEquip;

        foreach(KeyValuePair<EQUIPMENTTYPEID, EquipMent_Interface> 
            Equip in SavedEquip)
        {
            _Instance.m_AddedStat.Attack += Equip.Value.Attack;
            _Instance.m_AddedStat.Health += Equip.Value.Hp;
        }
    }

    public void InitStat()
    {
        UpdateHealth();
        UpdateMana();
        UpdateStamina();
        UpdateAttack();
    }

    /// <summary>
    /// BattleScene 시작시 플레이어 스텟을 지정해주는 역할을 가졌다.
    /// </summary>
    /// <param name="_Hp"></param>
    /// <param name="_Mp"></param>
    /// <param name="_Stamina"></param>
    public void UpdateStat(out int _Hp, out int _Mp,
        out int _Stamina, out int _Attack)
    {
        _Hp = m_ResultStat.Health;
        _Mp = m_ResultStat.Mana;
        _Stamina = m_ResultStat.Stamina;
        _Attack = m_ResultStat.Attack;
    }

    public void ChangeEquipMent(EquipMent_Interface OldEquip, 
        EquipMent_Interface NewEquip)
    {
        if (OldEquip != null)
        {
            _Instance.m_AddedStat.Health -= OldEquip.Hp;
            _Instance.m_AddedStat.Attack -= OldEquip.Attack;
        }
        if (NewEquip != null)
        {
            _Instance.m_AddedStat.Health += NewEquip.Hp;
            _Instance.m_AddedStat.Attack += NewEquip.Attack;
        }
        InitStat();

    }

    public void UpdateMoney(int Money)
    {
        DataController.GetInstance().InGameData.Money += Money;
    }

    public void UpdateHealth()
    {
        int AddedHealth = _Instance.m_AddedStat.Health +
            (_Instance.m_AddedStat.tStat.Str * 10);

        //Debug.Log("DataController Health : " + DataController.GetInstance().InGameData.Health);
        //Debug.Log(AddedHealth);

        _Instance.m_ResultStat.Health =
            DataController.GetInstance().InGameData.Health +
            (DataController.GetInstance().InGameData.tStat.Str * 10) +
            AddedHealth;

        //Debug.Log(m_ResultStat.Health);
    }

    public void UpdateMana()
    {
        int AddedMana = _Instance.m_AddedStat.Mana +
            (_Instance.m_AddedStat.tStat.Int * 10);

        //Debug.Log("DataController Mana : " + DataController.GetInstance().InGameData.Mana);
        //Debug.Log(AddedMana);

        _Instance.m_ResultStat.Mana =
            DataController.GetInstance().InGameData.Mana +
            (DataController.GetInstance().InGameData.tStat.Int * 10) +
            AddedMana;

        //Debug.Log(m_ResultStat.Mana);
    }

    public void UpdateStamina()
    {
        int AddedStamina = _Instance.m_AddedStat.Stamina +
            (_Instance.m_AddedStat.tStat.Dex * 1);

        //Debug.Log("DataController Stamina : " + DataController.GetInstance().InGameData.Stamina);
        //Debug.Log(AddedStamina);

        _Instance.m_ResultStat.Stamina =
            DataController.GetInstance().InGameData.Stamina +
            (DataController.GetInstance().InGameData.tStat.Dex * 1) +
            AddedStamina;

        //Debug.Log(m_ResultStat.Stamina);
    }

    public void UpdateAttack()
    {
        int AddedAttackStat = _Instance.m_AddedStat.Attack +
            (_Instance.m_AddedStat.tStat.Int * 1);

        _Instance.m_ResultStat.Attack =
            DataController.GetInstance().InGameData.Attack +
            (DataController.GetInstance().InGameData.tStat.Int * 1) +
            AddedAttackStat;
    }

}
