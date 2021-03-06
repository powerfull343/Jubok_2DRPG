﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;
using System.Linq;


//행동 패턴등의 클래스를 담는다.
public class MagicianCtrl : Moveable_Object {

    //Variable
    protected static List<Monster_Interface> _ColMonsters
        = new List<Monster_Interface>();
    public static List<Monster_Interface> ColMonsters
    {
        get { return _ColMonsters; }
        set { _ColMonsters = value; }
    }

    private static bool isAtkMotionSelect = false;
    private string m_ChoiceSkillName = string.Empty;

    //Components
    private Animator _PlayerAnim;
    private MagicianNormalRangeAtkCtrl NoramlRangeAtk;
    private GameObject m_PlayerCastingEffect;

    //delegate
    public delegate void ChoiceAction();

    //Debuging
    public int DebugingAtkPower = 1;

    void Start () {
        
        if (DebugingAtkPower == 0)
            Atk = 2;
        else
            Atk = DebugingAtkPower;

        PlayerDataManager.GetInstance().UpdateStat(out _Hp, out _Mp, out _Stamina);
        MaxHp = Hp;
        MaxStamina = Stamina;
        MaxMp = Mp;

        AtkSpeed = 1f;

        ObjectType = Moveable_Type.TYPE_PLAYER;

        _PlayerAnim = MecroMethod.CheckGetComponent<Animator>(this.gameObject);
        mStateAnim = MecroMethod.CheckGetComponent<Animator>(this.transform.parent.FindChild("ObjectState(Color)"));
        NoramlRangeAtk = transform.parent.FindChild(
            "Position - NormalRangeAtkRespawn").GetComponent<MagicianNormalRangeAtkCtrl>();
        m_PlayerCastingEffect = transform.FindChild("CastingEffect").gameObject;
        if(!m_PlayerCastingEffect)
            Debug.LogError(m_PlayerCastingEffect.name);
    }

    public override void AutoAction()
    {
        //Debug.Log("MonsterCount : " + MonsterManager.MonsterCount);

        if (MonsterManager.MonsterCount <= 0)
            Move();
        else
            Attack();
    }

    public override bool SetHp(int discountAmount)
    {
        Hp -= discountAmount;

        if (Hp <= 0)
            return false;

        return true;
    }

    protected override void Move()
    {
        _PlayerAnim.SetTrigger("Move");
    }

    protected override void Attack()
    {
        AttackMotion();
    }

    protected override void Fatal()
    {
    }
    
    public void AttackMotion()
    {
        if (!isAtkMotionSelect)
        {
            if (ColMonsters.Count >= 1)
                MeleeAttack();
            else
                RangeAttack();

            isAtkMotionSelect = true;
        }
    }

    protected override void MeleeAttack()
    {
        if (ColMonsterCountCompareZero())
        {
            ResetAtkMotionSelect();
            ResetMeleeAtkTrigger();
            return;
        }

        int RdmIdx = UnityEngine.Random.Range(0, 100);

        if (RdmIdx < 90)
            _PlayerAnim.SetTrigger("NormalMelee");
        else
            _PlayerAnim.SetTrigger("SpecialMelee");

    }

    protected override void RangeAttack()
    {
        if (MonsterManager.GetInstance().MonsterList.Count <= 0)
            return;

        int RdmIdx = UnityEngine.Random.Range(0, 100);

        if (RdmIdx < 50)
            _PlayerAnim.SetTrigger("NormalRange");
        else
            SpecialRangeAttack();
    }

    private void SpecialRangeAttack()
    {
        //Skill Choice
        int nRdmIdx = SkillManager.GetInstance().LoadedSkill.Count;

        
        //EarthForce, Thunder02, PinPanel

        m_ChoiceSkillName = "PinPanel";

        if(!SkillManager.GetInstance().CheckingSkillUse(m_ChoiceSkillName))
            _PlayerAnim.SetTrigger("NormalRange");
        else
            _PlayerAnim.SetTrigger("SpecialRange");
    }

    public void ResetRangeAtkTrigger()
    {
        _PlayerAnim.ResetTrigger("NormalRange");
        _PlayerAnim.ResetTrigger("SpecialRange");
    }

    public void SummonSkillEffect()
    {
        //EarthForce, Thunder02, PinPanel
        //SkillManager.GetInstance().UseSkill(m_ChoiceSkillName);
    }

    public void ActiveNormalRangeAtk()
    {
        NoramlRangeAtk.CallFireBall(Atk);
    }

    public void MeleeDamage()
    {
        if (ColMonsterCountCompareZero())
        {
            ResetAtkMotionSelect();
            ResetMeleeAtkTrigger();
            return;
        }
        
        if(!ColMonsters[0].SetHp(Atk))
        {
            ResetAtkMotionSelect();
            return;
        }

        int nEffectSkillShuffle = UnityEngine.Random.Range(0, 100);
        if(nEffectSkillShuffle > 90) //스킬 데미지를 입힘과 동시에 몬스터 삭제를 관할한다.
            SkillManager.GetInstance().UseSkill("CriticalLighting", ColMonsters[0].transform);

        ResetAtkMotionSelect();
    }

    public void MeleeAllDamage()
    {
        if (ColMonsterCountCompareZero())
        {
            ResetAtkMotionSelect();
            ResetMeleeAtkTrigger();
            return;
        }

        for (int i = 0; i < ColMonsters.Count; ++i)
        {
            ColMonsters[i].SetHp(Atk * 3);
            //RemoveColMonster(i);
        }

        //ResetMeleeAtkTrigger();

        ResetAtkMotionSelect();
    }

    public void ResetMeleeAtkTrigger()
    {
        if (ColMonsterCountCompareZero())
        {
            _PlayerAnim.ResetTrigger("NormalMelee");
            _PlayerAnim.ResetTrigger("SpecialMelee");
        }
    }

    /// <summary>
    /// Zero = true, else = false
    /// </summary>
    /// 
    public static bool ColMonsterCountCompareZero()
    {
        if (ColMonsters.Count <= 0)
        {
            Debug.Log("No Collide Monsters");
            return true;
        }
        return false;
    }

    public static void RemoveColMonsters(int nIdx)
    {
        if (ColMonsters[nIdx].Hp <= 0)
        {
            if (ColMonsters[nIdx] != null)
            {
                MonsterManager.GetInstance().RemovesMonster(
                    ColMonsters[nIdx].ObjectName,
                    ColMonsters[nIdx].transform.parent.gameObject);
            }

            ColMonsters.RemoveAt(nIdx);
        }
    }

    public void ResetAtkMotionSelect()
    {
        //ResetMeleeAtkTrigger();
        //ResetRangeAtkTrigger();
        isAtkMotionSelect = false;
    }
}
