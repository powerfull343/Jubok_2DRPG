using UnityEngine;
using System.Collections;

public class Death_SlashAtk_Anim : StateMachineBehaviour {

    private Death MonsterBody;
    private bool m_isSetMonsterBody;

    private bool m_isSetActivate = false;
    private bool m_isEndAnimation = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_isSetMonsterBody)
        {
            MonsterBody = Mecro.MecroMethod.CheckGetComponent<Death>(animator.gameObject);
            m_isSetMonsterBody = true;
        }

        m_isEndAnimation = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if(fNormalizeTime > 0.3f && fNormalizeTime < 0.5f)
        {
            if (!m_isSetActivate)
            {
                //Debug.LogError("Area1");
                MonsterBody.CloneAcitve(0);
                m_isSetActivate = true;
            }
        }
        else if(fNormalizeTime > 0.6f && fNormalizeTime < 0.8f)
        {
            if (!m_isSetActivate)
            {
                //Debug.Log("Area2");
                MonsterBody.CloneAcitve(1);
                m_isSetActivate = true;
            }
        }
        else if(!m_isEndAnimation && fNormalizeTime > 0.9f)
        {
            animator.ResetTrigger("SlashAttack");
            animator.SetTrigger("Idle");
            MonsterBody.bigScytheAttack();
            m_isSetActivate = false;
            m_isEndAnimation = true;
        }
        else
        {
            m_isSetActivate = false;
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
