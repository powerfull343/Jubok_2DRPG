using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoTyping : MonoBehaviour
{

    private UILabel m_Labelcomp;

    private IEnumerator m_TypingCoroutine;
    private string m_TypingText;
    private int m_TypingIdx;
    private int m_TypingMaxIdx;
    public bool m_isTypingEnd = false;

    public float m_DelayTime = 0.15f;

    void Start()
    {
        m_Labelcomp = Mecro.MecroMethod.CheckGetComponent<UILabel>(this.transform);
        m_TypingCoroutine = AutoTypingAction(m_DelayTime);
    }

    public void StartAction(string TargetText)
    {
        InitAutoTyping();
        m_TypingText = TargetText;
        m_TypingMaxIdx = m_TypingText.Length;

        Debug.Log("StartAction");
        StartCoroutine(m_TypingCoroutine);
    }

    public void InitAutoTyping()
    {
        m_TypingIdx = 1;
        m_TypingMaxIdx = 0;
        m_isTypingEnd = false;

        m_TypingCoroutine = AutoTypingAction(m_DelayTime);
    }

    private IEnumerator AutoTypingAction(float fDelayTime)
    {
        while (m_TypingIdx <= m_TypingMaxIdx)
        {
            m_Labelcomp.text = m_TypingText.Substring(0, m_TypingIdx++);
            yield return new WaitForSeconds(fDelayTime);
        }
        m_isTypingEnd = true;
        yield break;
    }

    


}
