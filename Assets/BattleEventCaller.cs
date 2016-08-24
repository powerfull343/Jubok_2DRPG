using UnityEngine;
using System.Collections;

public class BattleEventCaller : MonoBehaviour {

    //Event Variable
    private IEnumerator m_EventProcess;
    public IEnumerator EventProcess
    {
        get { return m_EventProcess; }
    }

    private PLAYEVENTID m_EventID = PLAYEVENTID.EVENT_NULL;
    public bool m_isFlowStart = false;
    private float m_fEventDelayTime = 5f;
    private float m_fEventStartTime = 0f;
    public float EventStartTime
    {
        get { return m_fEventStartTime; }
        set { m_fEventStartTime = value; }
    }
    private float m_fEventRetentionTime = 0f;
    public float EventRetentionTime
    {
        get { return m_fEventRetentionTime; }
        set { m_fEventRetentionTime = value; }
    }
    private float m_fEventEndTime = 10f;
    public float EventEndTime
    {
        get { return m_fEventEndTime; }
        set { m_fEventEndTime = value; }
    }
    private float m_fEventProgressRate = 1f;
    public float EventProgressRate
    { get { return m_fEventProgressRate; } }

    private float m_OriginRegenMinSec;
    private float m_OriginRegenMaxSec;
    private int m_OriginRegenMaxCount;

    [SerializeField]
    private EventTextRendering m_EventRenderingComp;

    public bool m_isEventCall = true;


    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<EventTextRendering>(m_EventRenderingComp);


        if (m_isEventCall)
        {
            m_EventProcess = ProcessEvents();
            StartCoroutine(m_EventProcess);
        }
        
    }

    IEnumerator ProcessEvents()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (m_fEventStartTime != 0f)
            { //이벤트 유지중
                CheckingEventRetention();
                yield return new WaitForFixedUpdate();
            }
            else
            { //이벤트가 없을때
                if (EventChangeSystem())
                    yield return new WaitForFixedUpdate();
                else
                    yield return new WaitForSeconds(m_fEventDelayTime);
            }
        }
        yield break;
    }

    private void CheckingEventRetention()
    {
        FlowEventTime();

        if (m_fEventRetentionTime >= m_fEventEndTime)
        {
            //이벤트 상황에 따라 다른 함수를 채용시킨다.
            ResetRegenSpeed();
            m_fEventStartTime = 0f;
            m_fEventRetentionTime = 0f;
            m_EventID = PLAYEVENTID.EVENT_NULL;
            m_EventRenderingComp.ResetEventRendering();
            m_fEventProgressRate = 1f;
            m_isFlowStart = false;
        }
    }

    public void FlowEventTime()
    {
        if (m_isFlowStart)
        {
            m_fEventRetentionTime = Time.time - m_fEventStartTime;
            m_fEventProgressRate = (m_fEventEndTime - m_fEventRetentionTime) / m_fEventEndTime;
        }
    }

    /// <summary>
    /// isCanival = true(StartEvent), false(EndEvent)
    /// </summary>
    /// <param name="isCanival"></param>
    public void ChangeRegenSpeed(bool isCanival)
    {
        if (isCanival)
        {
            MonsterManager.GetInstance().RegenMinWidth = 0f;
            MonsterManager.GetInstance().RegenMaxWidth = 2f;
            MonsterManager.MonsterMaxCount = 20;
            MonsterManager.GetInstance().StartRegenTime = 0;
            StopCoroutine(MonsterManager.GetInstance().iRegenMonster);
            StartCoroutine(MonsterManager.GetInstance().iRegenMonster);
        }
        else
        {
            m_OriginRegenMinSec = MonsterManager.GetInstance().RegenMinWidth;
            m_OriginRegenMaxSec = MonsterManager.GetInstance().RegenMaxWidth;
            m_OriginRegenMaxCount = MonsterManager.MonsterMaxCount;
        }
    }

    public void ResetRegenSpeed()
    {
        MonsterManager.GetInstance().RegenMinWidth = m_OriginRegenMinSec;
        MonsterManager.GetInstance().RegenMaxWidth = m_OriginRegenMaxSec;
        MonsterManager.MonsterMaxCount = m_OriginRegenMaxCount;
        MonsterManager.GetInstance().StartRegenTime = 1f;
    }

    private bool EventChangeSystem()
    {
        int nRandomOpenEventIndex = Random.Range(0, 100);
        if (nRandomOpenEventIndex < 0)
            return false;

        int nRandomEventIndex = Random.Range(0, (int)PLAYEVENTID.EVENT_MAX);
        m_EventID = (PLAYEVENTID)nRandomEventIndex;

        m_fEventStartTime = Time.time;
        Debug.Log(m_fEventStartTime);
        m_fEventProgressRate = 1f;
        m_EventRenderingComp.StartEventTextRendering("Canival Time");

        return true;
    }

    public void CanivalTimeSetting()
    {
        //여기서 진짜 이벤트 시작시간을 발생시킨다.
        EventStartTime = Time.time;
        m_isFlowStart = true;

        //진짜시작
        
        //원래 스피드를 먼저 복사한다.
        ChangeRegenSpeed(false);
        //스피드 상승
        ChangeRegenSpeed(true);
    }
}
