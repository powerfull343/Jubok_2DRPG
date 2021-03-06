﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Mecro;


public abstract class Monster_Interface : Moveable_Object {

    protected ATKTYPEID m_atktype;
    public ATKTYPEID atktype
    {
        get { return m_atktype; }
        set { m_atktype = value; }
    }

    protected MONSTERGRADEID m_grade;
    public MONSTERGRADEID grade
    {
        get { return m_grade; }
        set { m_grade = value; }
    }
    protected SUMMONPOSITIONID m_CreatePosition;
    public SUMMONPOSITIONID CreatePosition
    {
        get { return m_CreatePosition; }
        set { m_CreatePosition = value; }
    }

    protected Transform mParentTrans;
    protected BoxCollider mboxcol;
    protected Animator mAnim;
    protected bool isColPlayer = false;
    protected bool m_isOutSummonMonster = false;
    public bool isOutSummonMonster
    {
        get { return m_isOutSummonMonster; }
        set { m_isOutSummonMonster = value; }
    }
    
    public float mMonsterWalkingSpeed = 4f;
    
    protected Transform mHpStateTransform;

    protected delegate void VariableSetting(Monster_Interface Target, Monster_Interface Origin);
    protected static VariableSetting SetValues;
    protected static bool isSetValueSetting = false;

    public Monster_Interface()
    {
        if (!isSetValueSetting)
        {
            VariableSetting deleHp =
                (x, y) => x.Hp = y.Hp;
            VariableSetting deleAtk =
                (x, y) => x.Atk = y.Atk;
            VariableSetting deleAtkSpeed =
                (x, y) => x.AtkSpeed = y.AtkSpeed;
            VariableSetting deleObjectType =
                (x, y) => x.ObjectType = y.ObjectType;
            VariableSetting deleMonsterName =
                (x, y) => x.ObjectName = y.ObjectName;
            VariableSetting deleLoadPrefabName =
                (x, y) => x.LoadPrefabName = y.LoadPrefabName;
            VariableSetting deleAtktype =
                (x, y) => x.atktype = y.atktype;
            VariableSetting deleGrade =
                (x, y) => x.grade = y.grade;
            VariableSetting deleMaxHp =
                (x, y) => x.MaxHp = x.Hp;
            VariableSetting deleCreatePosition =
                (x, y) => x.CreatePosition = y.CreatePosition;

            SetValues += deleHp + deleAtk +
                deleAtkSpeed + deleObjectType +
                deleMonsterName +
                deleLoadPrefabName +
                deleAtktype + deleGrade +
                deleMaxHp + deleCreatePosition;

            isSetValueSetting = true;
        }
    }

    protected void Initializing()
    {
        mParentTrans =
            Mecro.MecroMethod.CheckGetComponent<Transform>(this.transform.parent);
        mboxcol =
            Mecro.MecroMethod.CheckGetComponent<BoxCollider>(this.transform);
        mAnim =
            Mecro.MecroMethod.CheckGetComponent<Animator>(this.transform);
        mStateAnimTransform =
            Mecro.MecroMethod.CheckGetComponent<Transform>(this.transform.parent.FindChild("ObjectState(Color)"));
        mStateAnim =
            Mecro.MecroMethod.CheckGetComponent<Animator>(mStateAnimTransform);
        mHpStateTransform =
            Mecro.MecroMethod.CheckGetComponent<Transform>(this.transform.parent.FindChild("HpUI"));
    }

    public override bool SetHp(int discountAmount)
    {
        Hp -= discountAmount;

        if (Hp <= 0)
        {
            Debug.Log("Hp Zero");
            MagicianCtrl.ColMonsters.Remove(this);
            MonsterManager.GetInstance().RemovesMonster(this.ObjectName,
                this.transform.parent.gameObject);
            return false;
        }

        return true;
    }

    protected string m_LoadPrefabName;
    public string LoadPrefabName
    {
        get { return m_LoadPrefabName; }
        set { m_LoadPrefabName = value; }
    }

    public override void AutoAction()
    {
        Move();
        Attack();
    }
    protected override void Move()
    {
        if ((int)m_grade > (int)MONSTERGRADEID.GRADE_NORMAL)
            isSkilledCondition = false;

        if (!isColPlayer && !isSkilledCondition)
        {
            mAnim.SetTrigger("Move");
            MovingPosition();
        }
    }

    protected virtual void MovingPosition()
    {
        Vector3 MovePos = new Vector3(mParentTrans.localPosition.x - (0.01f * mMonsterWalkingSpeed),
                mParentTrans.localPosition.y,
                mParentTrans.localPosition.z);

        mParentTrans.localPosition = Vector3.Lerp(this.transform.localPosition, MovePos, 1f);

    }

    protected override void Attack()
    {
        if(isColPlayer)
        {
            mAnim.SetTrigger("Atk");
        }
    }

    protected void ColChangeMove()
    {
        float fAmount = UnityEngine.Random.Range(-0.05f, 0.05f);
        mParentTrans.position += new Vector3(fAmount, 0f, 0f);
    }

    protected override void Fatal()
    {
        mAnim.SetTrigger("Die");
        
        Invoke("KillMonster", 0.95f);   
    }

    public void DisableMonsterComps()
    {
        mboxcol.enabled = false;
        mStateAnimTransform.gameObject.SetActive(false);
        mHpStateTransform.gameObject.SetActive(false);
        //Debug.LogError("disableComp");
    }

    protected void KillMonster()
    {
        Debug.Log("KillMonster");
        DropItemObjects();
        if (grade >= MONSTERGRADEID.GRADE_BOSS)
            MonsterManager.GetInstance().CheckBossExist = false;
        MecroMethod.CheckGetComponent<GameObject_Extension>(mParentTrans).SelfDestroy();
    }

    private int CalcMoneySize()
    {
        int MoneyResult = 0;

        return MoneyResult;
    }

    private void DropItemObjects()
    {
        ItemDropManager.GetInstance().DropItem(m_ObjectName, transform.position);
        ItemDropManager.GetInstance().DropCoin(transform.position, 10);
        //ItemDropManager.GetInstance().DropCoin(transform.position, CalcMoneySize());
    }

    protected abstract IEnumerator ActionCoroutine();

    protected void OutFieldMonsterAddMonsterCount(Collider collider)
    {
        if (collider.gameObject.CompareTag("OverTheArea"))
        {
            isOutSummonMonster = false;
            ++MonsterManager.MonsterCount;

            if (grade >= MONSTERGRADEID.GRADE_BOSS)
                Mecro.MecroMethod.CheckGetComponent<MonsterHpUICtrl>(mHpStateTransform).ShowBossHpUI();
        }
    }

    public static LoadedMonsterElement ConvertXMLToMonster(
        XmlNode node, out MonsterKey_Extension Key, out MONSTERGRADEID gradeid)
    {
        LoadedMonsterElement Monster = new LoadedMonsterElement();

        Key = new MonsterKey_Extension();
        Key.MonsterPrefabName = node.SelectSingleNode("Monster_Name").InnerText;

        string PrefabName = node.SelectSingleNode("Monster_PrefabName").InnerText;

        //Load Prefab Object
        string LoadFullPath = "BattleScene/Monsters/" + PrefabName;
        GameObject MonsterObject = Resources.Load(LoadFullPath) as GameObject;
        MecroMethod.CheckExistObejct<GameObject>(MonsterObject);
        Monster.OriginGameObject = MonsterObject;
        //Set Variables
        Monster_Interface MonsterInterface = Monster_NameList.CreateMonster(PrefabName);

        MonsterInterface.ObjectType = Moveable_Type.TYPE_MONSTER;
        MonsterInterface.m_ObjectName = Key.MonsterPrefabName;
        MonsterInterface.Hp = Int32.Parse(node.SelectSingleNode("Monster_Hp").InnerText);
        MonsterInterface.Atk = Int32.Parse(node.SelectSingleNode("Monster_Atk").InnerText);
        MonsterInterface.LoadPrefabName = node.SelectSingleNode("Monster_PrefabName").InnerText;
        MonsterInterface.atktype = (ATKTYPEID)Enum.Parse(typeof(ATKTYPEID),
            node.SelectSingleNode("Monster_AtkType").InnerText);
        if (!Enum.IsDefined(typeof(ATKTYPEID), MonsterInterface.atktype))
            Debug.LogError("Parse Error - ATKTYPEID");

        MonsterInterface.grade = (MONSTERGRADEID)Enum.Parse(typeof(MONSTERGRADEID),
            node.SelectSingleNode("Monster_Grade").InnerText);
        if (!Enum.IsDefined(typeof(MONSTERGRADEID), MonsterInterface.grade))
            Debug.LogError("Parse Error - MONSTERGRADEID");

        gradeid = MonsterInterface.grade;

        MonsterInterface.CreatePosition = (SUMMONPOSITIONID)Enum.Parse(typeof(SUMMONPOSITIONID),
            node.SelectSingleNode("Monster_Create_Position").InnerText);
        if (!Enum.IsDefined(typeof(SUMMONPOSITIONID), MonsterInterface.CreatePosition))
            Debug.LogError("Parse Error - SUMMONPOSITIONID");

        Key.MonsterCreatePosition = MonsterInterface.CreatePosition;
        //MecroMethod.CheckExistObejct<LoadedMonsterElement>(Monster);
        Monster.OriginInterfaceComp = MonsterInterface;

        return Monster;   
    }

    public bool CopyInterFace(Monster_Interface Origin)
    {
        //Debug.Log("Atk :" + Origin.Atk);

        SetValues(this.GetComponent<Monster_Interface>() ,Origin);

        return true;
    }
}

public class Monster_NameList
{
    public static Monster_Interface CreateMonster(string PrefabName)
    {
        Type type = Type.GetType(PrefabName, true);
        Monster_Interface instance = (Activator.CreateInstance(type)) as Monster_Interface;

        return instance;
    }
}