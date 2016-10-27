using UnityEngine;
using System.Collections;
using Mecro;

public class SummonEffectCtrl : MonoBehaviour {

    private Animator m_EffectAnim;
    private IEnumerator m_CheckingEffectOver;

    [SerializeField]
    private float m_fSummonAnimationRate = 0.9f;

    [SerializeField]
    private Vector3 m_fEffectOffset;

    private SUMMONPOSITIONID m_CreatePositionID;
    public SUMMONPOSITIONID CreatePositionID
    {
        get { return m_CreatePositionID; }
        set { m_CreatePositionID = value; }
    }
    private bool m_isElite;
    public bool isElite
    {
        get { return m_isElite; }
        set { m_isElite = value; }
    }
    private int m_nSummonMonsterCount;
    public int SummonMonsterCount
    {
        get { return m_nSummonMonsterCount; }
        set { m_nSummonMonsterCount = value; }
    }

    // Use this for initialization
	void Start () {
        m_EffectAnim = MecroMethod.CheckGetComponent<Animator>(this.gameObject);
        m_CheckingEffectOver = CheckingEffectOver();
        StartCoroutine(m_CheckingEffectOver);
        
        transform.position += m_fEffectOffset;

    }

    public void SetCreateMonsterInfo(bool __isElite,
        int __SummonMonsterCount, SUMMONPOSITIONID __CreatePositionID)
    {
        m_isElite = __isElite;
        m_nSummonMonsterCount = __SummonMonsterCount;
        m_CreatePositionID = __CreatePositionID;
    }
	
    IEnumerator CheckingEffectOver()
    {
        while(true)
        {
            if (m_fSummonAnimationRate <= Mathf.Repeat(m_EffectAnim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f))
            {
                MonsterManager.GetInstance().CreateMonster(m_isElite,
                    m_nSummonMonsterCount, m_CreatePositionID);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
