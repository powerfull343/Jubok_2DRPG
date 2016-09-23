using UnityEngine;
using System.Collections;
using LobbyManager;

public class PlayerStatusController : MonoBehaviour
{
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

    private GameObject_Extension m_ObjectExtension;
    

    public float m_fUISize = 0f;

    void OnEnable()
    {
        EventDelegate EventDg = new EventDelegate(this, "CallGameMenu");
        m_CallMenuButton.onClick.Add(EventDg);
    }

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UIWidget>(m_BGWidget);
        m_BGWidget.SetDimensions((int)BattleScene_NGUI_Panel.fScreenWidth,
            (int)(m_BGWidget.localSize.y));

        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_HealthLabel);
        Mecro.MecroMethod.CheckExistComponent<UI_Money>(m_MoneyCtrl);
        Mecro.MecroMethod.CheckExistComponent<UI_UpperStat_Stamina>(m_StatminaCtrl);
        Mecro.MecroMethod.CheckExistComponent<UIButton>(m_CallMenuButton);
        m_ObjectExtension = Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);

        //EventDelegate EventDg = new EventDelegate(this, "CallGameMenu");
        //m_CallMenuButton.onClick.Add(EventDg);

        m_fUISize = m_BGWidget.localSize.y;
    }

    void CallGameMenu()
    {
        if (LobbyController.GetInstance().mCurrentID == FIELDID.ID_VILAGE)
            VilageScene_NGUI_Panel.GetInstance().OpenBehindCollider();

        LobbyController.GetInstance().OpenMenuPanel(
            VilageScene_NGUI_Panel.GetInstance().transform);
    }

    public void MovingUpperStatusUI(Transform SceneParent, float fScreenHeight)
    {
        this.gameObject.SetActive(false);
        this.transform.parent = SceneParent;

        //float fHeight = (Screen.height / 2f) - (m_BGWidget.localSize.y / 2f);
        float fHeight = (fScreenHeight / 2f) - (m_BGWidget.localSize.y / 2f);
        this.transform.localPosition = new Vector3(0f, fHeight, 0f);
        this.transform.localScale = Vector3.one;
        this.gameObject.SetActive(true);

        if(LobbyController.GetInstance().mCurrentID != FIELDID.ID_VILAGE &&
            LobbyController.GetInstance().mCurrentID != FIELDID.ID_CASTLE)
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

    public bool SetMoney(int nMoneySize)
    {
        if (!CompareMoney(nMoneySize))
            return false;

        m_MoneyCtrl.UpdateMoneySize(nMoneySize);
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
