using UnityEngine;
using System.Collections;
using System;

public class GermanSkeleton : Monster_Interface
{
    void Start()
    {
        base.Initializing();

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
            OutFieldMonsterAddMonsterCount(other);

        if (other.gameObject.CompareTag("DontGoAwayCollider"))
        {
            ColChangeMove();
            isColPlayer = true;
        }
    }
}
