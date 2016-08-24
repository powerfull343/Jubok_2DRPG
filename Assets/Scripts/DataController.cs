using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

public class DataController : MonoBehaviour {

    private static DataController _instance;
    private PlayerData m_InGameData;
    public PlayerData InGameData
    {
        get { return m_InGameData; }
        set { m_InGameData = value; }
    }

    //Debuging Variable
    [SerializeField]
    private bool m_isStartLobbyScene = false;

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
        if (_instance == null)
        {
            m_InGameData = new PlayerData();
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
        if (m_isStartLobbyScene)
        {
            Debug.Log("SceneNumber : " + Application.loadedLevel);
            if (Application.loadedLevel == 1)
            {
                Load();
            }
        }

        //PlayerDataManager.GetInstance().InitPlayerDataManager();

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(
            Application.persistentDataPath + "/PlayerData.dat");

        PlayerData data = new PlayerData();
        data.Health = m_InGameData.Health;
        data.Mana = m_InGameData.Mana;
        data.Stamina = m_InGameData.Stamina;
        data.Money = m_InGameData.Money;
        data.Inventory = m_InGameData.Inventory;
        data.tStat = m_InGameData.tStat;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();

        Debug.Log(Application.persistentDataPath);

        try
        {
            FileStream file = File.Open(
            Application.persistentDataPath + "/PlayerData.dat",
            FileMode.Open);
        
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            m_InGameData.Health = data.Health;
            m_InGameData.Mana = data.Mana;
            m_InGameData.Attack = data.Attack;
            m_InGameData.Stamina = data.Stamina;
            m_InGameData.Money = data.Money;
            m_InGameData.Inventory = data.Inventory;
            m_InGameData.tStat = data.tStat;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
            FirstPlay();
        }
    }

    public void FirstPlay()
    {
        m_InGameData.Health = 20;
        m_InGameData.Mana = 20;
        m_InGameData.Attack = 1;
        m_InGameData.Stamina = 10;
        m_InGameData.Money = 2000;
        FirstSettingItems();
        m_InGameData.tStat = new tagStatInfo(1, 50f, 1, 25f, 1, 80f);

        Save();
    }

    private void FirstSettingItems()
    {
        m_InGameData.Inventory = new Dictionary<string, List<Item_Interface>>();

        Item_Interface Test1 = new Item_Interface();
        Test1 = Item_Interface.CreateNormalSpriteItem("Apple", 10, 0.3f, 10,
            ITEMTYPEID.ITEM_FOOD, ITEMGRADEID.ITEMGRADE_NORMAL,
            NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2, 78);

        EquipMent_Interface Test2 = new EquipMent_Interface();
        Test2 = EquipMent_Interface.CreateEquipMent("Sword", 100, 3f, 3, 0,
            ITEMTYPEID.ITEM_EQUIP, ITEMGRADEID.ITEMGRADE_NORMAL,
            EQUIPMENTTYPEID.EQUIP_WEAPON, SPRITE_TYPEID.SPRITE_NORMAL,
            NORMAL_SPRITE_ICONS.NORMAL_SPRITE_ICON2, 2);

        List<Item_Interface> MultiItem = new List<Item_Interface>();
        MultiItem.Add(Test1);
        m_InGameData.Inventory.Add(Test1.itemName, MultiItem);

        List<Item_Interface> MultiItem2 = new List<Item_Interface>();
        MultiItem2.Add(Test2);
        MultiItem2.Add(Test2);
        m_InGameData.Inventory.Add(Test2.itemName, MultiItem2);
        Debug.Log("m_InGameData.Inventory Count : " + m_InGameData.Inventory.Count);
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