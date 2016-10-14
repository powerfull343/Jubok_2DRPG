using UnityEngine;
using System.Collections;

public class Death_Idle_Anim : StateMachineBehaviour {

    private int m_IdleActionCount;
    private int m_IdleActionMaxCount;
    private bool m_isSetActionCount = false;

    private Death MonsterBody;
    private bool m_isSetMonsterBody = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_isSetMonsterBody)
        {
            MonsterBody = Mecro.MecroMethod.CheckGetComponent<Death>(animator.gameObject);
            m_isSetMonsterBody = true;
        }
        SetActionCount();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Mathf.Repeat(stateInfo.normalizedTime, 1f) > 0.95f)
        {
            //Debug.Log("m_IdleActionMaxCount : " + m_IdleActionMaxCount);
            //Debug.Log("m_IdleActionCount : " + m_IdleActionCount);
            CheckNextAction(animator);
        }
    }

    void CheckNextAction(Animator animator)
    {
        if(m_IdleActionCount >= m_IdleActionMaxCount)
        {
            if (MonsterBody.m_isOutSummonMonster)
            {
                OutSummonedMonsterFirstMoving(animator);
                m_isSetActionCount = false;
                SetActionCount();
                return;
            }

            int nActionIdx = Random.Range(0, 10);
            if (nActionIdx < 4)
                Moving(animator);
            else
                Attacking(animator);

            m_isSetActionCount = false;
            SetActionCount();
        }

        ++m_IdleActionCount;
    }

    void SetActionCount()
    {
        if (!m_isSetActionCount)
        {
            m_IdleActionMaxCount = Random.Range(4, 8);
            m_IdleActionCount = 0;
            m_isSetActionCount = true;
        }
    }

    void OutSummonedMonsterFirstMoving(Animator animator)
    {
        Debug.Log("First Summon Out of Screen Monster");
        Moving(animator);
        MonsterBody.m_isOutSummonMonster = false;
    }

    void Moving(Animator animator)
    {
        if (MonsterBody.m_isTeleport == true)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Teleport");
            MonsterBody.m_isTeleport = false;
        }
        else
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Move");
        }
    }

    void Attacking(Animator animator)
    {
        if (MonsterBody.m_isTeleport == true)
        //근접해있을때
        {
            animator.SetTrigger("SlashAttack");
        }
        else
        {
            animator.SetTrigger("BeamAttack");
        }
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
