using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class DebugingPanel :
    Singleton<DebugingPanel>
{
    [SerializeField]
    private UISprite m_SpriteBg;

    [SerializeField]
    private UILabel m_Message;
    private GameObject m_MessageInst;

    private Queue<GameObject> m_MessageQueue =
        new Queue<GameObject>();

    [SerializeField]
    private UIGrid m_GridPosition;
    [SerializeField]
    private UIScrollBar m_ScrollBar;

    [SerializeField]
    private int m_QueueMaxCount = 500;
    [SerializeField]
    private int m_PanelRenderingMaxCount = 8;

    [SerializeField]
    private GameObject m_UpButton;
    [SerializeField]
    private GameObject m_DownButton;

    //Up & Down Function
    private delegate void ButtonClickEvent();
    private IEnumerator m_PlayingEvent = null;
    private bool m_isCoroutinePlaying = false;

    //Hide & Show Function
    [SerializeField]
    private Animator m_ChildBodyAnim;
    [SerializeField]
    private UILabel m_ChildFunctionLabel;

    void Awake()
    {
        CreateInstance();

        Debug.Log("Debuging Awake");
        MecroMethod.CheckExistComponent<UISprite>(m_SpriteBg);
        MecroMethod.CheckExistComponent<UILabel>(m_Message);
        MecroMethod.CheckExistComponent<UIGrid>(m_GridPosition);
        MecroMethod.CheckExistComponent<UIScrollBar>(m_ScrollBar);
        MecroMethod.CheckExistObject<GameObject>(m_UpButton);
        MecroMethod.CheckExistObject<GameObject>(m_DownButton);
        MecroMethod.CheckExistComponent<Animator>(m_ChildBodyAnim);
        MecroMethod.CheckExistComponent<UILabel>(m_ChildFunctionLabel);

        m_MessageInst = m_Message.gameObject;
        if (m_MessageInst == null)
            Debug.LogError("Cannot Find Message GameObject");
    }

    void OnEnable()
    {
        StartCoroutine("CheckingUpDownButtonClick");
    }

    void Start()
    {
        if (LobbyManager.LobbyController.GetInstance().mCurrentSceneID
            == FIELDID.ID_VILAGE)
        {
            SetPanelPosition(VilageScene_NGUI_Panel.fScreenWidth,
                VilageScene_NGUI_Panel.fScreenHeight);
        }
        else
        {
            SetPanelPosition(BattleScene_NGUI_Panel.fScreenWidth,
                BattleScene_NGUI_Panel.fScreenHeight);
        }
    }

    private void SetPanelPosition(float _fWidth, float _fHeight)
    {
        float fWidth =
            -(_fWidth / 2f) + (m_SpriteBg.localSize.x / 2f);
        float fHeight =
            -(_fHeight / 2f) + (m_SpriteBg.localSize.y / 2f);

        transform.localPosition =
            new Vector3(fWidth, fHeight, 0f);
    }

    public void AddDebugingLog(object Message)
    {
        if(CheckingFullOfQueue())
        {
            Destroy(m_MessageQueue.Dequeue());
            m_MessageQueue.Enqueue(CopyMessageInst(Message));
        }
        else
            m_MessageQueue.Enqueue(CopyMessageInst(Message));

        m_ScrollBar.value = 1f;
    }

    private GameObject CopyMessageInst(object LogMessage)
    {
        GameObject Createdinstance = null;

        m_Message.text = LogMessage.ToString();
        Createdinstance = Instantiate(m_MessageInst);

        MecroMethod.SetPartent(Createdinstance.transform,
            m_GridPosition.transform);

        m_GridPosition.AddChild(Createdinstance.transform);

        //Createdinstance.transform.localPosition =
        //    m_MessageInst.transform.localPosition;
        Createdinstance.SetActive(true);
        return Createdinstance;
    }

    private bool CheckingFullOfQueue()
    {
        if (m_MessageQueue.Count >= m_QueueMaxCount)
            return true;

        return false;
    }
        
    private IEnumerator CheckingUpDownButtonClick()
    {
        while(true)
        {
            if (UICamera.hoveredObject == m_UpButton &&
                Input.GetMouseButtonDown(0))
            {
                if(!m_isCoroutinePlaying)
                {
                    m_isCoroutinePlaying = true;
                    m_PlayingEvent = ClickingInteraction(m_UpButton, 
                        UpButtonClick);
                    StartCoroutine(m_PlayingEvent);
                }
            }
            else if (UICamera.hoveredObject == m_DownButton &&
                Input.GetMouseButtonDown(0))
            {
                if(!m_isCoroutinePlaying)
                {
                    m_isCoroutinePlaying = true;
                    m_PlayingEvent = ClickingInteraction(m_DownButton,
                        DownButtonClick);
                    StartCoroutine(m_PlayingEvent);
                }
            }
            else
            {
                m_isCoroutinePlaying = false;
            }
            yield return null;
        }

        yield return null;
    }

    public void UpButtonClick()
    {
        m_ScrollBar.value -= 1f / m_MessageQueue.Count;
    }

    public void DownButtonClick()
    {
        m_ScrollBar.value += 1f / m_MessageQueue.Count;
    }

    private IEnumerator ClickingInteraction(
        GameObject _HoveringObject, ButtonClickEvent _PlayEvent)
    {
        float fFlowTime = 0.5f;
        _PlayEvent();

        yield return new WaitForSeconds(fFlowTime);

        while(true)
        {
            if (UICamera.hoveredObject == _HoveringObject &&
                Input.GetMouseButton(0))
            {
                _PlayEvent();

                if (fFlowTime <= 0.1f)
                    fFlowTime = 0.1f;
                else
                    fFlowTime *= 0.8f;
            }
            else
                break;

            yield return new WaitForSeconds(fFlowTime);
        }

        m_isCoroutinePlaying = false;
        StopCoroutine(m_PlayingEvent);
        yield return null;
    }

    public void FuncLogClear()
    {
        GameObject ClearTarget = null;

        while(m_MessageQueue.Count > 0)
        {
            ClearTarget = m_MessageQueue.Dequeue();
            if (ClearTarget == null)
                break;

            Destroy(ClearTarget);
        }

        m_ScrollBar.barSize = 1f;
    }

    public void FuncAnimatorHideAndShow()
    {
        if(m_ChildFunctionLabel.text == "Show")
        {
            m_ChildFunctionLabel.text = "Hide";
            m_ChildBodyAnim.SetTrigger("Show");

        }
        else
        {
            m_ChildFunctionLabel.text = "Show";
            m_ChildBodyAnim.SetTrigger("Hide");
        }
    }
}
