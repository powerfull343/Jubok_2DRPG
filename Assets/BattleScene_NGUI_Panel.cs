using UnityEngine;
using System.Collections;
using Mecro;

public class BattleScene_NGUI_Panel : Scene_Panel_Interface
{ 
    [SerializeField]
    private BossHpBarCtrlManager m_BossHpManager;
    public BossHpBarCtrlManager BossHpManager
    {
        get { return m_BossHpManager; }
    }

    [SerializeField]
    private Battle_NGUI_EventMsg m_EventMessageCtrl;
    public Battle_NGUI_EventMsg EventMessageCtrl
    {
        get { return m_EventMessageCtrl; }
    }

    [SerializeField]
    private Camera m_NGUICamera;
    public Camera NGUICamera
    {
        get { return m_NGUICamera; }
        set { m_NGUICamera = value; }
    }

    void Awake()
    {
        InitComponents();
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        MecroMethod.CheckExistComponent<BossHpBarCtrlManager>(m_BossHpManager);
        MecroMethod.CheckExistComponent<Battle_NGUI_EventMsg>(m_EventMessageCtrl);
        MecroMethod.CheckExistComponent<Camera>(m_NGUICamera);
    }

    void OnEnable()
    {
        Invoke("UpperPanelMoving", 0.05f); 
    }

    protected override void UpperPanelMoving()
    {
        base.UpperPanelMoving();
        //if BattleScene Start Reducing Statmina
        LobbyController.GetInstance().UpperStatusPanel.StartReducingStamina();
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
      
}
