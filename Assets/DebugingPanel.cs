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

    private int m_nQueueMaxCount = 0;

    void Awake()
    {
        CreateInstance();

        Debug.Log("Debuging Awake");
        MecroMethod.CheckExistComponent<UISprite>(m_SpriteBg);
        MecroMethod.CheckExistComponent<UILabel>(m_Message);
        m_MessageInst = m_Message.gameObject;
        if (m_MessageInst == null)
            Debug.LogError("Cannot Find Message GameObject");

        float fSize = m_SpriteBg.localSize.y / m_Message.localSize.y;
        m_nQueueMaxCount = (int)fSize;
    }

    void Start()
    {
        if(LobbyManager.LobbyController.GetInstance().mCurrentSceneID == FIELDID.ID_VILAGE)
        {
            float fHeight =
                -(VilageScene_NGUI_Panel.fScreenHeight / 2f) +
                (m_SpriteBg.localSize.y / 2f);
            float fWidth =
                -(VilageScene_NGUI_Panel.fScreenWidth / 2f) +
                (m_SpriteBg.localSize.x / 2f);

            transform.localPosition =
                new Vector3(fWidth, fHeight, 0f);
        }
        else
        {
            float fHeight =
                -(BattleScene_NGUI_Panel.fScreenHeight / 2f) +
                (m_SpriteBg.localSize.y / 2f);
            float fWidth =
                -(BattleScene_NGUI_Panel.fScreenWidth / 2f) +
                (m_SpriteBg.localSize.x / 2f);

            transform.localPosition =
                new Vector3(fWidth, fHeight, 0f);
        }
    }

    public void ShowDebugingLog(string Message)
    {
        if (CheckingFullOfQueue())
        {
            Destroy(m_MessageQueue.Dequeue());
            PushingUpLogMessage();
            m_MessageQueue.Enqueue(CopyMessageInst(Message));
            
        }
        else
        {
            PushingUpLogMessage();
            m_MessageQueue.Enqueue(CopyMessageInst(Message));
        }
    }

    private GameObject CopyMessageInst(string LogMessage)
    {
        GameObject Createdinstance = null;

        m_Message.text = LogMessage;
        Createdinstance = Instantiate(m_MessageInst);

        MecroMethod.SetPartent(Createdinstance.transform,
            this.transform);

        Createdinstance.transform.localPosition = 
            m_MessageInst.transform.localPosition;
        Createdinstance.SetActive(true);

        return Createdinstance;
    }

    private bool CheckingFullOfQueue()
    {
        if (m_MessageQueue.Count >= m_nQueueMaxCount)
            return true;

        return false;
    }

    private void PushingUpLogMessage()
    {
        if (m_MessageQueue.Count < 1)
            return;

        foreach(GameObject Message in m_MessageQueue)
            Message.transform.localPosition += new Vector3(0f, m_Message.localSize.y, 0f);
    }
        

}
