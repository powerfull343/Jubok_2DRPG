using UnityEngine;
using System.Collections;

public class Magician_Idle_Anim : StateMachineBehaviour {

    private bool m_isOnceLoop = false;
    private int m_nCount = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        Debug.Log("m_nCount : " + m_nCount);

        if (!m_isOnceLoop && fNormalizeTime >= 0.9f)
        {
            m_nCount++;
            m_isOnceLoop = true;
        }
        else if(fNormalizeTime <= 0.1f)
            m_isOnceLoop = false;


        if (m_nCount > 8)
        {
            animator.ResetTrigger("Idle");
            Debug.Log("Animator Move");
            animator.SetTrigger("Move");
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
