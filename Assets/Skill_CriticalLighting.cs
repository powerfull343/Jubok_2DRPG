using UnityEngine;
using System.Collections;
using System;
using Mecro;

public class Skill_CriticalLighting : Skill_Interface {

    [SerializeField]
    private Skill_CriticalLighting_Effect m_ChildEffect;
    // Use this for initialization
    void Start () {
        mSkillName = "CriticalLighting";
        mSkillAnim = null;
        mTargetType = Moveable_Type.TYPE_MONSTER;
        m_ExtensionInfo =
            MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);

        MecroMethod.CheckExistComponent(m_ChildEffect);
        m_ChildEffect.ParentSKillInfo = this;

        Invoke("EndSkill", 4f);
    }

    protected override void SkillSetting()
    {
        
    }

    protected override void SkillSetting(Transform Target)
    {
        base.SkillSetting(Target);
    }

    protected override void SetSkillDamage()
    {
        Debug.Log("Atk : " + PlayerCtrlManager.GetInstance().PlayerCtrl.Atk);

        mAtk =
            PlayerCtrlManager.GetInstance().PlayerCtrl.Atk * 4;
    }

    protected override void SetSkillManaCost()
    {
        //No Mana Cost Skill
    }

    public override void EndSkill()
    {
        m_ExtensionInfo.SelfDestroy();
    }
}
