using UnityEngine;
using System.Collections;
using Mecro;

public class MeleeAtk_Anim : StateMachineBehaviour {

    public Moveable_Type m_AttackTargetType = Moveable_Type.TYPE_PLAYER;

    /// <summary>
    /// this value cannot over than 1f
    /// </summary>
    public float m_fAttackTime = 0.9f;


    private Moveable_Object m_BodyInfo;
    private bool m_isOnceAttack = false;
    private bool m_isInitializing = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_isInitializing)
            Initializing(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float fNormalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1f);

        if (!m_isOnceAttack && fNormalizeTime > m_fAttackTime)
            TargetAttack();
        else if (fNormalizeTime < 0.05f)
            m_isOnceAttack = false;
    }

    void TargetAttack()
    {
        switch(m_AttackTargetType)
        {
            case Moveable_Type.TYPE_PLAYER:
                PlayerCtrlManager.GetInstance().PlayerCtrl.SetHp(m_BodyInfo.Atk);
                break;

            case Moveable_Type.TYPE_MONSTER:
                break;

            default:
                break;
        }

        m_isOnceAttack = true;
    }

    void Initializing(Animator animator)
    {
        m_BodyInfo = MecroMethod.CheckGetComponent<Moveable_Object>(animator.gameObject);
        m_isInitializing = true;
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
