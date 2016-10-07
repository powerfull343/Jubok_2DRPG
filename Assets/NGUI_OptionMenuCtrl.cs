using UnityEngine;
using System.Collections;
using Mecro;
using LobbyManager;
using LobbyButtonFunc;

public class NGUI_OptionMenuCtrl : MonoBehaviour
{
    private bool m_InitReady = false;
    private UIPanel m_OwnPanel;
    private SubPanelController m_PanelCtrler;

    [SerializeField]
    private UIButton m_CloseButton;
    [SerializeField]
    private UIButton m_AppQuitPopupButton;

    //Quit Popup Propertys
    [SerializeField]
    private UIPanel m_AppQuitPopupPanel;
    [SerializeField]
    private UIButton m_AppQuitYesButton;
    [SerializeField]
    private UIButton m_AppQuitNoButton;

    public bool OpenOptionMenuPanel(UIPanel _ParentPanel)
    {
        if (this.gameObject == null)
        {
            Debug.LogError("Cannot Created MenuPanel");
            return false;
        }
        if (this.gameObject.activeSelf)
            return false;

        this.gameObject.SetActive(false);

        if (!m_InitReady)
        {
            InitMenuFunctions();
            m_InitReady = true;
        }

        if (LobbyController.GetInstance().mCurrentSceneID == FIELDID.ID_VILAGE)
        {
            VilageScene_NGUI_Panel.GetInstance().OpenBehindCollider();
            VilageScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(_ParentPanel.depth - 1);
            Mecro.MecroMethod.SetPartent(this.transform,
                VilageScene_NGUI_Panel.GetInstance().transform);
        }
        else
        {
            BattleScene_NGUI_Panel.GetInstance().OpenBehindCollider();
            BattleScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(_ParentPanel.depth - 1);
            Mecro.MecroMethod.SetPartent(this.transform,
                BattleScene_NGUI_Panel.GetInstance().transform);
        }

        m_OwnPanel = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);

        m_OwnPanel.depth = _ParentPanel.depth + 1;
        this.gameObject.SetActive(true);

        return true;
    }

    public bool CloseOptionMenuPanel()
    {
        //if Opened MenuPanel SetActive = false;
        if (this.gameObject.activeSelf)
        {
            m_PanelCtrler.disappearWithAnimation();

            if (LobbyController.GetInstance().mCurrentSceneID == FIELDID.ID_VILAGE)
                VilageScene_NGUI_Panel.GetInstance().CloseBehindCollider();
            else
                BattleScene_NGUI_Panel.GetInstance().CloseBehindCollider();

            return true;
        }
        return false;
    }

    private void InitMenuFunctions()
    {
        if (!m_OwnPanel)
        {
            m_OwnPanel = MecroMethod.CheckGetComponent<
                UIPanel>(this.gameObject);
        }

        if (!m_PanelCtrler)
        {
            m_PanelCtrler = MecroMethod.CheckGetComponent<
                SubPanelController>(this.gameObject);
        }

        MecroMethod.CheckExistComponent<UIButton>(m_CloseButton);
        MecroMethod.CheckExistComponent<UIButton>(m_AppQuitPopupButton);
        MecroMethod.CheckExistComponent<UIPanel>(m_AppQuitPopupPanel);
        m_AppQuitPopupPanel.gameObject.SetActive(false);
        AddButtonDelegateFunc();
    }

    private void AddButtonDelegateFunc()
    {
        //App Quit Button Setting
        if (LobbyManager.LobbyController.GetInstance().mCurrentSceneID ==
            FIELDID.ID_VILAGE)
        {
            EventDelegate EventDg = new EventDelegate(
                VilageScene_NGUI_Panel.GetInstance(), "CloseBehindCollider");
            m_CloseButton.onClick.Add(EventDg);
        }
        else
        {
            EventDelegate EventDg = new EventDelegate(
                BattleScene_NGUI_Panel.GetInstance(), "CloseBehindCollider");
            m_CloseButton.onClick.Add(EventDg);
        }
        EventDelegate QuitPopupDg = new EventDelegate(
            this, "ShowQuitPopupBox");
        m_AppQuitPopupButton.onClick.Add(QuitPopupDg);

        EventDelegate AppQuitButton = new EventDelegate(
            OptionControll.GetInstance(), "ApplicationQuit");
        m_AppQuitYesButton.onClick.Add(AppQuitButton);

        EventDelegate AppQuitCancelButton = new EventDelegate(
            this, "HideQuitPopupBox");
        m_AppQuitNoButton.onClick.Add(AppQuitCancelButton);
    }

    private void ShowQuitPopupBox()
    {
        if (LobbyController.GetInstance().mCurrentSceneID == FIELDID.ID_VILAGE)
        {
            VilageScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
                m_OwnPanel.depth + 1);
        }
        else
        {
            BattleScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
                m_OwnPanel.depth + 1);
        }

        m_AppQuitPopupPanel.depth = m_OwnPanel.depth + 2;
        m_AppQuitPopupPanel.gameObject.SetActive(true);
    }

    private void HideQuitPopupBox()
    {
        if (LobbyController.GetInstance().mCurrentSceneID == FIELDID.ID_VILAGE)
        {
            VilageScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
                m_OwnPanel.depth - 1);
        }
        else
        {
            BattleScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
                m_OwnPanel.depth - 1);
        }

        m_AppQuitPopupPanel.gameObject.SetActive(false);
    }
}
