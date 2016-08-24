using UnityEngine;
using System.Collections;
using Mecro;

public class NGUI_MenuCtrl : MonoBehaviour
{
    private UIPanel m_OwnPanel;

    [SerializeField]
    private UIButton m_CloseButton;
    [SerializeField]
    private UIButton m_AppQuitPopupButton;

    //Quit Popup Propertys
    [SerializeField]
    private GameObject m_AppQuitPopup;
    [SerializeField]
    private UIButton m_AppQuitYesButton;
    [SerializeField]
    private UIButton m_AppQuitNoButton;
    void Awake()
    {
        m_OwnPanel = MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);

        MecroMethod.CheckExistComponent<UIButton>(m_CloseButton);
        MecroMethod.CheckExistComponent<UIButton>(m_AppQuitPopupButton);
        MecroMethod.CheckExistObejct<GameObject>(m_AppQuitPopup);
        m_AppQuitPopup.SetActive(false);
    }

    void OnEnable()
    {
        //Menu Close Button Setting
        if (LobbyManager.LobbyController.GetInstance().mCurrentID ==
            FIELDID.ID_VILAGE)
        {
            EventDelegate EventDg = new EventDelegate(
                VilageScene_NGUI_Panel.GetInstance(), "CloseBehindCollider");
            m_CloseButton.onClick.Add(EventDg);
        }

        //App Quit Button Setting
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
        VilageScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
            m_OwnPanel.depth + 1);
        m_AppQuitPopup.SetActive(true);
    }

    private void HideQuitPopupBox()
    {
        VilageScene_NGUI_Panel.GetInstance().SetBehindColliderDepth(
            m_OwnPanel.depth - 1);
        m_AppQuitPopup.SetActive(false);
    }
}
