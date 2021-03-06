﻿using UnityEngine;
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
    public Dictionary<string, List<GameObject>> MonsterList
        = new Dictionary<string, List<GameObject>>();

    //Variables
    public static int MonsterCount = 0;
    public static int MonsterMaxCount = 10;

    public float StartRegenTime = 5f;
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


    void OnEnable()
    {
        CreateInstance();
        OnlyOneMonster = isOnlyOneMonster;
        OnlyBossMonster = isOnlyBossMonster;
        SummonMonsterIdx = mSummonMonsterIdx;

        MecroMethod.CheckExistObejct<Transform>(InFieldPosition);
        MecroMethod.CheckExistObejct<Transform>(OutFieldPosition);
        MecroMethod.CheckExistObejct<BattleEventCaller>(m_BattleEventCaller);

        iRegenMonster = RegenMonster();
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(iRegenMonster);
    }
    
    void InitCreateMonsters()
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
            CreatePositionID = (SUMMONPOSITIONID)Random.Range(
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
            int nEliteDice = Random.Range(0, 100);

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
            nChooseCount = Random.Range(0, FieldEliteMonsterData.Count);
            if (OnlyOneMonster)
                nChooseCount = mSummonMonsterIdx;
            PositionID = FieldEliteMonsterData.Keys.ToList()[nChooseCount].MonsterCreatePosition;
        }
        else
        {
            nChooseCount = Random.Range(0, FieldMonsterData.Count);
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

        MonsterAddToMonsterList(CreatedMonster, FindExistKey);

        //밖에서 나온몹은 추가된걸 취급하지 않는다.
        if(CreatePositionID != SUMMONPOSITIONID.POSITION_OUTFIELD)
            ++MonsterCount;

        Debug.Log("MonsterCount : " + MonsterCount);
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
            Debug.LogError(CreatedInterface.name + " is Null");

        CreatedInterface.CopyInterFace(LoadedElement.OriginInterfaceComp);
        CreatedInterface.CreatePosition = CreatePositionID;
        
        if (CreatedInterface.CreatePosition == SUMMONPOSITIONID.POSITION_OUTFIELD)
            CreatedInterface.isOutSummonMonster = true;

        //Monster Transform Setting
        Monster.SetActive(false);
        Monster.transform.SetParent(MonsterManager.GetInstance().transform);

        //Position
        Vector3 CreatePos = SetCreatedMonsterPosition(CreatePositionID);
        CreatePos -= new Vector3(0f, Random.Range(0f, 0.3f), 0f);
        Monster.transform.position = CreatePos;

        //Scale
        Monster.transform.localScale = Vector3.one;
        Monster.SetActive(true);

        return Monster;
    }

    private Vector3 SetCreatedMonsterPosition(SUMMONPOSITIONID _CreatePositionID)
    {
        Vector3 vResult = Vector3.zero;
        switch (_CreatePositionID)
        {
            case SUMMONPOSITIONID.POSITION_INFIELD:
                vResult = MonsterManager.GetInstance().InFieldPosition.position;
                break;

            case SUMMONPOSITIONID.POSITION_OUTFIELD:
                vResult = MonsterManager.GetInstance().OutFieldPosition.position;
                break;
        }
        return vResult;
    }

    private void MonsterAddToMonsterList(GameObject Monster, string FindExistKey)
    {
        //Apply MonsterList
        if (!MonsterManager.GetInstance().MonsterList.ContainsKey(FindExistKey))
        {
            List<GameObject> MonsterObjectList = new List<GameObject>();
            MonsterObjectList.Add(Monster);
            MonsterManager.GetInstance().MonsterList.Add(FindExistKey, MonsterObjectList);
        }
        else
        {
            MonsterManager.GetInstance().MonsterList[FindExistKey].Add(Monster);
        }
    }

    IEnumerator RegenMonster()
    {
        yield return new WaitForSeconds(StartRegenTime);

        while (true)
        {
            //조건 만족하면 리젠 종료 후 탈출 구문을 쓴다.
            
            //몬스터가 너무 많아지면 일시 대기상태
            if (MonsterCount >= MonsterMaxCount)
                yield return new WaitForFixedUpdate();
            else
            {
                //생성 딜레이 시간 정하기
                float fDelayTime = Random.Range(RegenMinWidth, RegenMaxWidth);

                //몬스터 생성 시작
                InitCreateMonsters();

                //생성 후 대기
                yield return new WaitForSeconds(fDelayTime);
            }
        }
        yield break;
    }

    public void RemovesMonster(string KeyName, GameObject DeleteMonster)
    {
        if(MonsterList.Count <= 0)
        {
            Debug.Log("Cannot Delete Monster Node");
            return;
        }

        List<GameObject> Monsters = MonsterList[KeyName];
        Monsters.Remove(DeleteMonster);

        Monster_Interface DelMosnterInterface =
            MecroMethod.CheckGetComponent<Monster_Interface>(DeleteMonster.transform.FindChild("MonsterBody"));

        DelMosnterInterface.DisableMonsterComps();

        --MonsterManager.MonsterCount;
        Debug.Log(MonsterManager.MonsterCount);
    }

    public static void AttackFirstSummonedMonster()
    {
        Debug.Log("MonsterList Count : " + MonsterManager.GetInstance().MonsterList.Count);

        List<GameObject> SelectedMonsterList = MonsterManager.GetInstance(
            ).MonsterList.ToList()[0].Value;

        if (SelectedMonsterList == null)
        {
            Debug.Log("cannot Find Monster Object List");
            return;
        }

        Debug.Log("SelectedMonsterListCount : " + SelectedMonsterList.Count);

        Monster_Interface SelectedMonsterInfo =
            Mecro.MecroMethod.CheckGetComponent<Monster_Interface>(
            SelectedMonsterList[0].transform.FindChild("MonsterBody"));

        if (SelectedMonsterInfo == null)
        {
            Debug.Log("cannot Find MonsterInfo");
            return;
        }

        int nAtkPower = PlayerCtrlManager.GetInstance().PlayerCtrl.Atk;
        Debug.Log(nAtkPower);

        SelectedMonsterInfo.SetHp(nAtkPower);
    }

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
        Debug.Log(MonsterDataCount);

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

        Debug.Log(SelectedKey.ToString());
        Debug.Log(SelectedElement.ToString());

        if (SelectedElement.OriginGameObject == null)
            Debug.LogError("SelectedElement.OriginGameObject is null");
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
            CreatedInterface.isOutSummonMonster = true;

        //Monster Transform Setting
        ResultMonsterInst.SetActive(false);
        ResultMonsterInst.transform.SetParent(MonsterManager.GetInstance().transform);

        //Position
        if (CreatedInterface.CreatePosition != SUMMONPOSITIONID.POSITION_OPTIONAL)
        {
            Vector3 CreatePos = MonsterManager.GetInstance(
                ).SetCreatedMonsterPosition(CreatedInterface.CreatePosition);
            CreatePos -= new Vector3(0f, Random.Range(0f, 0.3f), 0f);
            ResultMonsterInst.transform.position = CreatePos;
        }
        else
            ResultMonsterInst.transform.position = SummonPosition;

        //Scale
        ResultMonsterInst.transform.localScale = Vector3.one;
        ResultMonsterInst.SetActive(true);

        MonsterManager.GetInstance().MonsterAddToMonsterList(
            ResultMonsterInst, CreatedInterface.ObjectName);

        //밖에서 나온몹은 추가된걸 취급하지 않는다.
        if (CreatedInterface.CreatePosition !=
            SUMMONPOSITIONID.POSITION_OUTFIELD)
            ++MonsterCount;

        return ResultMonsterInst;
    }
}

public class MonsterKey_Extension
{
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