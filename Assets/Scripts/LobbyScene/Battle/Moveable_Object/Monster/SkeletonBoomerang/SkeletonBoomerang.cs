using UnityEngine;
using System.Collections;
using System;

public class SkeletonBoomerang : Monster_Interface
{
    private Transform m_DeathParticles;
    private Transform m_BoomerangThrowTrans;

    private static GameObject m_BoomerangPrefab;
    private GameObject m_BoomerangObject;

    private bool m_isThrow = false;
    public bool IsThrow
    {
        get { return m_isThrow; }
        set { m_isThrow = value; }
    }

    private float m_AtkDelayTime = 1f;
    public float AtkDelayTime
    {
        get { return m_AtkDelayTime; }
        set { m_AtkDelayTime = value; }
    }

    void Start()
    {
        Initializing();

        StartCoroutine("ActionCoroutine");
    }

    protected override void Initializing()
    {
        base.Initializing();
        m_DeathParticles =
            Mecro.MecroMethod.CheckGetComponent<Transform>(
                this.transform.parent.FindChild("DeathParticles"));

        m_BoomerangThrowTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            this.transform.parent.FindChild("BoomerangThrow"));

        if (m_BoomerangPrefab == null)
        {
            m_BoomerangPrefab = Resources.Load(
                "BattleScene/Skills/SkeletonBoomerang - Boomerang") as GameObject;
            m_BoomerangPrefab.transform.localScale = new Vector3(3f, 3f, 1f);
        }

        ColChangeMove();
    }

    public override void AutoAction()
    {
        if(!m_isThrow)
            Attack();
        else
            idle();
    }

    protected override void Move()
    {
        base.Move();
    }

    private void idle()
    {
        mAnim.SetTrigger("Idle");
    }

    protected override void Attack()
    {
        mAnim.SetTrigger("Atk");
        m_isThrow = true;
    }

    protected override void Fatal()
    {
        //1. Bone Particle 발생시킴.
        this.gameObject.SetActive(false);
        if (!m_DeathParticles.gameObject.activeSelf)
            m_DeathParticles.gameObject.SetActive(true);

        //2. 실질적인 골드, 아이템 증가
        GetGold();
        GetItem();

        //3. 드랍할 아이템, 골드 활성화
        DropItemObjects();

        //4. 해당 몬스터의 모든 기능을 끝내고 객체 제거
        Invoke("KillMonster", 3f);
    }

    public override void KillMonster()
    {
        if (grade >= MONSTERGRADEID.GRADE_BOSS)
            MonsterManager.GetInstance().CheckBossExist = false;
        Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(mParentTrans).SelfDestroy();
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

    //void OnTriggerEnter(Collider other)
    //{
    //    if (m_isOutSummonMonster)
    //        OutFieldMonsterAddMonsterCount(other);

    //    if (other.gameObject.CompareTag("HighRangeAtkCollider"))
    //        isColPlayer = true;
    //}

    public void ThrowBoomerang()
    {
        if (!m_BoomerangObject)
        {
            m_BoomerangObject = Instantiate(m_BoomerangPrefab,
                m_BoomerangThrowTrans.position, Quaternion.identity) as GameObject;

            SkeletonBoomerang_Boomerang BoomComp =
                Mecro.MecroMethod.CheckGetComponent<SkeletonBoomerang_Boomerang>(m_BoomerangObject);
            BoomComp.SetThrowMonsterinfo(this);
            
        }
        else
        {
            m_BoomerangObject.SetActive(true);
        }

    }
}
