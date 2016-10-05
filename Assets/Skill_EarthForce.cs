using UnityEngine;
using System.Collections;
using Mecro;
using System.Collections.Generic;
using System.Linq;
using System;

public class Skill_EarthForce : Skill_Interface
{
    private Animator mChildGate;

    void Start()
    {
        mSkillName = "EarthForce";
        mSkillAnim = Mecro.MecroMethod.CheckGetComponent<Animator>(this.gameObject);
        //misChargeUse = true;
        //misMultiGameObject = true;
        mTargetType = Moveable_Type.TYPE_MONSTER;
        m_ExtensionInfo = 
            Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
        m_CostMana = 3;

        mChildGate =
            MecroMethod.CheckGetComponent<Animator>(
            transform.FindChild("EarthForceGate"));

        mSkillAnim.enabled = false;
    }

    protected override void SkillSetting()
    {
        if (MonsterManager.GetInstance().MonsterList.Count <= 0)
        {
            EndSkill();
            return;
        }

        //아무나 공격하게함.
        int nRandomSelectedMonster =
            UnityEngine.Random.Range(0, MonsterManager.GetInstance().MonsterList.Count);

        //List<GameObject> MonsterList =
        //    MonsterManager.GetInstance().MonsterList.Values.ElementAt(nRandomSelectedMonster);


        mTargetTrans = 
            MonsterManager.GetInstance().MonsterList[nRandomSelectedMonster].transform;

        transform.localScale = mTargetTrans.localScale;
        transform.position = mTargetTrans.position + new Vector3(0f, 0.5f, 0f);

        mTargetInfo = MecroMethod.CheckGetComponent<Moveable_Object>(
            mTargetTrans.FindChild("MonsterBody"));
    }

    protected override void SetSkillDamage()
    {
        mAtk =
            PlayerCtrlManager.GetInstance().PlayerCtrl.Atk * 10;
    }

    protected override void SetSkillManaCost()
    {
        base.SetSkillManaCost();
    }

    public override void EndSkill()
    {
        base.EndSkill();
    }

    public void SummonEarthForce()
    {
        if (!mTargetTrans || !mTargetInfo)
        {
            Destroy(this.gameObject);
            return;
        }

        mSkillAnim.enabled = true;
        mTargetInfo.SkillTargetFreezing(true);

        if (!mChildGate.gameObject.activeSelf)
            mChildGate.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        AttackToCollider(other);
    }
    
}
