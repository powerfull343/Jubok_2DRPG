using UnityEngine;
using System.Collections;
using Mecro;

public class Skill_EarthForce_CallForceGate_Anim : StateMachineBehaviour {

    private Skill_EarthForce m_ParentSkillInfo;
    private bool m_isAnimationEnd = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_ParentSkillInfo)
        {
            m_ParentSkillInfo = 
                MecroMethod.CheckGetComponent<Skill_EarthForce>(
                    animator.transform.parent.gameObject);
        }
        m_isAnimationEnd = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);
        if (fNormalTime >= 0.9f && !m_isAnimationEnd)
        {
            m_ParentSkillInfo.SummonEarthForce();
            m_isAnimationEnd = true;
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
