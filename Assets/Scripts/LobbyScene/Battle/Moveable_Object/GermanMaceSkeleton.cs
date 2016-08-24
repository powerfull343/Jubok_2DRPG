using UnityEngine;
using System.Collections;
using System;

public class GermanMaceSkeleton : Monster_Interface
{
    private Transform MaceTrans;

    void Start()
    {
        base.Initializing();

        MaceTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            this.transform.parent.FindChild("Mace"));

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

        if(!MaceTrans.gameObject.activeSelf && isColPlayer)
            MaceTrans.gameObject.SetActive(true);
    }

    protected override void Fatal()
    {
        MaceTrans.gameObject.SetActive(false);
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

        if (other.gameObject.CompareTag("MeleeAtkCollider"))
        {
            ColChangeMove();
            isColPlayer = true;
        }
    }
}
