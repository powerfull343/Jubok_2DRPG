using UnityEngine;
using System.Collections;
using Mecro;
using LobbyButtonFunc;

public class VilageScene_NGUI_Panel : Scene_Panel_Interface
{
    [SerializeField]
    private UIWidget m_BackGround;

    [SerializeField]
    private bool m_isShowDebugingWindow = false;

    void Awake()
    {
        InitComponents();
    }

    void OnEnable()
    {
        Invoke("UpperPanelMoving", 0.05f);
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        MecroMethod.CheckExistComponent<UIWidget>(m_BackGround);
        m_BackGround.SetDimensions((int)fScreenWidth, (int)fScreenHeight);
        MecroMethod.CheckExistObject<GameObject>(m_TempCollider);
    }

    public void ClickMenuXButton()
    {
        LobbyController.GetInstance().OpenedPanel = IDSUBPANEL.PANELID_NONE;
        Invoke("CloseBehindCollider", 1f);
    }

    public override void CloseBehindCollider()
    {
        if (LobbyController.GetInstance().OpenedPanel == IDSUBPANEL.PANELID_NONE)
        {
            SetBehindColliderDepth(10);
            m_TempCollider.SetActive(false);
        }
        else
            SetBehindColliderDepth(10);
    }
}
