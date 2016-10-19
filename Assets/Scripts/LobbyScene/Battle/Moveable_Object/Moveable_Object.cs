using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Moveable_Object : MonoBehaviour {

    protected int _Hp;
    public int Hp
    {
        get { return _Hp; }
        set
        {
            if(_Hp > value)
            {
                if(mStateAnim != null)
                    mStateAnim.SetTrigger("Hit");
            }

            _Hp = value;
        }
    }

    protected int _MaxHp;
    public int MaxHp
    {
        get { return _MaxHp; }
        set { _MaxHp = value; }
    }

    protected int _Mp;
    public int Mp
    {
        get { return _Mp; }
        set { _Mp = value; }
    }

    protected int _MaxMp;
    public int MaxMp
    {
        get { return _MaxMp; }
        set { _MaxMp = value; }
    }

    protected int _Atk;
    public int Atk
    {
        get { return _Atk; }
        set { _Atk = value; }
    }

    protected int _Stamina;
    public int Stamina
    {
        get { return _Stamina; }
        set { _Stamina = value; }
    }

    protected int _MaxStamina;
    public int MaxStamina
    {
        get { return _MaxStamina; }
        set { _MaxStamina = value; }
    }
    protected float _AtkSpeed = 1f;
    public float AtkSpeed
    {
        get { return _AtkSpeed; }
        set { _AtkSpeed = value; }
    }
    public Moveable_Type ObjectType;
    protected string m_ObjectName;
    public string ObjectName
    {
        get { return m_ObjectName; }
        set { m_ObjectName = value; }
    }
    
    protected bool isSkilledCondition = false;

    protected Transform mStateAnimTransform;
    protected Animator mStateAnim;

    public abstract void AutoAction();
    protected abstract void Move();
    protected abstract void Attack();
    protected abstract void Fatal();
    protected virtual void RangeAttack() { }
    protected virtual void MeleeAttack() { }

    public bool SkillTargetFreezing(bool isHit)
    {
        isSkilledCondition = isHit;
        return isSkilledCondition;
    }

    public abstract bool SetHp(int discountAmount);

    public virtual void ClearAllData() { }

    
}
