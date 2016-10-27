using UnityEngine;
using System.Collections;

public class SkeletonBoomerang_Atk_Anim : StateMachineBehaviour {

    private SkeletonBoomerang m_BodyInfo;
    private bool m_isThrowBoomerang;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_BodyInfo = Mecro.MecroMethod.CheckGetComponent<SkeletonBoomerang>(animator.gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if (!m_isThrowBoomerang && fNormalizeTime > 0.7f)
        {
            m_BodyInfo.ThrowBoomerang();

            m_isThrowBoomerang = true;
        }
        else if (fNormalizeTime < 0.1f)
            m_isThrowBoomerang = false;

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
