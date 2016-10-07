using UnityEngine;
using System.Collections;
using LobbyManager;
using Mecro;

public class BattleScene_NGUI_Panel 
    : Singleton<BattleScene_NGUI_Panel> {

    public static float fScreenWidth = 1280;
    public static float fScreenHeight = 720;

    [SerializeField]
    private BossHpBarCtrlManager m_BossHpManager;
    public BossHpBarCtrlManager BossHpManager
    {
        get { return m_BossHpManager; }
        //set { m_BossHpManager = value; }
    }

    [SerializeField]
    private Battle_NGUI_EventMsg m_EventMessageCtrl;
    public Battle_NGUI_EventMsg EventMessageCtrl
    {
        get { return m_EventMessageCtrl; }
        //set { m_EventMessageCtrl = value;}
    }

    [SerializeField]
    private Camera m_NGUICamera;
    public Camera NGUICamera
    {
        get { return m_NGUICamera; }
        set { m_NGUICamera = value; }
    }

    private GameObject m_TempCollider;
    private UIPanel m_TempColliderComp;

    void Awake()
    {
        CreateInstance();
        MecroMethod.CheckExistComponent<BossHpBarCtrlManager>(m_BossHpManager);
        MecroMethod.CheckExistComponent<Battle_NGUI_EventMsg>(m_EventMessageCtrl);
        MecroMethod.CheckExistComponent<Camera>(m_NGUICamera);
        if (!m_TempCollider)
        {
            m_TempCollider =
                Instantiate(Resources.Load(
                    "UIPanels/Panel - CannotControl") as GameObject);
            Mecro.MecroMethod.SetPartent(m_TempCollider.transform,
                this.transform);
        }
    }

    void OnEnable()
    {
        Invoke("UpperPanelMoving", 0.05f); 
    }

    private void UpperPanelMoving()
    {
        LobbyController.GetInstance().UpperStatusPanel.MovingUpperStatusUI(
            this.transform, fScreenHeight);
        //if BattleScene Start Reducing Statmina
        LobbyController.GetInstance().UpperStatusPanel.StartReducingStamina();
    }

    public void OpenBehindCollider()
    {
        m_TempCollider.SetActive(true);
    }

    public void CloseBehindCollider()
    {
        m_TempCollider.SetActive(false);
    }

    public void CallEventMessage(string strMessage, Color LabelColor)
    {
        EventMessageCtrl.CallEventMessage(strMessage, LabelColor);
    }

    public void CreateBossHpBar(string MonsterKey)
    {
        BossHpManager.CreateHpBar(MonsterKey);
    }

    public BossHpBar GetSummonedHpBar()
    {
        return BossHpManager.SummonedBossHpBar;
    }

    public void SetBehindColliderDepth(int ChangeDepthAmount)
    {
        if (!m_TempColliderComp)
        {
            m_TempColliderComp =
                MecroMethod.CheckGetComponent<UIPanel>(m_TempCollider);
        }

        m_TempColliderComp.depth = ChangeDepthAmount;
        m_TempColliderComp.Update();
    }
}
