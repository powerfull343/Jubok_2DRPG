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

    void Awake()
    {
        CreateInstance();
        MecroMethod.CheckExistComponent<BossHpBarCtrlManager>(m_BossHpManager);
        MecroMethod.CheckExistComponent<Battle_NGUI_EventMsg>(m_EventMessageCtrl);
        MecroMethod.CheckExistComponent<Camera>(m_NGUICamera);
    }

    void OnEnable()
    {
        Invoke("UpperPanelMoving", 0.05f); 
    }

    private void UpperPanelMoving()
    {
        LobbyController.GetInstance().UpperStatusPanel.MovingUpperStatusUI(
            this.transform, fScreenHeight);
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
