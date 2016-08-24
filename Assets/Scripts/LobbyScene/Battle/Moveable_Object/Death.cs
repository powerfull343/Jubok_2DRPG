using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Death : Monster_Interface
{
    public enum LAZERTYPE
    {
        LAZER_BEAM = 0,
        LAZER_SCYTHE01,
        LAZER_SCYTHE02,
        LAZER_SCYTHE03,
        LAZER_SCYTHE04,
        LAZER_BIGSCYTHE,
        LAZER_MAX,
    };

    private string[] m_ColliderTags = new string[3];
    /// <summary>
    /// 0 = High, 1 = Middle, 2 = DontGoAwayCollider
    /// </summary>
    public int m_nPosIndex = 0;
    public bool m_isTeleport = false;
    private Transform m_TelePortTrans;

    //Beam Variable
    private List<GameObject> BeamPrefabs =
        new List<GameObject>();
    private Transform m_BeamAppearTrans;
    private LAZERTYPE m_LazerType;
    public bool m_ScytheSummoned = false;

    //Slash Variable
    private List<Transform> MonsterClone
        = new List<Transform>();

    //Big Scythe Variable
    private Transform m_BigScytheTrans;
    private Transform m_BigScytheRoatateTrans;
    private GameObject m_BigScytheObject;

    //Hp Bar Variable
    private BossHpBar m_BossHpBar;


    void Start()
    {
        base.Initializing();

        mParentTrans.position += new Vector3(0f, 1f, 0f);

        InitColliderInfo();

        m_TelePortTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            transform.parent.parent.FindChild("CreatePosition(InField)"));
        m_BeamAppearTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            transform.parent.FindChild("Transform - BeamAtkTrans"));
        m_BigScytheRoatateTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            transform.parent.parent.FindChild("CreatePosition(BigSkillRotate)"));
        m_BigScytheTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            transform.parent.parent.FindChild("CreatePosition(BigSkill)"));

        InitBeamPrefabs();
        InitMonsterClones();

        if (m_CreatePosition == SUMMONPOSITIONID.POSITION_OUTFIELD)
            mAnim.SetTrigger("Move");

        StartCoroutine("ActionCoroutine");
    }

    void InitColliderInfo()
    {
        m_ColliderTags[0] = "HighRangeAtkCollider";
        m_ColliderTags[1] = "MiddleRangeAtkCollider";
        m_ColliderTags[2] = "DontGoAwayCollider";
    }

    void InitBeamPrefabs()
    {
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - DeathLazer") as GameObject);
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - Scythe01") as GameObject);
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - Scythe02") as GameObject);
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - Scythe03") as GameObject);
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - Scythe04") as GameObject);
        BeamPrefabs.Add(Resources.Load("BattleScene/Skills/Death - BigScythe") as GameObject);
    }

    void InitMonsterClones()
    {
        MonsterClone.Add(transform.parent.FindChild("MonsterBodyClone01"));
        MonsterClone.Add(transform.parent.FindChild("MonsterBodyClone02"));
    }

    public override bool SetHp(int discountAmount)
    {
        base.SetHp(discountAmount);
        if (!m_BossHpBar)
        {
            m_BossHpBar = mHpStateTransform.gameObject.GetComponent<MonsterHpUICtrl>().HpBar;
            if (!m_BossHpBar)
                return false;
        }

        return true;
    }

    public override void AutoAction()
    {
        if (mAnim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            Move();
    }

    protected override void Move()
    {
        //이동하는데 이동시에는 멀중가 순으로 이동한다.
        base.MovingPosition();
    }

    protected override void Attack()
    {
        //이동이 끝나면 공격 그리고 맨 근접콜라이더에 있을시엔 슬래쉬를 쓴다.
    }

    public void Teleport()
    {
        MagicianCtrl.ColMonsters.Remove(this);

        mParentTrans.position = m_TelePortTrans.position + new Vector3(0f, 1f, 0f);
    }

    public void BeamAttack(LAZERTYPE type)
    {
        Vector3 vDirection = m_BeamAppearTrans.position -
            PlayerCtrlManager.GetInstance().transform.position;

        float fCosAngle = Mecro.MecroMethod.GetAngle(Vector3.right, vDirection.normalized);

        //Create Object
        GameObject BeamObject = Instantiate(BeamPrefabs[(int)type],
           m_BeamAppearTrans.position, Quaternion.Euler(new Vector3(0f, 0f, fCosAngle))) as GameObject;


        if (type == LAZERTYPE.LAZER_BEAM)       //Beam Type
        {
            Mecro.MecroMethod.CheckGetComponent<
                Bullet_Extension>(BeamObject).m_AtkPower = (float)Atk / 3;
        }
        else //Summon 5 Scythes
        {
            BeamObject.SetActive(false);

            Death_Scythe_Rotation ScytheInfo =
                Mecro.MecroMethod.CheckGetComponent<Death_Scythe_Rotation>(BeamObject);
            ScytheInfo.AtkPower = (float)Atk / 2;
            BeamObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            if (type == LAZERTYPE.LAZER_SCYTHE04)
            {
                m_ScytheSummoned = true;
                ScytheInfo.m_isLastScythe = true;
                ScytheInfo.MonsterBody = this;
            }

            BeamObject.SetActive(true);

        }
    }

    public void bigScytheAttack()
    {
        if(m_BigScytheObject != null)
        {
            if (m_BigScytheObject.activeSelf)
                return;

            m_BigScytheObject.transform.position = m_BigScytheTrans.position;
            m_BigScytheObject.SetActive(true);
            return;
        }

        GameObject BeamObject = Instantiate(BeamPrefabs[(int)LAZERTYPE.LAZER_BIGSCYTHE],
            m_BigScytheTrans.position, Quaternion.identity) as GameObject;

        m_BigScytheObject = BeamObject;

        BeamObject.SetActive(false);

        BigScytheController Ctrler = new BigScytheController(m_BigScytheRoatateTrans, (float)Atk);
        Type Classtype = Ctrler.GetType();

        //Attach Component Complete
        BeamObject.AddComponent(Classtype);
        Mecro.MecroMethod.CheckGetComponent<BigScytheController>(BeamObject).CopyInstance(Ctrler);

        BeamObject.transform.localScale = new Vector3(-3f, 3f, 1f);
        BeamObject.SetActive(true);
    }

    public void CloneAcitve(int nIndx)
    {
        if (!MonsterClone[nIndx].gameObject.activeSelf)
            MonsterClone[nIndx].gameObject.SetActive(true);
    }

    protected override void Fatal()
    {
        if(m_BigScytheObject != null)
            Destroy(m_BigScytheObject);
        DisableMonsterComps();

        mAnim.SetTrigger("Die");
        Invoke("KillMonster", 2f);
    }

    protected override IEnumerator ActionCoroutine()
    {
        while (true)
        {
            if (Hp <= 0)
            {
                Fatal();
                break;
            }
            AutoAction();

            yield return new WaitForFixedUpdate();
        }

        yield break;

    }

    void OnTriggerEnter(Collider other)
    {
        if (m_isOutSummonMonster)
            OutFieldMonsterAddMonsterCount(other);

        //콜라이더 3개 다써야함. 맨 근접 공격 콜라이더 적용시 텔레포트
        if(other.gameObject.CompareTag(m_ColliderTags[m_nPosIndex]))
        {
            if (m_nPosIndex >= 2)
            {
                m_nPosIndex = 0;
                m_isTeleport = true;
            }
            ++m_nPosIndex;
            mAnim.SetTrigger("Idle");
        }
    }
}
