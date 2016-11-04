using UnityEngine;
using System.Collections;
using Mecro;

public class Skill_EarthForce_Anim : StateMachineBehaviour {

    private Skill_EarthForce m_SkillObject;
    private bool m_isAnimationLoopEnd = false;
    private bool m_isSkillLanded = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_SkillObject)
            m_SkillObject = MecroMethod.CheckGetComponent<Skill_EarthForce>(animator.gameObject);

        m_isAnimationLoopEnd = false;
        m_isSkillLanded = false;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if (!m_isAnimationLoopEnd && fNormalizeTime > 0.9f)
        {
            m_isAnimationLoopEnd = true;
            m_SkillObject.EndSkill();
        }
        else if (!m_isSkillLanded && fNormalizeTime > 0.15f)
        {
            m_SkillObject.ColliderEnable();
            m_isSkillLanded = true;
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
