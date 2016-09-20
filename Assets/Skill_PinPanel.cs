using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mecro;

public class Skill_PinPanel : Skill_Interface {

    [SerializeField]
    private Animator m_ChildMagicCrestAnim;
    [SerializeField]
    private Transform m_ChildSkillRegenPosition;
    [SerializeField]
    private List<Skill_PinPanel_FireBall> m_ChildFireBalls = 
        new List<Skill_PinPanel_FireBall>();
    public List<Skill_PinPanel_FireBall> ChildFireBalls
    { get { return m_ChildFireBalls; } }

    void Start()
    {
        mSkillName = "PinPanel";

        if (!m_ChildMagicCrestAnim)
            Debug.LogError("Cannot Find " + m_ChildMagicCrestAnim.name);

        if (!m_ChildSkillRegenPosition)
            Debug.LogError("Cannot Find " + m_ChildSkillRegenPosition.name);

        if(m_ChildFireBalls.Count == 0)
            Debug.LogError("Cannot Find " + m_ChildFireBalls.ToString());

        mSkillAnim = m_ChildMagicCrestAnim;
        mTargetType = Moveable_Type.TYPE_MONSTER;
        m_ExtensionInfo =
            Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
        m_CostMana = 15;
    }

    protected override void SkillSetting()
    {
        transform.localPosition = m_ChildSkillRegenPosition.localPosition;
        transform.localScale = Vector3.one;
    }

    protected override void SetSkillDamage()
    {
        mAtk =
            PlayerCtrlManager.GetInstance().PlayerCtrl.Atk;
    }

    protected override void SetSkillManaCost()
    {
        base.SetSkillManaCost();
    }

    public void CreateFireBall()
    {
        StartCoroutine("SetFireBall");
    }

    public override void EndSkill()
    {
        m_ChildFireBalls.Clear();
        base.EndSkill();
    }

    IEnumerator SetFireBall()
    {
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < m_ChildFireBalls.Count; ++i)
        {
            if (!m_ChildFireBalls[i].gameObject.activeSelf)
                m_ChildFireBalls[i].gameObject.SetActive(true);

            m_ChildFireBalls[i].FireBallStartPlay();
            m_ChildFireBalls[i].AtkPower = (int)mAtk;
            m_ChildFireBalls[i].ParentSkillInfo = this;
            yield return new WaitForSeconds(0.75f);
        }

        for (int i = 0; i < m_ChildFireBalls.Count; ++i)
            m_ChildFireBalls[i].FireBallShot();

        mSkillAnim.SetBool("AttackStart", true);
        StartCoroutine("CheckingFireBall");

        yield break;
    }

    IEnumerator CheckingFireBall()
    {
        while(true)
        {
            if (m_ChildFireBalls.Count <= 0)
            {
                Debug.Log("FireballAllClear");
                mSkillAnim.SetBool("AttackEnd", true);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

}
