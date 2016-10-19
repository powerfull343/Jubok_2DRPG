using UnityEngine;
using System.Collections;
using System;

public class BlueSkeleton : Monster_Interface {

    private bool m_isThrowHead = false;

    private Transform HeadTrans;

    void Start() {

        Initializing();

        HeadTrans =
            Mecro.MecroMethod.CheckGetComponent<Transform>(transform.parent.FindChild("Head"));

        //Choice Head Monster
        int ThrowHeadCnt = UnityEngine.Random.Range(0, 4);
        if (ThrowHeadCnt >= 2)
        {
            m_isThrowHead = true;
            ThrowHead();
        }
        else
            StartCoroutine("ActionCoroutine");
    }


    public override void AutoAction()
    {
        base.AutoAction();
    }

    protected override void Move()
    {
        if(!m_isThrowHead)
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
        while(true)
        {
            if(Hp <= 0)
            {
                Fatal();
                break;
            }
            AutoAction();

            yield return new WaitForFixedUpdate();
        }

        yield break;

    }

    void ThrowHead()
    {
        mAnim.SetTrigger("ThrowHead");
    }

    public void ThrowHeadEnd()
    {
        if(!HeadTrans.gameObject.active)
            HeadTrans.gameObject.SetActive(true);

        StartCoroutine("ActionCoroutine");

        m_isThrowHead = false;
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
