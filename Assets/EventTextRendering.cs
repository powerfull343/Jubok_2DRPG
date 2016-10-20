using UnityEngine;
using System.Collections;

public class EventTextRendering : MonoBehaviour {

    private AutoTyping m_AutoTypingComp;
    private IEnumerator m_RetentionEvent;
    private UILabel m_EventLabelComp;

    [SerializeField]
    private Transform m_ProgressBarTrans;
    private UISprite m_ProgressRateSprite;

	void Start () {
        m_AutoTypingComp = Mecro.MecroMethod.CheckGetComponent<AutoTyping>(this.gameObject);
        m_EventLabelComp = Mecro.MecroMethod.CheckGetComponent<UILabel>(this.gameObject);

        Mecro.MecroMethod.CheckExistComponent<Transform>(m_ProgressBarTrans);
        m_ProgressRateSprite =
            Mecro.MecroMethod.CheckGetComponent<UISprite>(m_ProgressBarTrans.FindChild("Sprite - BackGround"));

        m_RetentionEvent = RetentionEvent();
    }

    void Update()
    {
        if (m_AutoTypingComp.m_isTypingEnd)
        {
            EventTextMiniModeAnimation();
            m_AutoTypingComp.m_isTypingEnd = false;
        }
    }

    public void StartEventTextRendering(string strTarget)
    {
        m_AutoTypingComp.StartAction(strTarget);
    }

    public void ResetEventRendering()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        Debug.Log(transform.localPosition);
        Debug.Log(transform.localScale);
        m_EventLabelComp.text = string.Empty;
        m_RetentionEvent = RetentionEvent();
        m_ProgressBarTrans.gameObject.SetActive(false);
    }

    public void ResetAllTextRendering()
    {
        StopAllCoroutines();
        m_AutoTypingComp.ResetAutoTyping();
        ResetEventRendering();

        TweenPosition.Begin(this.gameObject, 1f, Vector3.zero);
        TweenScale.Begin(this.gameObject, 1f, Vector3.one);
    }

    void EventTextMiniModeAnimation()
    {
        Vector3 vScaleRate = new Vector3(0.45f, 0.45f, 1f);
        //50 = upperUISize
        Vector3 vFinalPosition = new Vector3(
            (BattleScene_NGUI_Panel.fScreenWidth / 2f) - ((m_EventLabelComp.localSize.x * vScaleRate.x) / 2f),
            (BattleScene_NGUI_Panel.fScreenHeight / 2f) - ((m_EventLabelComp.localSize.y * vScaleRate.y) / 2f) - 100, 0f);

        //Test
        TweenPosition.Begin(this.gameObject, 2f, vFinalPosition);
        TweenScale.Begin(this.gameObject, 2f, vScaleRate);
        
        StartCoroutine(m_RetentionEvent);
    }

    IEnumerator RetentionEvent()
    {
        m_ProgressBarTrans.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.1f);
        MonsterManager.GetInstance().BattleEventCaller.CanivalTimeSetting();

        m_ProgressBarTrans.gameObject.SetActive(true);

        while (true)
        {
            m_ProgressRateSprite.fillAmount = 
                MonsterManager.GetInstance().BattleEventCaller.EventProgressRate;
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    
	
}
