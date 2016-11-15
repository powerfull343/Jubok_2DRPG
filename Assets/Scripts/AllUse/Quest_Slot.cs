using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mecro;

public class Quest_Slot : MonoBehaviour {

    private static Dictionary<string, GameObject> m_LoadedQuestIcons =
        new Dictionary<string, GameObject>();

    private EventDelegate m_QuestSlotFunc;

    public enum SLOTTYPE
    {
        SLOT_WAIT = 0,
        SLOT_ORDER,
    };

    private Quest_Interface m_ChildQuest;
    public Quest_Interface ChildQuest
    { get { return m_ChildQuest; } }

    private Quest_Slot.SLOTTYPE m_QuestSlotType;
    public Quest_Slot.SLOTTYPE QuestSlotType
    {
        get { return m_QuestSlotType; }
        set { m_QuestSlotType = value; }
    }
    public bool m_QuestNaviSetting = false;

    [SerializeField]
    private UILabel m_QuestExpression;
    public UILabel QuestExpression
    { get { return m_QuestExpression; } }

    private UIWidget m_ButtonWidget;
    public UIWidget ButtonWidget
    { get { return m_ButtonWidget; } }
    private UIButton m_ButtonComp;
    public UIButton ButtonComp { get { return m_ButtonComp;} }
    //private UIButtonScale m_ButtonScale;

    private static GameObject m_LoadedQuestSlot;

    void Awake()
    {
        MecroMethod.CheckExistComponent<UILabel>(m_QuestExpression);
        m_ButtonComp =
            MecroMethod.CheckGetComponent<UIButton>(this.gameObject);
        //m_ButtonScale =
        //    MecroMethod.CheckGetComponent<UIButtonScale>(this.gameObject);
        m_ButtonWidget =
            MecroMethod.CheckGetComponent<UIWidget>(this.gameObject);
        
        LoadingQuestIcon();
    }


    void OnEnable()
    { 
        if (m_QuestSlotFunc == null &&
            m_QuestNaviSetting == false &&
            Vilage_QuestManager.GetInstance() != null)
        {
            m_QuestSlotFunc = new EventDelegate(
               Vilage_QuestManager.GetInstance(), "ClickQuestButton");
            m_QuestSlotFunc.parameters[0] = MecroMethod.CreateEventParm(
                this, this.GetType());

            m_ButtonComp.onClick.Add(m_QuestSlotFunc);
        }
    }

    private void LoadingQuestIcon()
    {
        if (m_LoadedQuestIcons.Count > 0)
            return;

        string FilePath = "LobbyScene/QuestPart/QuestStillImage/QuestImage - ";

        m_LoadedQuestIcons.Add("BlueSkeleton",
            Resources.Load<GameObject>(FilePath + "BlueSkeleton"));
        m_LoadedQuestIcons.Add("Death",
            Resources.Load<GameObject>(FilePath + "Death"));
        m_LoadedQuestIcons.Add("GermanMaceSkeleton",
            Resources.Load<GameObject>(FilePath + "GermanMaceSkeleton"));
        m_LoadedQuestIcons.Add("GermanSkeleton",
            Resources.Load<GameObject>(FilePath + "GermanSkeleton"));
        m_LoadedQuestIcons.Add("Mimic",
            Resources.Load<GameObject>(FilePath + "Mimic"));
        m_LoadedQuestIcons.Add("SkeletonBomber",
            Resources.Load<GameObject>(FilePath + "SkeletonBomber"));
        m_LoadedQuestIcons.Add("SkeletonBoomerang",
            Resources.Load<GameObject>(FilePath + "SkeletonBoomerang"));
        m_LoadedQuestIcons.Add("SkeletonWarrior",
            Resources.Load<GameObject>(FilePath + "SkeletonWarrior"));
    }

    public static Quest_Slot CreateQuestSlot(Quest_Interface _LoadedQuest,
        Quest_Slot.SLOTTYPE _QuestType)
    {
        if (m_LoadedQuestSlot == null)
        {
            m_LoadedQuestSlot =
                Resources.Load<GameObject>("LobbyScene/QuestPart/QuestItem");
        }

        if (_QuestType == Quest_Slot.SLOTTYPE.SLOT_WAIT &&
             DataController.GetInstance().QuestGameData.AcceptQuest.ContainsKey(
            _LoadedQuest.QuestTarget))
            return null;

        GameObject newQuestInst = Instantiate(m_LoadedQuestSlot);
        Quest_Slot newQuestSlotComp =
            MecroMethod.CheckGetComponent<Quest_Slot>(newQuestInst);

        newQuestSlotComp.SetChildQuestInfo(
            _LoadedQuest, _QuestType);

        return newQuestSlotComp;
    }

    public void SetChildQuestInfo(Quest_Interface _QuestInfo, 
        Quest_Slot.SLOTTYPE _QuestSlotType)
    {
        //m_ChildQuest = new Quest_Interface_Comp(_QuestInfo);
        m_ChildQuest = _QuestInfo;
        m_QuestSlotType = _QuestSlotType;

        //Image, Quest Info Setting
        AddIconImage();
        WriteExpressionLabel();
    }

    private void AddIconImage()
    {
        GameObject IconImage =
            Instantiate(m_LoadedQuestIcons[m_ChildQuest.QuestTarget]);
        IconImage.transform.SetParent(transform);
        IconImage.transform.localPosition = new Vector3(-98f, 0f, 0f);
        IconImage.SetActive(true);
    }

    public void WriteExpressionLabel()
    {
        StringBuilder builder = new StringBuilder();
        if (m_ChildQuest.QuestTargetCount < m_ChildQuest.QuestTargetMaxCount)
        {
            builder.AppendLine(m_ChildQuest.QuestTarget);
            builder.Append(m_ChildQuest.QuestTargetCount);
            builder.Append(" / ");
            builder.Append(m_ChildQuest.QuestTargetMaxCount);
        }
        else
        {
            builder.AppendLine(m_ChildQuest.QuestTarget);
            builder.Append("[ffff00]");
            builder.Append(m_ChildQuest.QuestTargetCount);
            builder.Append(" / ");
            builder.Append(m_ChildQuest.QuestTargetMaxCount);
            builder.Append("[-]");
        }

        m_QuestExpression.text = builder.ToString();
    }

    public void ChangeButtonTrans()
    {
        switch (m_QuestSlotType)
        {
            case SLOTTYPE.SLOT_WAIT:
                Vilage_QuestManager.GetInstance(
                    ).WaitQuestGrid.AddChild(this.transform);
                transform.localScale = Vector3.one;
                m_ButtonWidget.ParentHasChanged();
                break;

            case SLOTTYPE.SLOT_ORDER:
                Vilage_QuestManager.GetInstance(
                    ).OrderedQuestGrid.AddChild(this.transform);
                transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                m_ButtonWidget.ParentHasChanged();
                break;
        }
    }

    public void ChangeQuestCondition()
    {
        switch(m_QuestSlotType)
        {
            case SLOTTYPE.SLOT_ORDER:
                gameObject.SetActive(false);
                m_QuestSlotType = SLOTTYPE.SLOT_WAIT;
                ChangeButtonTrans();
                gameObject.SetActive(true);
                break;

            case SLOTTYPE.SLOT_WAIT:
                gameObject.SetActive(false);
                m_QuestSlotType = SLOTTYPE.SLOT_ORDER;
                ChangeButtonTrans();
                gameObject.SetActive(true);
                break;
        }
    }

    public bool GetClearQuestResult()
    {
        if (ChildQuest.QuestTargetCount >= ChildQuest.QuestTargetMaxCount)
            return true;

        return false;
    }

}
