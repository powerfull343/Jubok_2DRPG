using UnityEngine;
using System.Collections;

public class ProgressTime : MonoBehaviour {

    public bool m_isFlowStart = false;
    /// <summary>
    /// if Reapeat Count -1 is Infinity
    /// </summary>
    private int m_RoutineMaxRepeatCount = -1;
    public int RoutineMaxRepeatCount
    {
        get { return m_RoutineMaxRepeatCount; }
        set { m_RoutineMaxRepeatCount = value; }
    }
    private int m_CurrentRoutineCount = 0;
    public int CurrentRoutineCount
    {
        get { return m_CurrentRoutineCount; }
    }

    private float m_fStartDelayTime = 0f;
    public float StartDelayTime
    {
        get { return m_fStartDelayTime; }
        set { m_fStartDelayTime = value; }
    }
    private float m_fStartTime = 0f;
    public float StartTime
    {
        get { return m_fStartTime; }
    }
    private float m_fRetentionTime = 0f;
    public float RetentionTime
    {
        get { return m_fRetentionTime; }
    }
    private float m_fEndTime = 10f;
    public float EndTime
    {
        get { return m_fEndTime; }
        set { m_fEndTime = value; }
    }
    private float m_fProgressRate = 1f;
    public float ProgressRate
    {
        get { return m_fProgressRate; }
    }

    /// <summary>
    /// if Max Reapeat Count -1 is Infinity
    /// </summary>
    public void InitTime(float _fStartDelayTime, float _fEndTimeAmount, int MaxRepeatCount)
    {
        EndTime = _fEndTimeAmount;
        StartDelayTime = _fStartDelayTime;
        m_RoutineMaxRepeatCount = MaxRepeatCount;
    }

    public void Begin()
    {
        StartCoroutine("StartFlowTime");
    }

    IEnumerator StartFlowTime()
    {
        yield return new WaitForSeconds(m_fStartDelayTime);

        m_CurrentRoutineCount = 0;
        m_isFlowStart = true;
        m_fStartTime = Time.time;

        while (true)
        {
            if (RoutineMaxRepeatCount >= m_CurrentRoutineCount)
                break;
            else
                ResetTime();

            if(CalcTime())
                m_CurrentRoutineCount++;
            yield return new WaitForFixedUpdate();
        }

        m_isFlowStart = false;
        yield break;
    }

    private bool CalcTime()
    {
        //Start by 0 to 1
        m_fProgressRate = (Time.time - m_fStartTime) / m_fEndTime;

        if(m_fProgressRate >= 1f)
            return true;

        return false;
    }

    private void ResetTime()
    {
        m_fStartTime = Time.time;
        m_fProgressRate = 0f;
    }
}
