using UnityEngine;
using System.Collections;
using System;

public class SkeletonBomber : Monster_Interface
{
    private static GameObject mBomb;
    private Bullet_Extension mBombInfo;
    private static bool misBombSetting = false;
    private Transform mBombThrowTrans;

    void Start()
    {
        base.Initializing();

        if (!misBombSetting)
            BombSetting();

        mBombThrowTrans = transform.parent.FindChild("BombThrow");
        Mecro.MecroMethod.CheckGetComponent<Transform>(mBombThrowTrans);

        StartCoroutine("ActionCoroutine");
    }

    public override void AutoAction()
    {
        base.AutoAction();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Fatal()
    {
        base.Fatal();
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
            OutFieldMonster_CanAttack(other);

        if (other.gameObject.CompareTag("MiddleRangeAtkCollider"))
        {
            ColChangeMove();
            isColPlayer = true;
        }
    }

    void BombSetting()
    {
        mBomb = Resources.Load(
            "BattleScene/Skills/SkeletonBomber - Bomb") as GameObject;

        mBombInfo = Mecro.MecroMethod.CheckGetComponent<Bullet_Extension>(mBomb);

        misBombSetting = true;
    }

    public void ThrowingBomb()
    {
        GameObject BombObject = Instantiate(mBomb, mBombThrowTrans.position, Quaternion.identity) as GameObject;
        BombObject.GetComponent<Bullet_Extension>().m_AtkPower = 3f;
    }
}
