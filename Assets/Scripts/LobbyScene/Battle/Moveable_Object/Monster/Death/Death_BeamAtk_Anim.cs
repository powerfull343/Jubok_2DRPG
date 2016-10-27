using UnityEngine;
using System.Collections;

public class Death_BeamAtk_Anim : StateMachineBehaviour {

    private Death MonsterBody;
    private bool m_isSetMonsterBody = false;
    private Death.LAZERTYPE LazerType;
    private bool m_isCheckAtk = false;
    private bool m_isCycleAtk = false;
    private int m_nAtkIdx = 0;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_isSetMonsterBody)
        {
            MonsterBody = Mecro.MecroMethod.CheckGetComponent<Death>(animator.gameObject);
            m_isSetMonsterBody = true;
        }

        if (MonsterBody.m_ScytheSummoned)
            m_nAtkIdx = 0;
        else
            m_nAtkIdx = Random.Range(0, 2);

        //Debug.Log(m_nAtkIdx);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if(fNormalizeTime > 0.9f)
        {
            animator.SetTrigger("Idle");
            m_isCheckAtk = false;
        }

        Attack(animator, fNormalizeTime);
    }

    void Attack(Animator animator, float fNormalizeTime)
    {
        if (m_nAtkIdx == 0)
        {
            if (!FireBeamAtk(animator, fNormalizeTime, 0.1f, 0.2f) &&
            !FireBeamAtk(animator, fNormalizeTime, 0.3f, 0.4f) &&
            !FireBeamAtk(animator, fNormalizeTime, 0.5f, 0.6f) &&
            !FireBeamAtk(animator, fNormalizeTime, 0.7f, 0.8f))
                m_isCheckAtk = false;

            //Maybe.. Create 16 Objects
        }
        else
        {
            if (fNormalizeTime > 0.1f && fNormalizeTime < 0.2f)
                m_isCheckAtk = false;
            else if (fNormalizeTime > 0.3f && fNormalizeTime < 0.4f)
                m_isCheckAtk = false;
            else if (fNormalizeTime > 0.5f && fNormalizeTime < 0.6f)
                m_isCheckAtk = false;
            else if (fNormalizeTime > 0.7f && fNormalizeTime < 0.8f)
                m_isCheckAtk = false;
            else
            {
                m_isCheckAtk = true;
                m_isCycleAtk = false;
            }

            if (!m_isCheckAtk && !m_isCycleAtk)
                ScytheAttack();
        }
              
    }

    bool FireBeamAtk(Animator animator, float fNormalizeTime, float fMinTime, float fMaxTime)
    {
        if (!m_isCheckAtk)
        {
            if (fNormalizeTime > fMinTime && fNormalizeTime < fMaxTime)
            {
                //Debug.LogError("fNormalizeTime : " + fNormalizeTime);

                MonsterBody.BeamAttack((Death.LAZERTYPE)m_nAtkIdx);
                m_isCheckAtk = true;
                return true;
            }
        }
        return false;
    }

    void ScytheAttack()
    {
        MonsterBody.BeamAttack((Death.LAZERTYPE)m_nAtkIdx++);

        if (m_nAtkIdx >= 5)
            m_nAtkIdx = 1;

        m_isCheckAtk = true;
        m_isCycleAtk = true;

    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
