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

    public int m_CanivalEventRate = 30;

    [SerializeField]
    private EventTextRendering m_EventRenderingComp;

    //Event Progress Looping
    public bool m_isEventCall = true;
    private bool m_isPreviousEvent = false;

    void Awake()
    {
        Mecro.MecroMethod.CheckExistComponent<EventTextRendering>(m_EventRenderingComp);
        
    }

    void OnEnable()
    {
        if (m_isEventCall)
        {
            InitOriginRegenValues();
            m_EventProcess = ProcessEvents();
            StartCoroutine(m_EventProcess);
        }
    }

    private void InitOriginRegenValues()
    {
        m_OriginRegenMinSec = MonsterManager.GetInstance().RegenMinWidth;
        m_OriginRegenMaxSec = MonsterManager.GetInstance().RegenMaxWidth;
        m_OriginRegenMaxCount = MonsterManager.MonsterMaxCount;
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
    public void ChangeRegenSpeed()
    {
        MonsterManager.GetInstance().RegenMinWidth = 0f;
        MonsterManager.GetInstance().RegenMaxWidth = 2f;
        MonsterManager.MonsterMaxCount = 20;
        MonsterManager.GetInstance().StartRegenTime = 0;
        StopCoroutine(MonsterManager.GetInstance().iRegenMonster);
        StartCoroutine(MonsterManager.GetInstance().iRegenMonster);
    }

    public void ResetRegenSpeed()
    {
        MonsterManager.GetInstance().RegenMinWidth = m_OriginRegenMinSec;
        MonsterManager.GetInstance().RegenMaxWidth = m_OriginRegenMaxSec;
        MonsterManager.MonsterMaxCount = m_OriginRegenMaxCount;
        MonsterManager.GetInstance().StartRegenTime = 1f;

        m_fEventStartTime = 0f;
        m_fEventRetentionTime = 0f;
        m_EventID = PLAYEVENTID.EVENT_NULL;
        m_EventRenderingComp.ResetEventRendering();
        m_fEventProgressRate = 1f;
        m_isFlowStart = false;
    }

    //Canival Time Event
    private bool EventChangeSystem()
    {
        //이벤트가 연속적으로 발생하는 것을 방지하기 위해 막아준다.
        if (m_isPreviousEvent)
        {
            m_isPreviousEvent = false;
            return false;
        }

        int nRandomOpenEventIndex = Random.Range(0, 100);
        //Debug.Log("nRandomOpenEventIndex : " + nRandomOpenEventIndex);
        if (nRandomOpenEventIndex <= 100 - m_CanivalEventRate)
            return false;

        int nRandomEventIndex = Random.Range(0, (int)PLAYEVENTID.EVENT_MAX);
        m_EventID = (PLAYEVENTID)nRandomEventIndex;

        m_fEventStartTime = Time.time;
        //Debug.Log(m_fEventStartTime);
        m_fEventProgressRate = 1f;
        InitOriginRegenValues();
        m_EventRenderingComp.StartEventTextRendering("Canival Time");

        //이벤트가 연속적으로 발생하는 것을 방지하기 위해 막아준다.
        m_isPreviousEvent = true;
        return true;
    }

    public void CanivalTimeSetting()
    {
        //여기서 진짜 이벤트 시작시간을 발생시킨다.
        EventStartTime = Time.time;
        m_isFlowStart = true;

        //진짜시작
        
        //스피드 상승
        ChangeRegenSpeed();
    }

    public void ResetEventCaller()
    {
        //이미 이벤트가 진행중일때
        if (m_isFlowStart)
            ResetRegenSpeed();
        //이벤트가 진행중이지 않을시
        else
        {
            //이벤트 글씨 효과도 발생하지 않았을때
            if (m_fEventStartTime == 0f)
            {
                Debug.Log("not Reset");
                return;
            }
            //글시 효과 발생중 이벤트 종료시
            else
            {
                Debug.Log("ResetEventRendering");
                m_EventRenderingComp.StopAllCoroutines();
                m_EventRenderingComp.ResetAllTextRendering();
                ResetRegenSpeed();
            }
        }
    }
}
