using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class ItemDropManager :
    Singleton<ItemDropManager>
{
    /// <summary>
    /// Monster Drops items
    /// Key = MonsterName, Value = items
    /// </summary>
    private Dictionary<string, List<Item_Interface>> m_LoadedItemDropList
         = new Dictionary<string, List<Item_Interface>>();
    public Dictionary<string, List<Item_Interface>> LoadedItemDropList
    {
        get { return m_LoadedItemDropList; }
        set { m_LoadedItemDropList = value; }
    }

    /// <summary>
    /// Loaded Chest Prefabs
    /// </summary>
    private Dictionary<ITEMGRADEID, GameObject> m_LoadedChestPrefabs
        = new Dictionary<ITEMGRADEID, GameObject>();
    public Dictionary<ITEMGRADEID, GameObject> LoadedChestPrefabs
    {
        get { return m_LoadedChestPrefabs; }
        set { m_LoadedChestPrefabs = value; }
    }

    /// <summary>
    /// Obtained item List
    /// </summary>
    private Dictionary<ITEMGRADEID, List<Item_Interface>> m_ObtainedItems
        = new Dictionary<ITEMGRADEID, List<Item_Interface>>();
    public Dictionary<ITEMGRADEID, List<Item_Interface>> ObtainedItems
    {
        get { return m_ObtainedItems; }
        set { m_ObtainedItems = value; }
    }

    private GameObject m_CoinPrefab;
    public GameObject CoinPrefab
    { get { return m_CoinPrefab; } }

    [SerializeField]
    private Transform m_ObtainTrans;
    private Vector3 m_ObtainPosition;
    public Vector3 ObtainPosition
    { get { return m_ObtainPosition; } }

    //it Must NGUI Space Position
    [SerializeField]
    private Transform m_ObtainParent;
    public Transform ObtainParent
    {
        get { return m_ObtainParent; }
    }

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        CheckingPropertys();
        LoadChestGameObjects();
        LoadCoinObjects();
        LoadDropList();
    }

    private void CheckingPropertys()
    {
        MecroMethod.CheckExistComponent<Transform>(m_ObtainParent);
        MecroMethod.CheckExistComponent<Transform>(m_ObtainTrans);
        m_ObtainPosition = MecroMethod.GetWorldPos(m_ObtainTrans, Vector3.zero);

        //m_ObtainPosition = MecroMethod.NormalToNGUIWorldPos(m_ObtainTrans.position);
        
        //Debug.LogError(m_ObtainPosition);
    }

    private void LoadChestGameObjects()
    {
        for(int i = 0; i < (int)ITEMGRADEID.ITEMGRADE_MAX; ++i)
        {
            m_LoadedChestPrefabs.Add((ITEMGRADEID)i, 
            ConvertEnumToChestPrefab((ITEMGRADEID)i));
        }
    }

    private void LoadCoinObjects()
    {
        m_CoinPrefab = Resources.Load("BattleScene/Item/Chests/Coin") as GameObject;
    }

    private GameObject ConvertEnumToChestPrefab(ITEMGRADEID itemGrade)
    {
        GameObject LoadChestPrefab;
        string ChestPath = "BattleScene/item/Chests/";

        string itemgradestring = itemGrade.ToString();

        //EX)ITEMGRADE_NORMAL -> ORMAL
        string ChestFullPath = itemgradestring.Substring(11);

        //EX)ORMAL -> ormal
        ChestFullPath = ChestFullPath.ToLower();

        //EX)Normal -> N + ormal;
        ChestFullPath = itemgradestring[10].ToString() + ChestFullPath + "Chest";

        //EX) "BattleScene/item/Chests" + NormalChest
        ChestFullPath = ChestPath + ChestFullPath;

        LoadChestPrefab = Resources.Load(ChestFullPath) as GameObject;

        return LoadChestPrefab;
    }

    public void LoadDropList()
    {
        Item_Interface item = new Item_Interface();
        item.DropItemRate = 1000;
        item.itemName = "Bone";
        item.itemGrade = ITEMGRADEID.ITEMGRADE_NORMAL;
        item.itemWeight = 1f;
        item.itemType = ITEMTYPEID.ITEM_GEM;
        item.itemCount = 2;

        Item_Interface item2 = new Item_Interface();
        item2.DropItemRate = 700;
        item2.itemName = "item2";
        item2.itemGrade = ITEMGRADEID.ITEMGRADE_MAGIC;

        Item_Interface item3 = new Item_Interface();
        item3.DropItemRate = 500;
        item3.itemName = "item3";
        item3.itemGrade = ITEMGRADEID.ITEMGRADE_RARE;

        Item_Interface item4 = new Item_Interface();
        item4.DropItemRate = 300;
        item4.itemName = "item4";
        item4.itemGrade = ITEMGRADEID.ITEMGRADE_UNIQUE;

        Item_Interface item5 = new Item_Interface();
        item5.DropItemRate = 100;
        item5.itemName = "item5";
        item5.itemGrade = ITEMGRADEID.ITEMGRADE_LEGEND;

        List<Item_Interface> TestList = new List<Item_Interface>();
        TestList.Add(item);
        TestList.Add(item4);
        TestList.Add(item5);

        List<Item_Interface> TestList2 = new List<Item_Interface>();
        TestList2.Add(item);
        TestList2.Add(item2);
        TestList2.Add(item3);


        m_LoadedItemDropList.Add("Skeleton01", TestList);
        m_LoadedItemDropList.Add("Skeleton02", TestList2);
    }

    public void DropItem(string MonsterName, Vector3 MonsterPosition)
    {
        if (!m_LoadedItemDropList.ContainsKey(MonsterName))
        {
            Debug.Log("Cannot Find Item");
            return;
        }

        //Debug.LogError(MonsterName);
        List<Item_Interface> itemList = m_LoadedItemDropList[MonsterName];

        for (int i = 0; i < itemList.Count; ++i)
        {
            int nRdmIdx = Random.Range(0, 1000);

            if (nRdmIdx <= itemList[i].DropItemRate)
            {
                GameObject ChestObject = Instantiate(
                    m_LoadedChestPrefabs[itemList[i].itemGrade]) as GameObject;

                ChestObject.SetActive(false);
                ChestObject.transform.parent = m_ObtainParent;
                ChestObject.transform.position =
                    MecroMethod.NormalToNGUIWorldPos(MonsterPosition);
                ChestObject.transform.localScale = Vector3.one;
                ChestObject.SetActive(true);
            }
        }
    }

    public void DropCoin(Vector3 MonsterPosition, int MoneySize, 
        int MinMoneyDrop = 3, int MaxMoneyDrop = 5)
    {
        int nRdmIdx = UnityEngine.Random.Range(MinMoneyDrop, MaxMoneyDrop);

        for (int i = 0; i < nRdmIdx; ++i)
        {
            GameObject CoinInst = Instantiate(m_CoinPrefab) as GameObject;
            CoinInst.SetActive(false);
            CoinInst.transform.parent = m_ObtainParent;
            CoinInst.transform.position =
                MecroMethod.NormalToNGUIWorldPos(MonsterPosition);
            CoinInst.transform.localScale = Vector3.one;

            MecroMethod.CheckGetComponent<ChestController>(CoinInst).MoneySize = MoneySize;
            
            CoinInst.SetActive(true);
        }
    }

    private void DropItemPosition()
    {

    }

    private void DropRate()
    {
        
    }
}
