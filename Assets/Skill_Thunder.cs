using UnityEngine;
using System.Collections;
using Mecro;
using System.Collections.Generic;
using System.Linq;
using System;

public class Skill_Thunder : Skill_Interface {

    //static Skill_Thunder()
    //{
    //    Debug.LogError("STaticThunder TEst");
    //}

    void Start()
    {
        mSkillName = "Thunder02";
        mSkillAnim = Mecro.MecroMethod.CheckGetComponent<Animator>(this.gameObject);
        //misChargeUse = false;
        //misMultiGameObject = false;
        mTargetType = Moveable_Type.TYPE_MONSTER;
        m_ExtensionInfo =
            Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);

        m_CostMana = 5;
    }

    protected override void SkillSetting()
    {
        transform.position = transform.FindChild("ThunderRespwan").localPosition;
        transform.localScale = new Vector3(1.5f, 1.9f, 1f);
    }


    protected override void SetSkillDamage()
    {
        mAtk =
            PlayerCtrlManager.GetInstance().PlayerCtrl.Atk * 5f;
    }

    protected override void SetSkillManaCost()
    {
        base.SetSkillManaCost();
    }

    public override void EndSkill()
    {
        base.EndSkill();
    }

    void OnTriggerEnter(Collider other)
    {
        AttackToCollider(other);
    }

}
