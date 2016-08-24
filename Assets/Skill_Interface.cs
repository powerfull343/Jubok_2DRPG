using UnityEngine;
using System.Collections;
using Mecro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GameObject_Extension))]
public abstract class Skill_Interface : MonoBehaviour {

    protected string mSkillName;
    protected Animator mSkillAnim;
    protected int m_CostMana = 2;
    public int CostMana
    {
        get { return m_CostMana; }
    }
    protected Moveable_Type mTargetType;
    protected Moveable_Object mTargetInfo;
    protected Transform mTargetTrans;
    protected GameObject_Extension m_ExtensionInfo;

    protected float mAtk;

    /// <summary>
    /// Setting Variables After Use Method
    /// </summary>
    
    public void InitializingSkill(Transform Target)
    {
        if (!Target)
            SkillSetting();
        else
            SkillSetting(Target);
        SetSkillDamage();
        SetSkillManaCost();
    }
    protected abstract void SkillSetting();
    protected virtual void SkillSetting(Transform Target)
    {
        mTargetTrans = Target;
        transform.position = mTargetTrans.position;
        transform.localScale = Vector3.one;
    }
    protected abstract void SetSkillDamage();
    protected virtual void SetSkillManaCost()
    {
        PlayerCtrlManager.GetInstance().PlayerCtrl.Mp -= m_CostMana;
    }
    public virtual void EndSkill()
    {
        if (mTargetInfo)
            mTargetInfo.SkillTargetFreezing(false);
        SkillManager.GetInstance().SkillList_KeyUseSetting(mSkillName, false);
        m_ExtensionInfo.SelfDestroy();
    }

    //Atk Method
    //it Use not Use Collider
    public void MonsterAttack()
    {
        if(mTargetInfo && mTargetInfo.Hp > 0)
            mTargetInfo.Hp -= (1 * (PlayerCtrlManager.GetInstance().PlayerCtrl.Atk / 2));
        else if (mTargetInfo.Hp <= 0)
        {
            Monster_Interface Mon_interface =
                MagicianCtrl.ColMonsters.Find(x => 
                x.gameObject == mTargetInfo.gameObject);

            if (Mon_interface != null)
            {
                Debug.LogError(Mon_interface.name);
                MagicianCtrl.ColMonsters.Remove(Mon_interface);
            }
            //else
              //  Debug.LogError("Moninter is null");
        }
    }

    private string GetTargetName()
    {
        //type Choice
        string TargetName = string.Empty;
        if (mTargetType == Moveable_Type.TYPE_MONSTER)
            TargetName = "Monster";
        else
            TargetName = "Player";

        return TargetName;
    }

    public void AttackToCollider(Collider other)
    {
        //Damage Setting

        if (other.gameObject.CompareTag(GetTargetName()))
        {
            Moveable_Object ObjectInfo =
                MecroMethod.CheckGetComponent<Moveable_Object>(other.gameObject);
            ObjectInfo.SetHp((int)mAtk);
            Debug.Log("Dam");
            Debug.Log(mAtk);
            //if (ObjectInfo.Hp <= 0)
            //    SkillManager.GetInstance().MonsterHpZero(ObjectInfo);
        }
    }

}
