﻿using UnityEngine;
using System.Collections;
using System;

public class Mimic : Monster_Interface
{
    void Start()
    {
        base.Initializing();
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

    public void BornEnd()
    {
        StartCoroutine("ActionCoroutine");
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
