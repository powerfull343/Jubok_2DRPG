using UnityEngine;
using System.Collections;


public class PlayerStatusController : MonoBehaviour
{
    private UIPanel m_OwnPanel;
    [SerializeField]
    private UIWidget m_BGWidget;
    [SerializeField]
    private UILabel m_HealthLabel;
    [SerializeField]
    private UI_Money m_MoneyCtrl;
    [SerializeField]
    private UI_UpperStat_Stamina m_StatminaCtrl;
    [SerializeField]
    private UIButton m_CallMenuButton;

    //Menu Fuctions
    [SerializeField]
    private NGUI_OptionMenuCtrl m_MenuPanel;
    public NGUI_OptionMenuCtrl MenuPanel
    { get { return m_MenuPanel; } }

    private GameObject_Extension m_ObjectExtension;

    public float m_fUISize = 0f;

    void Awake()
    {
        InitComponents();
        EventDelegate EventDg = new EventDelegate(LobbyController.GetInstance().UpperStatusPanel,
            "ShowAndHideOptionMenu");
        m_CallMenuButton.onClick.Add(EventDg);
    }

    void InitComponents()
    {
        Mecro.MecroMethod.CheckExistComponent<UIWidget>(m_BGWidget);
        m_BGWidget.SetDimensions((int)BattleScene_NGUI_Panel.fScreenWidth,
            (int)(m_BGWidget.localSize.y));

        m_OwnPanel = Mecro.MecroMethod.CheckGetComponent<UIPanel>(this.gameObject);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_HealthLabel);
        Mecro.MecroMethod.CheckExistComponent<UI_Money>(m_MoneyCtrl);
        Mecro.MecroMethod.CheckExistComponent<UI_UpperStat_Stamina>(m_StatminaCtrl);
        Mecro.MecroMethod.CheckExistComponent<UIButton>(m_CallMenuButton);
        m_ObjectExtension = Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);

        //EventDelegate EventDg = new EventDelegate(this, "CallGameMenu");
        //m_CallMenuButton.onClick.Add(EventDg);

        m_fUISize = m_BGWidget.localSize.y;

        //Upper Stat UI -> Menu Button Click Function
        Mecro.MecroMethod.CheckExistComponent<NGUI_OptionMenuCtrl>(m_MenuPanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowAndHideOptionMenu();
    }

    void ShowAndHideOptionMenu()
    {
        if (!m_MenuPanel.OpenOptionMenuPanel(m_OwnPanel))
            m_MenuPanel.CloseOptionMenuPanel();
    }

    public void MovingUpperStatusUI(Transform SceneParent, float fScreenHeight)
    {
        this.gameObject.SetActive(false);
        this.transform.parent = SceneParent;

        float fHeight = (fScreenHeight / 2f) - (m_BGWidget.localSize.y / 2f);
        this.transform.localPosition = new Vector3(0f, fHeight, 0f);
        this.transform.localScale = Vector3.one;
        this.gameObject.SetActive(true);
    }

    public void StartReducingStamina()
    {
        if (LobbyController.GetInstance().mCurrentSceneID != FIELDID.ID_VILAGE &&
            LobbyController.GetInstance().mCurrentSceneID != FIELDID.ID_CASTLE)
            m_StatminaCtrl.ReducingStatmina();
    }

    public bool CompareMoney(int nMoneySize)
    {
        int MoneyResult =
            DataController.GetInstance().InGameData.Money + nMoneySize;

        if (MoneyResult < 0)
        {
            Debug.Log("Money not Enough");
            return false;
        }

        return true;
    }

    public bool CompareStamina(int nStamina)
    {
        if (DataController.GetInstance().InGameData.Stamina < nStamina)
            return false;

        return true;
    }
    /// <summary>
    /// nUpdateMoneySize = 증감될 금액을 적는다. 
    /// </summary>
    /// <param name="nUpdateMoneySize"></param>
    /// <returns></returns>
    public bool SetMoney(int nUpdateMoneySize)
    {
        if (!CompareMoney(nUpdateMoneySize))
            return false;

        Debug.Log("nUpdateMoneySize : " + nUpdateMoneySize);

        m_MoneyCtrl.UpdateMoneySize(nUpdateMoneySize);
        return true;
    }

    public void ActiveUpperStatus()
    {
        if(m_ObjectExtension != null)
            m_ObjectExtension.SelfActive();
        else
            Debug.LogError("Cannot Find " + this.name + "/" + m_ObjectExtension.name);
    }

    public void InActiveUpperStatus()
    {
        if (m_ObjectExtension != null)
            m_ObjectExtension.SelfHide();
        else
            Debug.LogError("Cannot Find " + this.name + "/" + m_ObjectExtension.name);
    }
}
