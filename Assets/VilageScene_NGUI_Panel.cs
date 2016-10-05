using UnityEngine;
using System.Collections;
using Mecro;
using LobbyManager;
using LobbyButtonFunc;

public class VilageScene_NGUI_Panel : 
    Singleton<VilageScene_NGUI_Panel>
{
    public static float fScreenWidth = 1280;
    public static float fScreenHeight = 720;

    [SerializeField]
    private UIWidget m_BackGround;
    [SerializeField]
    private GameObject m_TempCollider;
    private UIPanel m_TempColliderPanelComp;

    [SerializeField]
    private bool m_isShowDebugingWindow = false;

    void Awake()
    {
        CreateInstance();
        Mecro.MecroMethod.ShowSceneLogConsole("VilageScene_Awake", true);
        if(Application.loadedLevel == 1)
        {
            if (Screen.fullScreen == true)
            {
                Mecro.MecroMethod.ShowSceneLogConsole("VilageScene_FullScreen", true);
                Screen.fullScreen = false;
            }
        }
    }

    void OnEnable()
    {
        MecroMethod.CheckExistComponent<UIWidget>(m_BackGround);
        m_BackGround.SetDimensions(1280, 720);
        MecroMethod.CheckExistObject<GameObject>(m_TempCollider);

        Invoke("UpperPanelMoving", 0.05f);
    }

    private void UpperPanelMoving()
    {
        LobbyManager.LobbyController.GetInstance(
            ).UpperStatusPanel.MovingUpperStatusUI(
            this.transform, fScreenHeight);
    }

    public void OpenBehindCollider()
    {
        m_TempCollider.SetActive(true);
    }

    public void CloseBehindCollider()
    {
        m_TempCollider.SetActive(false);
    }

    public void SetBehindColliderDepth(int ChangeDepthAmount)
    {
        if (!m_TempColliderPanelComp)
        {
            m_TempColliderPanelComp = 
                MecroMethod.CheckGetComponent<UIPanel>(m_TempCollider);
        }

        m_TempColliderPanelComp.depth = ChangeDepthAmount;
        m_TempColliderPanelComp.Update();
    }

    public void ClickMenuXButton()
    {
        LobbyController.GetInstance().OpenedPanel = IDSUBPANEL.PANELID_NONE;
        Invoke("CloseBehindCollider", 1f);
    }
}
