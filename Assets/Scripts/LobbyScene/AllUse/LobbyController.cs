using UnityEngine;
using System.Collections;
using LobbyButtonFunc;
using Mecro;

public class LobbyController
    : Singleton<LobbyController>
{
    public FIELDID mCurrentSceneID = FIELDID.ID_VILAGE;

    public static FIELDID mSelectedSceneID = FIELDID.ID_ERROR;
    [SerializeField]
    private bool isSlow = false;

    [SerializeField]
    private Transform LobbyPanel;
    public Transform GetLobbyPanel
    { get { return LobbyPanel; } }
    [SerializeField]
    private Transform BattlePanel;
    public Transform GetBattlePanel
    { get { return BattlePanel; } }
    [SerializeField]
    private Transform HidingPanel;

    //뒤에 배경을 막을 필요가 있을시에 직접적으로 생성한다.
    public GameObject m_InstanceCollider;

    public GameObject[] SubPanels;

    private PlayerStatusController m_UpperStatusPanel;
    public PlayerStatusController UpperStatusPanel
    {
        get { return m_UpperStatusPanel; }
        set { m_UpperStatusPanel = value; }
    }

    private IDSUBPANEL m_OpenedPanel = IDSUBPANEL.PANELID_NONE;
    public IDSUBPANEL OpenedPanel
    {
        get { return m_OpenedPanel; }
        set { m_OpenedPanel = value; }
    }

    void Awake()
    {
        Debug.Log("LobbyCtrl");
        CreateInstance();

        if (isSlow)
            Time.fixedDeltaTime = 0.5f;

        CheckingComponents();
        UpperStatusUILoading();
        SetVilageFunctions();
    }

    private void CheckingComponents()
    {
        if (LobbyPanel == null)
        {
            GameObject VilageSceneObject =
                Instantiate(Resources.Load(
                    "SceneObjects/VilageSceneContainer") as GameObject);
            LobbyPanel = VilageSceneObject.transform;
            if (mCurrentSceneID == FIELDID.ID_VILAGE)
                VilageSceneObject.SetActive(true);
        }

        if (BattlePanel == null)
        {
            GameObject BattleSceneObject =
                Instantiate(Resources.Load(
                    "SceneObjects/BattleSceneContainer") as GameObject);
            BattlePanel = BattleSceneObject.transform;
            if (mCurrentSceneID == FIELDID.ID_BATTLEFIELD01)
                BattleSceneObject.SetActive(true);
        }

        if (HidingPanel == null)
        {
            GameObject HidingPanelObject =
                Instantiate(Resources.Load(
                    "SceneObjects/Panel - HidingBG") as GameObject);
            HidingPanel = HidingPanelObject.transform;
            HidingPanelObject.SetActive(false);
        }

    }

    private void UpperStatusUILoading()
    {
        //Upper Stat UI;
        GameObject UpperStatUI =
            Instantiate(Resources.Load("UIPanels/Panel - UpperPlayerStatus") as GameObject);

        if (!UpperStatUI)
            Debug.LogError(m_UpperStatusPanel.name + " is null");
        else
        {
            //Panel Setting
            UpperStatusPanel =
                Mecro.MecroMethod.CheckGetComponent<PlayerStatusController>(UpperStatUI);
        }
    }

    private void SetVilageFunctions()
    {
        SubPanels[0] = LobbyPanel.transform.FindChild(
            "Panel - FunctionBlackSmith").gameObject;
        SubPanels[1] = LobbyPanel.transform.FindChild(
            "Panel - FunctionTownHall").gameObject;
        SubPanels[2] = LobbyPanel.transform.FindChild(
            "Panel - FunctionGroceryStore").gameObject;
        SubPanels[3] = LobbyPanel.transform.FindChild(
            "Panel - FunctionBattleField").gameObject;
        SubPanels[4] = LobbyPanel.transform.FindChild(
            "Panel - DetailPlayerStatus").gameObject;
        SubPanels[5] = UpperStatusPanel.transform.FindChild(
            "Panel - FunctionMenu").gameObject;
    }

    public void SettingBlockPanel(int SettingPanelDepth)
    {
        if (m_InstanceCollider == null)
        {
            m_InstanceCollider =
                Instantiate(Resources.Load("LobbyScene/LobbyScene - LobbyPart/Panel - CannotControl") as GameObject
                , Vector3.zero, Quaternion.identity) as GameObject;
        }

        MecroMethod.CheckGetComponent<UIPanel>(m_InstanceCollider).depth =
            SettingPanelDepth;
        m_InstanceCollider.SetActive(true);
    }

    public void HidingBlockPanel()
    {
        if (m_InstanceCollider.activeSelf)
        {
            m_InstanceCollider.SetActive(false);
        }
    }

    public void CloseCannotClickVilage()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_NONE;
        Invoke("AfterAnimationClosing", 1f);
    }

    public void OpenVilagePanel(IDSUBPANEL _PanelId)
    {
        Debug.Log(_PanelId.ToString());
        if(_PanelId != IDSUBPANEL.PANELID_OPTIONMENU)
            m_OpenedPanel = _PanelId;

        SubPanels[(int)_PanelId].SetActive(true);
    }

    public void OpenBlackSmithPanel()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_BLACKSMITH;
        SubPanels[(int)IDSUBPANEL.PANELID_BLACKSMITH].SetActive(true);
    }

    public void OpenTownHallPanel()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_TOWNHALL;
        SubPanels[(int)IDSUBPANEL.PANELID_TOWNHALL].SetActive(true);
    }

    public void OpenGroceryStorePanel()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_GROCERYSTORE;
        SubPanels[(int)IDSUBPANEL.PANELID_GROCERYSTORE].SetActive(true);
    }

    public void OpenBattleFieldPanel()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_BATTLEFIELD;
        SubPanels[(int)IDSUBPANEL.PANELID_BATTLEFIELD].SetActive(true);
    }

    public void OpenOptionMenuPanel()
    {
        SubPanels[(int)IDSUBPANEL.PANELID_OPTIONMENU].SetActive(true);
    }

    public void OpenPlayerStatusPanel()
    {
        m_OpenedPanel = IDSUBPANEL.PANELID_PLAYERSTAT;
        SubPanels[(int)IDSUBPANEL.PANELID_PLAYERSTAT].SetActive(true);
    }

    //=======Change Level=========//
    public void EntryAnotherField()
    {
        if (mCurrentSceneID == mSelectedSceneID)
            return;
        //Debug.Log(mSelectedSceneID);
        //Debug.Log(GetInstance().mCurrentSceneID);

        mCurrentSceneID = mSelectedSceneID;
    }

    public void ChangePanel()
    {
        StartCoroutine("LowerPanelAlpha");
    }

    public void HideAndShowUpperStatusPanel()
    {
        if (!UpperStatusPanel.gameObject.activeSelf)
            UpperStatusPanel.ActiveUpperStatus();
        else
            UpperStatusPanel.InActiveUpperStatus();

    }

    /// <summary>
    /// Scene 전환할때 이용된다.
    /// </summary>
    /// <returns></returns>
    IEnumerator LowerPanelAlpha()
    {
        Transform HideTrans = LobbyController.mSelectedSceneID == FIELDID.ID_VILAGE ?
            BattlePanel : LobbyPanel;
        Transform ShowTrans = LobbyController.mSelectedSceneID == FIELDID.ID_VILAGE ?
            LobbyPanel : BattlePanel;

        if (!HidingPanel.gameObject.activeSelf)
            HidingPanel.gameObject.SetActive(true);

        UIPanel hidingwidget = MecroMethod.CheckGetComponent<UIPanel>(HidingPanel);

        while (hidingwidget.alpha < 1f)
        {
            hidingwidget.alpha += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        HideTrans.gameObject.SetActive(false);

        ShowTrans.gameObject.SetActive(true);
        while (hidingwidget.alpha > 0f)
        {
            hidingwidget.alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        if (HidingPanel.gameObject.activeSelf)
            HidingPanel.gameObject.SetActive(false);

        yield break;
    }
}

