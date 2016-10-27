using UnityEngine;
using System.Collections;
using Mecro;

public class UI_GroceryStore_StatusMsg : MonoBehaviour {

    public float m_fHidingTime = 0.5f;
    private UIPanel m_OwnPanel;

    [SerializeField]
    private Transform m_MessageWindow;
    [SerializeField]
    private UISprite m_BG;
    [SerializeField]
    private UILabel m_StatMsg;

    private bool m_flowMessage = false;

    void Awake()
    {
        m_OwnPanel = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);
        MecroMethod.CheckExistComponent<Transform>(m_MessageWindow);
        MecroMethod.CheckExistComponent<UISprite>(m_BG);
        MecroMethod.CheckExistComponent<UILabel>(m_StatMsg);
    }

    void OnEnable()
    {
        ResetCondition();
        m_flowMessage = false;
    }

    private void ResetCondition()
    {
        m_OwnPanel.alpha = 1f;
        m_MessageWindow.localPosition = Vector3.zero;
    }

    public void StartingShowMessage(string EventText)
    {
        ResetCondition();
        m_StatMsg.text = EventText;
        if (!m_flowMessage)
            StartCoroutine("HideMessage");
        else
        {
            StopCoroutine("HideMessage");
            StartCoroutine("HideMessage");
        }
    }

    private IEnumerator HideMessage()
    {
        m_flowMessage = true;
        float fFlowTime = 0f;
        float fFrequency = 0f;
        float fDestinationYValue = 50f;

        while(m_OwnPanel.alpha > 0f)
        {
            fFlowTime += Time.deltaTime;
            fFrequency = fFlowTime / m_fHidingTime;
            //1. Window Position Up
            m_MessageWindow.localPosition = new Vector3(0f,
                (fDestinationYValue * fFrequency), 0f);

            //2. Panel's Alpha value discount
            m_OwnPanel.alpha = 1f - fFrequency;

            yield return null;
        }

        gameObject.SetActive(false);
        yield return null;
        
    }

    
}
