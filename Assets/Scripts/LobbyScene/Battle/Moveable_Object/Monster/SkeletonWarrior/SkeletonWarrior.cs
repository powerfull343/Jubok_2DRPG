using UnityEngine;
using System.Collections;
using System;

public class SkeletonWarrior : Monster_Interface
{
    private Transform MagicCrestTrans;

    void Start()
    {
        base.Initializing();

        MagicCrestTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(
            transform.parent.FindChild("Sprite - MagicCrest"));

        StartCoroutine("ActionCoroutine");
    }

    public override void AutoAction()
    {
        base.AutoAction();
        MagicCrestTrans.Rotate(Vector3.forward);
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
        MagicCrestTrans.gameObject.SetActive(false);
        mAnim.SetTrigger("Die");
        Invoke("KillMonster", 1.5f);
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

        if (other.gameObject.CompareTag("DontGoAwayCollider"))
        {
            ColChangeMove();
            isColPlayer = true;
        }
    }
}
