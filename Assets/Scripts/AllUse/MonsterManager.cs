using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Mecro;
using System.Linq;

public class MonsterManager
    : Singleton<MonsterManager>
{
    public enum MonsterSummonID
    {
        SUMMON_NORMAL,
        SUMMON_ELITE,
        SUMMON_SPECIAL,
        SUMMON_MAX,
    };

    /// <summary>
    /// 로드된 몬스터 항목(일반등급)
    /// </summary>
    public static Dictionary<MonsterKey_Extension, LoadedMonsterElement> FieldMonsterData
        = new Dictionary<MonsterKey_Extension, LoadedMonsterElement>();

    /// <summary>
    /// 로드된 몬스터 항목(상위등급)
    /// </summary>
    public static Dictionary<MonsterKey_Extension, LoadedMonsterElement> FieldEliteMonsterData
        = new Dictionary<MonsterKey_Extension, LoadedMonsterElement>();
    /// <summary>
    /// 로드된 몬스터 항목(특수한조건)
    /// </summary>
    public static Dictionary<MonsterKey_Extension, LoadedMonsterElement> FieldSpecialMonsterData
        = new Dictionary<MonsterKey_Extension, LoadedMonsterElement>();

    /// <summary>
    /// 현재 소환된 몬스터 항목
    /// </summary>
    //public Dictionary<string, List<GameObject>> MonsterList
    //    = new Dictionary<string, List<GameObject>>();
    public List<GameObject> MonsterList
        = new List<GameObject>();

    //Variables
    //public static int MonsterCount = 0;
    public static bool m_isMonsterExist = false;
    public static int MonsterMaxCount = 10;

    public float StartRegenTime = 10f;
    public float RegenMinWidth = 1f;
    public float RegenMaxWidth = 3f;
    public Transform InFieldPosition;
    public Transform OutFieldPosition;
    
    /// <summary>
    /// it Use Cannot Create over One Boss Object
    /// </summary>
    private bool m_isCheckBossExist = false;
    public bool CheckBossExist
    {
        get { return m_isCheckBossExist; }
        set { m_isCheckBossExist = value; }
    }

    private IEnumerator m_iRegenMonster;
    public IEnumerator iRegenMonster
    {
        get { return m_iRegenMonster; }
        set { m_iRegenMonster = value; }
    }

    //Use Debuging
    public bool isOnlyOneMonster = false;
    public static bool OnlyOneMonster;
    public bool isOnlyBossMonster = false;
    public static bool OnlyBossMonster;
    public int mSummonMonsterIdx = 0;
    public static int SummonMonsterIdx;

    //Battle Event Caller
    [SerializeField]
    private BattleEventCaller m_BattleEventCaller;
    public BattleEventCaller BattleEventCaller
    {
        get { return m_BattleEventCaller; }
    }

    //Boss hpBar Manager
    [SerializeField]
    private BossHpBarCtrlManager m_BossHpManager;
    public BossHpBarCtrlManager BossHpManager
    { get { return m_BossHpManager; } }

    //Using MonsterManager 
    private bool m_Processing = false;

    //CreatedMonster Transform
    [SerializeField]
    private Transform m_CreatedMonsterParent;
    public Transform CreatedMonsterParent
    { get { return m_CreatedMonsterParent; } }

    void OnEnable()
    {
        Debug.Log("MonsterManager Onenable");
        CreateInstance();
        
        OnlyOneMonster = isOnlyOneMonster;
        OnlyBossMonster = isOnlyBossMonster;
        SummonMonsterIdx = mSummonMonsterIdx;

        MecroMethod.CheckExistObject<Transform>(InFieldPosition);
        MecroMethod.CheckExistObject<Transform>(OutFieldPosition);
        MecroMethod.CheckExistObject<BattleEventCaller>(m_BattleEventCaller);
        MecroMethod.CheckExistObject<BossHpBarCtrlManager>(m_BossHpManager);

        if (m_CreatedMonsterParent == null)
            m_CreatedMonsterParent = transform.FindChild("CreatedMonsterParent");
        MecroMethod.CheckExistComponent<Transform>(m_CreatedMonsterParent);
        
        iRegenMonster = RegenMonster();
        StartCoroutine(iRegenMonster);
    }

    void StartCreateMonsters()
    {
        bool isCreateElite;
        int nCreateMonsterCount;
        SUMMONPOSITIONID CreatePositionID;

        ChooseCreateMonster(out isCreateElite,
            out nCreateMonsterCount, out CreatePositionID);

        //Cannot Create Over Two Boss Monster
        if(isCreateElite && m_isCheckBossExist)
        {
            Debug.Log("Cannot Create over One Boss");
            return;
        }

        //보스가 생성될 것이 정해졌으므로 여기서 보스가 생성되었다고 알린다.
        if (isCreateElite)
            CheckBossExist = true;

        if (CreatePositionID == SUMMONPOSITIONID.POSITION_INANDOUT)
            CreatePositionID = (SUMMONPOSITIONID)UnityEngine.Random.Range(
                0, (int)SUMMONPOSITIONID.POSITION_INANDOUT);

        switch (CreatePositionID)
        {
            case SUMMONPOSITIONID.POSITION_INFIELD:
                CreateSummonEffect(isCreateElite, nCreateMonsterCount, CreatePositionID);
                break;

            case SUMMONPOSITIONID.POSITION_OUTFIELD:
                //Invoke("CreateMonster", 0.9f);
                CreateMonster(isCreateElite, nCreateMonsterCount, CreatePositionID);
                break;

            case SUMMONPOSITIONID.POSITION_UPPERINFIELD:
                break;
        }
    }

    private void ChooseCreateMonster(out bool isElite,
        out int nChooseCount, out SUMMONPOSITIONID PositionID)
    {
        isElite = false;
        nChooseCount = 0;
        PositionID = SUMMONPOSITIONID.POSITION_INFIELD;

        if (OnlyBossMonster)
        {
            isElite = true;
            ChoiceMonster(isElite, out nChooseCount, out PositionID);
        }
        else
        {
            int nEliteDice = UnityEngine.Random.Range(0, 100);
            if (OnlyOneMonster)
                nEliteDice = 0;

            if(nEliteDice > 95) //Elite
            {
                isElite = true;
                ChoiceMonster(isElite, out nChooseCount, out PositionID);
            }
            else //Normal
            {
                isElite = false;
                ChoiceMonster(isElite, out nChooseCount, out PositionID);
            }
        }
    }

    private void ChoiceMonster(bool isElite,
        out int nChooseCount, out SUMMONPOSITIONID PositionID)
    {
        if(isElite)
        {
            nChooseCount = UnityEngine.Random.Range(0, FieldEliteMonsterData.Count);
            
            if (OnlyOneMonster)
                nChooseCount = mSummonMonsterIdx;
            PositionID = FieldEliteMonsterData.Keys.ToList()[nChooseCount].MonsterCreatePosition;
        }
        else
        {
            nChooseCount = UnityEngine.Random.Range(0, FieldMonsterData.Count);
            if (OnlyOneMonster)
                nChooseCount = mSummonMonsterIdx;
            PositionID = FieldMonsterData.Keys.ToList()[nChooseCount].MonsterCreatePosition;
        }
    }

    /// <summary>
    /// Effect Call CreateMonster
    /// </summary>
    private void CreateSummonEffect(bool _isCreateElite,
        int _nCreateMonsterCount,
        SUMMONPOSITIONID _CreatePositionID)
    {
        SummonEffectManager.GetInstance().SummonEffectCall("Thunder",
            _isCreateElite, _nCreateMonsterCount, _CreatePositionID);
        //GameObject SummonEffect =
        //    Resources.Load(
        //        "BattleScene/Monster_Summon_Effect/Monster_Summon_Effect(BlackHole)") as GameObject;
        //Instantiate(SummonEffect, InFieldPosition.position, Quaternion.identity);
    }

    public void CreateMonster(bool isElite,
        int CreateMonsteridx,
        SUMMONPOSITIONID CreatePositionID)
    {
        string FindExistKey = string.Empty;
        GameObject CreatedMonster = MonsterFactory(
            CreateMonsteridx, out FindExistKey, isElite, CreatePositionID);

        if(CreatedMonster == null)
        {
            Debug.Log("Cannot Create Monster");
            return;
        }

        if (m_Processing == false)
        {
            Destroy(CreatedMonster);
            return;
        }

        MonsterList.Add(CreatedMonster);
    }

    //Loaded XML Monster Looping Method
    private GameObject MonsterFactory(int nMonsterCnt,
        out string findExistKey, bool isElite, SUMMONPOSITIONID CreatePositionID)
    {
        //Bring To MonsterElements
        Dictionary<MonsterKey_Extension, LoadedMonsterElement> MonsterData;

        if (!isElite)
            MonsterData = FieldMonsterData;
        else
            MonsterData = FieldEliteMonsterData;

        LoadedMonsterElement LoadedElement = MonsterData.Values.ToList()[nMonsterCnt];
        findExistKey = MonsterData.Keys.ToList()[nMonsterCnt].MonsterPrefabName;
        GameObject Monster = Instantiate(LoadedElement.OriginGameObject);

        //MonsterBody Setting
        Transform MonsterBodyTrans = Monster.transform.FindChild("MonsterBody");
        MonsterBodyTrans.gameObject.AddComponent(LoadedElement.OriginInterfaceType);
        Monster_Interface CreatedInterface = MonsterBodyTrans.gameObject.GetComponent<Monster_Interface>();

        if (CreatedInterface == null)
        {
            Debug.Log(CreatedInterface.name + " is Null");
            return null;
        }

        CreatedInterface.CopyInterFace(LoadedElement.OriginInterfaceComp);
        CreatedInterface.CreatePosition = CreatePositionID;
        
        if (CreatedInterface.CreatePosition == SUMMONPOSITIONID.POSITION_OUTFIELD)
            CreatedInterface.m_isOutSummonMonster = true;

        //Monster Transform Setting
        Monster.SetActive(false);
        Monster.transform.SetParent(m_CreatedMonsterParent);
        //Monster.transform.SetParent(MonsterManager.GetInstance().transform);

        //Position
        Vector3 CreatePos = SetCreatedMonsterPosition(CreatePositionID);
        CreatePos -= new Vector3(0f, UnityEngine.Random.Range(0f, 0.3f), 0f);
        Monster.transform.position = CreatePos;

        //Scale
        Monster.transform.localScale = Vector3.one;
        Monster.SetActive(true);

        return Monster;
    }

    private Vector3 SetCreatedMonsterPosition(
        SUMMONPOSITIONID _CreatePositionID)
    {
        Vector3 vResult = Vector3.zero;
        switch (_CreatePositionID)
        {
            case SUMMONPOSITIONID.POSITION_INFIELD:
                vResult = MonsterManager.GetInstance().InFieldPosition.position;
                m_isMonsterExist = true;
                break;

            case SUMMONPOSITIONID.POSITION_OUTFIELD:
                vResult = MonsterManager.GetInstance().OutFieldPosition.position;
                break;
        }
        return vResult;
    }

    IEnumerator RegenMonster()
    {
        m_Processing = true;
        yield return new WaitForSeconds(StartRegenTime);

        //조건 만족하면 리젠 종료 후 탈출 구문을 쓴다.
        while (m_Processing == true)
        {
            //몬스터가 너무 많아지면 일시 대기상태
            if(MonsterList.Count >= MonsterMaxCount)
                yield return new WaitForFixedUpdate();
            else
            {
                //생성 딜레이 시간 정하기
                float fDelayTime = 
                    UnityEngine.Random.Range(RegenMinWidth, RegenMaxWidth);

                //몬스터 생성 시작
                StartCreateMonsters();

                //생성 후 대기
                yield return new WaitForSeconds(fDelayTime);
            }
        }
        yield break;
    }

    //public void RemovesMonster(string KeyName, GameObject DeleteMonster)
    public void RemoveMonster(GameObject DeleteMonster)
    {
        if(MonsterList.Count <= 0)
        {
            Debug.Log("Cannot Delete Monster Node");
            return;
        }

        //List<GameObject> Monsters = MonsterList[KeyName];
        //Monsters.Remove(DeleteMonster);
        if(MonsterList.Remove(DeleteMonster) == false)
        {
            Debug.LogError("Cannot Find Delete Monster Node");
            return;
        }

        Monster_Interface DelMosnterInterface =
            MecroMethod.CheckGetComponent<Monster_Interface>(
                DeleteMonster.transform.FindChild("MonsterBody"));

        DelMosnterInterface.DisableMonsterComps();
        UpdateScreenMonster();
        //--MonsterManager.MonsterCount;
        //Debug.Log(MonsterManager.MonsterCount);
    }

    //if you Change Scene loaded all monster data remove it
    public void ClearMonsterManager()
    {
        StopAllCoroutines();

        //MonsterManager 돌고있는 프로레스 관련
        m_Processing = false;

        //BattleEventCaller 관련
        MonsterManager.GetInstance().BattleEventCaller.ResetEventCaller();

        //Boss 관련
        m_isCheckBossExist = false;
        m_BossHpManager.RemoveAllData();

        //몬스터 객체 관련
        m_isMonsterExist = false;
        List<WeakReference> WeakRefList = new List<WeakReference>();

        RemoveLoadedMonsterContainer(FieldMonsterData, ref WeakRefList);
        RemoveLoadedMonsterContainer(FieldEliteMonsterData, ref WeakRefList);
        RemoveLoadedMonsterContainer(FieldSpecialMonsterData, ref WeakRefList);

        Debug.Log(WeakRefList.Count);

        FieldMonsterData.Clear();
        FieldEliteMonsterData.Clear();
        FieldSpecialMonsterData.Clear();

        for (int i = 0; i < WeakRefList.Count; ++i)
            WeakRefList.Remove(WeakRefList[i]);
        WeakRefList.Clear();

        Debug.Log("Clear Comp");

        //Instantiated Monster Clear
        for(int i = 0; i < MonsterList.Count; ++i)
            MonsterList.RemoveAt(i);
        MonsterList.Clear();

        m_CreatedMonsterParent.DestroyChildren();

        //GameObject MonsterParent = null;
        //for(int i = 0; i < MonsterList.Count; ++i)
        //{
        //    MonsterParent = MonsterList[i];
        //    MonsterList.RemoveAt(MonsterParent);
        //    Destroy(MonsterParent);
        //}
        //MonsterList.Clear();
    }

    private void RemoveLoadedMonsterContainer(
        Dictionary<MonsterKey_Extension, LoadedMonsterElement> _DeleteContainer,
        ref List<WeakReference> _WeakRefList)
    {
        int nLength = _DeleteContainer.Count;
        for (int i = 0; i < nLength; ++i)
        {
            _WeakRefList.Add(new WeakReference(_DeleteContainer.ToList()[i].Key));
            _WeakRefList.Add(new WeakReference(_DeleteContainer.ToList()[i].Value));
        }
    }

    public static void AttackFirstSummonedMonster()
    {
        List<GameObject> _MonsterList =
            MonsterManager.GetInstance().MonsterList;

        //몬스터 갯수가 한마리도 존재하지 않을 경우는 연산 스킵.
        if (_MonsterList.Count <= 0)
            return;

        Monster_Interface SelectedMonsterInfo =
            Mecro.MecroMethod.CheckGetComponent<Monster_Interface>(
            _MonsterList[0].transform.FindChild("MonsterBody"));

        if (SelectedMonsterInfo == null)
        {
            Debug.Log("cannot Find MonsterInfo");
            return;
        }

        int nAtkPower = PlayerCtrlManager.GetInstance().PlayerCtrl.Atk;
        //Debug.Log(nAtkPower);

        SelectedMonsterInfo.SetHp(nAtkPower);
    }

    //if you want create monster use to script use this method
    //If Apply Monster Setting SummonPosition = SUMMONPOSITIONID.POSITION_OUTFIELD
    //You Must Setting SummonPosition 
    public static GameObject MonsterFactory(MonsterSummonID SummonGrade,
        string MonsterPrefabName, Vector3 SummonPosition)
    {
        GameObject ResultMonsterInst = null;
        MonsterKey_Extension SelectedKey = null;
        LoadedMonsterElement SelectedElement = null;
        Dictionary<MonsterKey_Extension,
            LoadedMonsterElement> SelectedMonsterData = null;

        //Select Summon Monster Data Type
        switch(SummonGrade)
        {
            case MonsterSummonID.SUMMON_NORMAL:
                SelectedMonsterData = FieldMonsterData;
                break;

            case MonsterSummonID.SUMMON_ELITE:
                SelectedMonsterData = FieldEliteMonsterData;
                break;

            case MonsterSummonID.SUMMON_SPECIAL:
                SelectedMonsterData = FieldSpecialMonsterData;
                break;
        }

        //if You Cannot Find MonsterData Cancel this Method
        if (SelectedMonsterData == null)
        {
            Debug.Log("Not Exist SummonType Data");
            return null;
        }


        int MonsterDataCount = SelectedMonsterData.Count;

        for (int i = 0; i < MonsterDataCount; ++i)
        {
            if(SelectedMonsterData.Keys.ToList()[i].MonsterPrefabName ==
                 MonsterPrefabName)
            {
                SelectedKey = SelectedMonsterData.Keys.ToList()[i];
                SelectedElement = SelectedMonsterData[SelectedKey];
                break;
            }
        }

        if (SelectedElement.OriginGameObject == null)
        {
            Debug.Log("SelectedElement.OriginGameObject is null");
            return null;
        }
        ResultMonsterInst = Instantiate(SelectedElement.OriginGameObject);

        //MonsterBody Setting
        Transform MonsterBodyTrans = ResultMonsterInst.transform.FindChild("MonsterBody");
        MonsterBodyTrans.gameObject.AddComponent(SelectedElement.OriginInterfaceType);
        Monster_Interface CreatedInterface = MonsterBodyTrans.gameObject.GetComponent<Monster_Interface>();

        if (CreatedInterface == null)
            Debug.LogError(CreatedInterface.name + " is Null");

        CreatedInterface.CopyInterFace(SelectedElement.OriginInterfaceComp);
        CreatedInterface.CreatePosition = SelectedKey.MonsterCreatePosition;

        if (CreatedInterface.CreatePosition == SUMMONPOSITIONID.POSITION_OUTFIELD)
            CreatedInterface.m_isOutSummonMonster = true;

        //Monster Transform Setting
        ResultMonsterInst.SetActive(false);
        ResultMonsterInst.transform.SetParent(MonsterManager.GetInstance().CreatedMonsterParent);
        //ResultMonsterInst.transform.SetParent(MonsterManager.GetInstance().transform);

        //Position
        if (CreatedInterface.CreatePosition != SUMMONPOSITIONID.POSITION_OPTIONAL)
        {
            Vector3 CreatePos = MonsterManager.GetInstance(
                ).SetCreatedMonsterPosition(CreatedInterface.CreatePosition);
            CreatePos -= new Vector3(0f, UnityEngine.Random.Range(0f, 0.3f), 0f);
            ResultMonsterInst.transform.position = CreatePos;
        }
        else
            ResultMonsterInst.transform.position = SummonPosition;

        //Scale
        ResultMonsterInst.transform.localScale = Vector3.one;
        ResultMonsterInst.SetActive(true);

        if (MonsterManager.GetInstance().m_Processing == false)
        {
            Destroy(ResultMonsterInst);
            return null;
        }

        m_isMonsterExist = true;
        //Add To MonsterList
        MonsterManager.GetInstance().MonsterList.Add(ResultMonsterInst);

        return ResultMonsterInst;
    }

    public void UpdateScreenMonster()
    {
        if (MonsterList.Count <= 0)
            m_isMonsterExist = false;
    }
}

public class MonsterKey_Extension : DisposableObject
{
    public MonsterKey_Extension()
    {
        base.SetOwnProperty();
    }

    public override void Dispose()
    {
        m_MonsterPrefabName = string.Empty;
        m_MonsterCreatePosition = 0;
    }

    private string m_MonsterPrefabName;
    public string MonsterPrefabName
    {
        get { return m_MonsterPrefabName; }
        set { m_MonsterPrefabName = value; }
    }

    private SUMMONPOSITIONID m_MonsterCreatePosition;
    public SUMMONPOSITIONID MonsterCreatePosition
    {
        get { return m_MonsterCreatePosition; }
        set { m_MonsterCreatePosition = value; }
    }

   
}