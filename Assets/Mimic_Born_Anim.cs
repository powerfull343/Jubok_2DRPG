using UnityEngine;
using System.Collections;

public class Mimic_Born_Anim : StateMachineBehaviour {

    private Mimic m_OwnInfo;
    private Transform m_MovingTarget;
    private Vector3 m_Destination;
    private float m_fDestinationLength;
    private bool m_isAnimationEnd = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_OwnInfo)
            m_OwnInfo = Mecro.MecroMethod.CheckGetComponent<Mimic>(animator.gameObject);

        m_MovingTarget = m_OwnInfo.transform.parent;
        m_Destination = MonsterManager.GetInstance().InFieldPosition.localPosition;
        m_fDestinationLength = Vector3.Distance(m_MovingTarget.localPosition, m_Destination);

        m_isAnimationEnd = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);
        float fSpeed = fNormalizeTime * 0.5f;
        float fFrequency = fSpeed / m_fDestinationLength;

        Debug.Log("fFrequency : " + fFrequency);

        m_MovingTarget.localPosition = Vector3.Lerp(m_MovingTarget.localPosition,
            m_Destination, fFrequency);


        if (fNormalizeTime > 0.95f && !m_isAnimationEnd)
        {
            m_MovingTarget.localPosition = m_Destination;
            m_isAnimationEnd = true;
            animator.SetTrigger("BornEnd");
            m_OwnInfo.BornEnd();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
