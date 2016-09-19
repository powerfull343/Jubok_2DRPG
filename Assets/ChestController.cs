using UnityEngine;
using System;
using System.Collections;

public class ChestController : MonoBehaviour
{
    private Vector3 m_Center;
    private Vector3 m_FallPosition;
    private Vector3 m_StartPosition;
    private float m_StartTime;
    private float m_FallTime = 0.5f;

    public bool m_isCoin = false;
    private int m_MoneySize = 1;
    public int MoneySize
    {
        get { return m_MoneySize; }
        set { m_MoneySize = value; }
    }

    private bool m_isAnimationLoopEnd = false;
    

    void Start()
    {
        m_StartTime = Time.time;

        Vector3 vFlyPower = new Vector3(UnityEngine.Random.Range(50f, 100f),
            UnityEngine.Random.Range(-10f, 50f), 0f);

        m_StartPosition = transform.localPosition;
        m_StartPosition.z = 0f;
        m_FallPosition = transform.localPosition + vFlyPower;
        m_FallPosition.z = 0f;

        m_Center = m_StartPosition + (vFlyPower / 2f) + new Vector3(0f, 200f, 0f);

        if (m_isCoin)
            StartCoroutine("RotateAnimation");

        StartCoroutine("DropAnimation");
    }

    IEnumerator RotateAnimation()
    {
        float fRotateSpeed = UnityEngine.Random.Range(2f, 8f);
        while(true)
        {
            transform.Rotate(new Vector3(0f, fRotateSpeed, 0f));
            if (m_isAnimationLoopEnd)
                break;

            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    IEnumerator DropAnimation()
    {
        while(true)
        {
            float fracComplete = (Time.time - m_StartTime) / m_FallTime;

            Vector3 StartRelCenter = Vector3.Lerp(m_StartPosition, m_Center, fracComplete);
            Vector3 DropRelCenter = Vector3.Lerp(m_Center, m_FallPosition, fracComplete);
            transform.localPosition = Vector3.Lerp(StartRelCenter, DropRelCenter, fracComplete);

            if (fracComplete >= 1f)
            {
                //Debug.LogError("End");
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        //===if you bag overweight===//

        //==그게 아닌 경우==//

        //Mimic인 경우//

        StartCoroutine("HideAnimation");
        yield break;
    }

    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(2f);

        m_FallTime = UnityEngine.Random.Range(0.05f, 0.25f);
        m_StartPosition = transform.localPosition;
        m_FallPosition = ItemDropManager.GetInstance().ObtainPosition;
        m_Center = ((m_StartPosition + m_FallPosition) * 0.5f) + new Vector3(0f, 3f, 0f);
        m_StartTime = Time.time;
        bool isSmall = false;

        while (true)
        {
            float frecComplete = (Time.time - m_StartTime) / m_FallTime;
 
            Vector3 StartRelCenter = Vector3.Lerp(m_StartPosition, m_Center, frecComplete);
            Vector3 DropRelCenter = Vector3.Lerp(m_Center, m_FallPosition, frecComplete);
            transform.localPosition = Vector3.Lerp(StartRelCenter, DropRelCenter, frecComplete);

            if (frecComplete >= 1f)
            {
                //Debug.LogError("End");
                m_isAnimationLoopEnd = true;
                break;
            }
            else if(frecComplete >= 0.5f && !isSmall)
            {
                isSmall = true;
                TweenScale.Begin(this.gameObject, (1f - frecComplete), new Vector3(0.25f, 0.25f, 0.25f));
            }

            yield return new WaitForFixedUpdate();
        }

        if (m_isCoin)
        {
            LobbyManager.LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(m_MoneySize);
        }

        Destroy(this.gameObject);
        yield return null;
    }
}	
