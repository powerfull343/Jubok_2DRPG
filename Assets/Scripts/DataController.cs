using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;

public class DataController : MonoBehaviour {

    private static DataController _instance;
    private PlayerData m_InGameData;
    public PlayerData InGameData
    {
        get { return m_InGameData; }
        set { m_InGameData = value; }
    }

    private Player_QuestData m_QuestGameData;
    public Player_QuestData QuestGameData
    {
        get { return m_QuestGameData; }
        set { m_QuestGameData = value; }
    }

    //Debuging Variable
    private bool m_isDataLoad = false;

    private DataController()
    {

    }

    public static DataController GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        Debug.Log("DataController Awaking");
        Debug.Log("_Instance null : " + (_instance == null));
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if(_instance != this)
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
        InitDataController();
        //Invoke("InitDataController", 0.1f);
    }

    void InitDataController()
    {
        Debug.Log("m_InGameData : " + (m_InGameData == null));
        Debug.Log("m_QuestGameData : " + (m_QuestGameData == null));
        if (m_InGameData == null)
        {
            Load();
            m_isDataLoad = true;
        }

        //Debug.Log("buildIndex : " + SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //PlayerDataManager Loading
            GameObject PlayerDataMgr = Instantiate(
                Resources.Load("DataObjects/PlayerDataCalc")
                as GameObject);
            Debug.Log("PlayerDataManager Null : " + (PlayerDataMgr == null));
            PlayerDataMgr.transform.SetParent(_instance.transform, false);
            PlayerDataMgr.SetActive(true);

            GameObject LobbyController = Instantiate(
                Resources.Load("DataObjects/LobbyManager") as GameObject);
            Debug.Log("LobbyController Null : " + (LobbyController == null));
            LobbyController.transform.SetParent(_instance.transform, false);
            LobbyController.SetActive(true);

            GameObject PanelController = Instantiate(
                Resources.Load("DataObjects/PanelManager") as GameObject);
            Debug.Log("PanelManager Null : " + (PanelController == null));
            PanelController.transform.SetParent(_instance.transform, false);
            PanelController.SetActive(true);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(
        //    Application.persistentDataPath + "/PlayerData.dat");
        FileStream file = File.Create(
            Application.dataPath + "/PlayerData.dat");

        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(Application.dataPath);

        PlayerData data = new PlayerData();
        data.Health = m_InGameData.Health;
        data.Mana = m_InGameData.Mana;
        data.Stamina = m_InGameData.Stamina;
        data.Money = m_InGameData.Money;
        data.Inventory = m_InGameData.Inventory;
        data.ArmedEquip = m_InGameData.ArmedEquip;
        data.tStat = m_InGameData.tStat;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        m_InGameData = new PlayerData();
        BinaryFormatter bf = new BinaryFormatter();

        //Debug.Log(Application.persistentDataPath);

        try
        {
            //FileStream file = File.Open(
            //    Application.persistentDataPath + "/PlayerData.dat",
            //    FileMode.Open);
            FileStream file = File.Open(
                Application.dataPath + "/PlayerData.dat",
                FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            m_InGameData.Health = data.Health;
            m_InGameData.Mana = data.Mana;
            m_InGameData.Attack = data.Attack;
            m_InGameData.Stamina = data.Stamina;
            m_InGameData.Money = data.Money;
            m_InGameData.Inventory = data.Inventory;
            m_InGameData.ArmedEquip = data.ArmedEquip;
            m_InGameData.tStat = data.tStat;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
            FirstPlay();
        }
        catch (IsolatedStorageException e)
        {
            Debug.Log(e.Message);
            FirstPlay();
        }

        QuestLoad();
    }

    public void FirstPlay()
    {
        m_InGameData.Health = 20;
        m_InGameData.Mana = 20;
        m_InGameData.Attack = 1;
        m_InGameData.Stamina = 10;
        m_InGameData.Money = 2000;

        m_InGameData.tStat = new tagStatInfo(1, 50f, 1, 25f, 1, 80f);
        FirstSettingItems();
        FirstSettingEquipItem();

        Save();
    }

    public void QuestSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(
            Application.dataPath + "/QuestData.dat");

        Player_QuestData data = new Player_QuestData();
        data = m_QuestGameData;

        bf.Serialize(file, data);
        file.Close();
    }

    public void QuestLoad()
    {
        m_QuestGameData = new Player_QuestData();
        BinaryFormatter bf = new BinaryFormatter();

        try
        {
            FileStream file = File.Open(
                Application.dataPath + "/QuestData.dat",
                FileMode.Open);

            Player_QuestData data = 
                (Player_QuestData)bf.Deserialize(file);
            file.Close();

            m_QuestGameData = data;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            QuestSave();
        }
    }

    private void FirstSettingItems()
    {
        m_InGameData.Inventory = new Dictionary<string, List<Item_Interface>>();

        Item_Interface Test1 =
            Supplies_Interface.CreateSupplies("Apple", 10, 0.3f, SUPPLIESEFFECTID.EFFECT_STAMINA, 10
            , ITEMTYPEID.ITEM_FOOD, ITEMGRADEID.ITEMGRADE_NORMAL, SPRITE_TYPEID.SPRITE_NORMAL, NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2,
            78);
        Test1.itemCount = 10;

        EquipMent_Interface Test2 = EquipMent_Interface.CreateEquipMent("Sword", 100, 3f, 3, 0,
            ITEMTYPEID.ITEM_EQUIP, ITEMGRADEID.ITEMGRADE_NORMAL,
            EQUIPMENTTYPEID.EQUIP_WEAPON, SPRITE_TYPEID.SPRITE_NORMAL,
            NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2, 2);

        EquipMent_Interface Test3 = EquipMent_Interface.CreateEquipMent("Sword", 100, 3f, 3, 0,
            ITEMTYPEID.ITEM_EQUIP, ITEMGRADEID.ITEMGRADE_NORMAL,
            EQUIPMENTTYPEID.EQUIP_WEAPON, SPRITE_TYPEID.SPRITE_NORMAL,
            NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2, 2);

        List<Item_Interface> MultiItem = new List<Item_Interface>();
        MultiItem.Add(Test1);
        m_InGameData.Inventory.Add(Test1.itemName, MultiItem);

        List<Item_Interface> MultiItem2 = new List<Item_Interface>();
        MultiItem2.Add(Test2);
        MultiItem2.Add(Test3);
        m_InGameData.Inventory.Add(Test2.itemName, MultiItem2);
        Debug.Log("m_InGameData.Inventory Count : " + m_InGameData.Inventory.Count);
    }

    private void FirstSettingEquipItem()
    {
        m_InGameData.ArmedEquip = 
            new Dictionary<EQUIPMENTTYPEID, EquipMent_Interface>();
    }
}

[Serializable]
public class PlayerData
{
    public int Health;
    public int Mana;
    public int Attack;
    public int Stamina;
    public int Money;

    public tagStatInfo tStat;

    public Dictionary<string, List<Item_Interface>> Inventory;
    public Dictionary<EQUIPMENTTYPEID, EquipMent_Interface> ArmedEquip;
}

[Serializable]
public struct tagStatInfo
{
    private int Strength;
    public int Str
    {
        get { return Strength; }
        set { Strength = value; }
    }
    private float fStrengthExp;
    public float StrengthExp
    {
        get { return fStrengthExp; }
        set { fStrengthExp = value; }
    }

    private int Dexterity;
    public int Dex
    {
        get { return Dexterity; }
        set { Dexterity = value; }
    }
    private float fDexterityExp;
    public float DexterityExp
    {
        get { return fDexterityExp; }
        set { fDexterityExp = value; }
    }

    private int Intelligence;
    public int Int
    {
        get { return Intelligence; }
        set { Intelligence = value; }
    }
    private float fIntelligenceExp;
    public float IntelligenceExp
    {
        get { return fIntelligenceExp; }
        set { fIntelligenceExp = value; }
    }

    private Dictionary<string, int> AddedStat;
    public Dictionary<string, int> AddStat
    {
        get { return AddedStat; }
        set { AddedStat = value;}
    }

    public tagStatInfo(int _str, float _fStrExp,
        int _dex, float _fDexExp, int _int, float _fIntExp)
    {
        Strength = _str;
        Dexterity = _dex;
        Intelligence = _int;
        fStrengthExp = _fStrExp;
        fDexterityExp = _fDexExp;
        fIntelligenceExp = _fIntExp;
        AddedStat = new Dictionary<string, int>();
    }
}

[Serializable]
public struct tagJobStatInfo
{
    private JOBKINDSID mJobID;
    public JOBKINDSID Job
    {
        get { return mJobID; }
        set { mJobID = value; }
    }
    //Job Passive SkillName
    private Dictionary<string, SkillInfo> mPassiveSkill;
    public Dictionary<string, SkillInfo> PassiveSkill
    {
        get { return mPassiveSkill; }
        set { mPassiveSkill = value; }
    }
}

[Serializable]
public struct SkillInfo
{
    private int mSkillLevel;
    public int SkillLevel
    {
        get { return mSkillLevel; }
        set { mSkillLevel = value; }
    }

    private int mSkillExp;
    public int SkillExp
    {
        get { return mSkillExp; }
        set { mSkillExp = value; }
    }
    
}