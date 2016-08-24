using UnityEngine;
using System.Collections;

public class SkeletonBomber_Atk_Anim : StateMachineBehaviour {

    private SkeletonBomber Body;
    private bool ThrowOnce = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Body = Mecro.MecroMethod.CheckGetComponent<SkeletonBomber>(animator.gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Mathf.Repeat(stateInfo.normalizedTime, 1f) > 0.75f &&
            !ThrowOnce)
        {
            Body.ThrowingBomb();
            ThrowOnce = true;
        }
        else if(Mathf.Repeat(stateInfo.normalizedTime, 1f) < 0.1f)
            ThrowOnce = false;
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
