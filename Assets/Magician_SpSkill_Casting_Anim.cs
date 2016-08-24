using UnityEngine;
using System.Collections;

public class Magician_SpSkill_Casting_Anim : StateMachineBehaviour {

    private MagicianCtrl m_PlayerObject;
    private bool m_isAnimationLoopEnd = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_PlayerObject)
        {
            m_PlayerObject =
                Mecro.MecroMethod.CheckGetComponent<MagicianCtrl>(animator.gameObject);
        }

        m_isAnimationLoopEnd = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if(fNormalizeTime > 0.9f && !m_isAnimationLoopEnd)
        {
            m_isAnimationLoopEnd = true;
            m_PlayerObject.SummonSkillEffect();
            m_PlayerObject.ResetAtkMotionSelect();
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
