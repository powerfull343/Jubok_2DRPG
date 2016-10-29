using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class AnimObject_Extension : GameObject_Extension {

    public bool m_isExecuteOnce = true;
    public float m_isExecuteOncetime = 1f;

    /// <summary>
    /// GameObject Has Animation Component
    /// </summary>
    private Animation mAnimation;

    /// <summary>
    /// GameObject has Animator Component
    /// </summary>
    private Animator mAnimator;
    
    /// <summary>
    /// Use Play&Stop Animation Property
    /// </summary>
    private float mOriginAnimSpeed = 1f;

    /// <summary>
    /// if You Want Init Animation Stop -> true
    /// </summary>
    [SerializeField]
    private bool isFirstPlay = true;

    // Use this for initialization
    void Awake() {
        CheckAnimComponent();
    }

    void OnEnable()
    {
        if(!isFirstPlay)
        {
            mOriginAnimSpeed = mAnimator.speed;
            mAnimator.speed = 0f;
        }

        if(m_isExecuteOnce)
            Invoke("SelfDestroy", m_isExecuteOncetime);
    }

    private void CheckAnimComponent()
    {
        mAnimator = MecroMethod.CheckGetComponent<
            Animator>(this.gameObject);

        if (mAnimator)
            return;

        mAnimation = MecroMethod.CheckGetComponent<
            Animation>(this.gameObject);
    }

    public void PlayAnimation()
    {
        if (mAnimator != null)
        {
            mAnimator.speed = mOriginAnimSpeed;
            return;
        }

        if (mAnimation != null)
        {
            mAnimation.Play();
            return;
        }

        Debug.Log("this object hasn't Animator&Animation Component");
        return;
    }

    public void StopAnimation()
    {
        if (mAnimator != null)
        {
            mOriginAnimSpeed = mAnimator.speed;
            mAnimator.speed = 0f;
            return;
        }

        if (mAnimation != null)
        {
            mAnimation.Stop();
            return;
        }

        Debug.Log("this object hasn't Animator&Animation Component");
        return;
    }

    
}
