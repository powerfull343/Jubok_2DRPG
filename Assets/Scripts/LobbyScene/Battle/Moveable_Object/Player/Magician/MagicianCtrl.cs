using UnityEngine;
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

    void Awake()
    {
        ObjectType = Moveable_Type.TYPE_PLAYER;

        _PlayerAnim = MecroMethod.CheckGetComponent<Animator>(this.gameObject);
        mStateAnim = MecroMethod.CheckGetComponent<Animator>(this.transform.parent.FindChild("ObjectState(Color)"));
        NoramlRangeAtk = transform.parent.FindChild(
            "Position - NormalRangeAtkRespawn").GetComponent<MagicianNormalRangeAtkCtrl>();
        m_PlayerCastingEffect = transform.FindChild("CastingEffect").gameObject;
        if (!m_PlayerCastingEffect)
            Debug.LogError(m_PlayerCastingEffect.name);
    }

    void OnEnable()
    {
        InitPlayerStat();
        InitColMonsterContainer();
        _PlayerAnim.enabled = true;
        Invoke("InitEventTextMsg", 0.25f);
        
    }

    private void InitPlayerStat()
    {
        PlayerDataManager.GetInstance().UpdateStat(out _Hp, out _Mp,
           out _Stamina, out _Atk);
        MaxHp = Hp;
        MaxStamina = Stamina;
        MaxMp = Mp;

        if (DebugingAtkPower == 0)
            Atk = _Atk;
        else
            Atk = DebugingAtkPower;

        AtkSpeed = 1f;
    }

    private void InitColMonsterContainer()
    {
        if (_ColMonsters == null)
            _ColMonsters = new List<Monster_Interface>();
    }

    protected override void InitEventTextMsg()
    {
        Debug.Log("Magician EventText Init");
        base.InitEventTextMsg();
        if (!mEventMsg)
        {
            GameObject EventTextMsg = Instantiate(m_LoadedEventText);
            EventTextMsg.transform.SetParent(Battle_NGUI_EventMsgManager.GetInstance().transform, false);
            mEventMsg = Mecro.MecroMethod.CheckGetComponent<Battle_NGUI_EventMsg>(EventTextMsg);
            mEventMsg.InitEventMsg(transform.parent, false);
        }

        StartCoroutine("TestEventText");
    }

    private void ResetPlayerAction()
    {
        ResetMeleeAtkTrigger();
        ResetRangeAtkTrigger();
        SpriteRenderer ChildCastingEffectComp =
            m_PlayerCastingEffect.GetComponent<SpriteRenderer>();

        if (ChildCastingEffectComp != null)
            ChildCastingEffectComp.sprite = null;

        _PlayerAnim.SetTrigger("Move");
        _PlayerAnim.enabled = false;
    }

    public override void AutoAction()
    {
        //Debug.Log("MonsterCount : " + MonsterManager.MonsterCount);

        if (!MonsterManager.m_isMonsterExist)
        {
            //Debug.Log("Moving");
            Move();
        }
        else
        {
            //Debug.Log("Attacking");
            Attack();
        }
    }

    public override bool SetHp(int discountAmount)
    {
        Hp -= discountAmount;
        if (Hp <= 0)
            Hp = 1;

        ShowEventTextMsg("- " + discountAmount.ToString(), Color.red);

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
        _PlayerAnim.ResetTrigger("Move");
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

        if (RdmIdx > 80)
        {
            //Debug.Log("NormalRange");
            SpecialRangeAttack();
            
        }
        else
        {
            //Debug.Log("SpecialRange");
            _PlayerAnim.SetTrigger("NormalRange");
        }
    }

    private void SpecialRangeAttack()
    {
        //Skill Choice
        int nRdmIdx = SkillManager.GetInstance().LoadedSkill.Count;
        nRdmIdx = Random.Range(0, nRdmIdx);

        //EarthForce, Thunder02, PinPanel
        switch (nRdmIdx)
        {
            case 0:
                m_ChoiceSkillName = "EarthForce";
                break;

            case 1:
                m_ChoiceSkillName = "Thunder02";
                break;

            case 2:
                m_ChoiceSkillName = "PinPanel";
                break;
        }

        //int nRdmIdx = SkillManager.GetInstance().LoadedSkill.Count;
        //m_ChoiceSkillName = "PinPanel";

        if (!SkillManager.GetInstance().CheckingSkillUse(m_ChoiceSkillName))
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
        //Debug.Log(m_ChoiceSkillName);
        SkillManager.GetInstance().UseSkill(m_ChoiceSkillName);
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
        if(nEffectSkillShuffle > 90 && ColMonsters.Count >= 1) //스킬 데미지를 입힘과 동시에 몬스터 삭제를 관할한다.
            SkillManager.GetInstance().UseSkill("CriticalLighting", ColMonsters[0].transform);

        ResetMeleeAtkTrigger();
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

        ResetMeleeAtkTrigger();
        ResetAtkMotionSelect();
    }

    public void ResetMeleeAtkTrigger()
    {
        _PlayerAnim.ResetTrigger("NormalMelee");
        _PlayerAnim.ResetTrigger("SpecialMelee");
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
                MonsterManager.GetInstance().RemoveMonster(
                    ColMonsters[nIdx].transform.parent.gameObject);
            }

            ColMonsters.RemoveAt(nIdx);
        }
    }

    public void ResetAtkMotionSelect()
    {
        isAtkMotionSelect = false;
    }

    public override void ClearAllData()
    {
        ResetPlayerAction();

        //Remove All ColMonster Data
        //int nColMonsterLen = ColMonsters.Count;
        //if(nColMonsterLen > 0)
        //{
        //    for(int i = 0; i < nColMonsterLen; ++i)
        //        ColMonsters.RemoveAt(i);
        //}
        ColMonsters.Clear();
        NoramlRangeAtk.ClearAllFireBall();
    }

    IEnumerator TestEventText()
    {
        while(true)
        {
            ShowEventTextMsg("Test", Color.white);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
