using UnityEngine;
using System.Collections;

public class Death_Teleport_Anim : StateMachineBehaviour {

    private Death MonsterBody;
    private bool m_isSetMonsterBody = false;
    private bool m_isPositionChange = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        if(!m_isSetMonsterBody)
        {
            MonsterBody = Mecro.MecroMethod.CheckGetComponent<Death>(animator.gameObject);
            m_isSetMonsterBody = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormailzeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if(fNormailzeTime > 0.9f)
        {
            animator.SetTrigger("Idle");
            m_isPositionChange = false;
        }

        if(!m_isPositionChange && fNormailzeTime > 0.6f && fNormailzeTime < 0.7f)
        {
            MonsterBody.Teleport();
            m_isPositionChange = true;
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
