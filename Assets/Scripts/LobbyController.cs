using UnityEngine;
using System.Collections;
using LobbyButtonFunc;
using Mecro;

namespace LobbyManager
{
    public class LobbyController 
        : Singleton<LobbyController>
    {
        public FIELDID mCurrentSceneID = FIELDID.ID_VILAGE;
        [SerializeField]
        private bool isSlow = false;

        [SerializeField]
        private Transform LobbyPanel;
        [SerializeField]
        private Transform BattlePanel;
        [SerializeField]
        private Transform HidingPanel;

        //뒤에 배경을 막을 필요가 있을시에 직접적으로 생성한다.
        public GameObject m_InstanceCollider;

        //
        public GameObject[] SubPanels;

        private PlayerStatusController m_UpperStatusPanel;
        public PlayerStatusController UpperStatusPanel
        { 
            get { return m_UpperStatusPanel; }
            set { m_UpperStatusPanel = value; }
        }
        private GameObject m_MenuPanel;
        public GameObject MenuPanel
        { get { return m_MenuPanel; } }

        private IDSUBPANEL m_OpenedPanel = IDSUBPANEL.PANELID_NONE;
        public IDSUBPANEL OpenedPanel
        {
            get { return m_OpenedPanel; }
            set { m_OpenedPanel = value; }
        }

        void Awake()
        {
            Debug.Log("LobbyCtrl");

            Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);

            if (isSlow)
                Time.fixedDeltaTime = 0.5f;

            CreateInstance();
            
            if (LobbyPanel == null)
                Debug.Log(LobbyPanel.name + "is null");

            if (BattlePanel == null)
                Debug.Log(BattlePanel.name + "is null");


            foreach(GameObject obj in SubPanels)
            {
                if(obj == null)
                {
                    Debug.Log("One of Object is Null");
                    break;
                }
            }

            UpperStatusUILoading();
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

            //Upper Stat UI -> Menu Button Click Function
            m_MenuPanel = Instantiate(Resources.Load("UIPanels/Panel - FunctionMenu") as GameObject);
        }

        public void OpenMenuPanel(Transform Parent)
        {
            if (!m_MenuPanel)
            {
                m_MenuPanel =
                    Instantiate(Resources.Load("UIPanels/Panel - FunctionMenu") as GameObject);
            }

            m_MenuPanel.SetActive(false);

            m_MenuPanel.transform.parent = Parent;
            m_MenuPanel.transform.localPosition = Vector3.zero;
            m_MenuPanel.transform.localScale = Vector3.one;

            m_MenuPanel.SetActive(true);
        }

        public void SettingBlockPanel(int SettingPanelDepth)
        {
            if(m_InstanceCollider == null)
            {
                m_InstanceCollider = 
                    Instantiate(Resources.Load("LobbyScene/Panel - CannotControl") as GameObject
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

        //public void OpenCannotClickVilage()
        //{
        //    TempCollider.SetActive(true);
        //}

        public void CloseCannotClickVilage()
        {
            m_OpenedPanel = IDSUBPANEL.PANELID_NONE;
            Invoke("AfterAnimationClosing", 1f);
        }

        //void AfterAnimationClosing()
        //{
        //    TempCollider.SetActive(false);
        //}

        //public void ClickMenuXButton()
        //{
        //    if (OpenedPanel == IDSUBPANEL.PANELID_NONE)
        //        TempCollider.SetActive(false);
        //}

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
        public void ChangePanel()
        {
            StartCoroutine("LowerPanelAlpha");
        }

        public void HideAndShowUpperStatusPanel()
        {
            if(!UpperStatusPanel.gameObject.activeSelf)
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
            Transform HideTrans = BattleFieldManager.mSelectID != FIELDID.ID_VILAGE ?
                LobbyPanel : BattlePanel;
            Transform ShowTrans = BattleFieldManager.mSelectID != FIELDID.ID_VILAGE ?
                BattlePanel : LobbyPanel;

            if(!HidingPanel.gameObject.activeSelf)
                HidingPanel.gameObject.SetActive(true);

            UIPanel hidingwidget = MecroMethod.CheckGetComponent<UIPanel>(HidingPanel);

            while (hidingwidget.alpha < 1f)
            {
                hidingwidget.alpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            HideTrans.gameObject.SetActive(false);

            ShowTrans.gameObject.SetActive(true);
            while (hidingwidget.alpha > 0f)
            {
                hidingwidget.alpha -= 0.03f;
                yield return new WaitForSeconds(0.01f);
            }

            if (HidingPanel.gameObject.activeSelf)
                HidingPanel.gameObject.SetActive(false);

            yield break;
        }
    }
}