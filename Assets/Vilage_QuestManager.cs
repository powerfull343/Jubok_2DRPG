using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;

public class Vilage_QuestManager : 
    Singleton<Vilage_QuestManager> {

    [SerializeField]
    private UIGrid m_OrderedQuestGrid;
    public UIGrid OrderedQuestGrid
    { get { return m_OrderedQuestGrid; } }

    [SerializeField]
    private UIGrid m_WaitQuestGrid;
    public UIGrid WaitQuestGrid
    { get { return m_WaitQuestGrid; } }

    private static GameObject m_LoadedQuestInst;
    private static GameObject m_LoadedQuestSelectArea;

    [SerializeField]
    private UIWidget m_QuestExtensionButtons;

    void Awake()
    {
        CreateInstance();
        MecroMethod.CheckExistComponent<UIGrid>(m_OrderedQuestGrid);
        MecroMethod.CheckExistComponent<UIGrid>(m_WaitQuestGrid);
        MecroMethod.CheckExistComponent<UIWidget>(m_QuestExtensionButtons);
        m_QuestExtensionButtons.alpha = 0f;
        if (m_LoadedQuestInst == null)
        {
            m_LoadedQuestInst = 
                Resources.Load<GameObject>("LobbyScene/QuestPart/QuestItem");
        }

        if(m_LoadedQuestSelectArea == null)
        {
            m_LoadedQuestSelectArea =
                Instantiate(Resources.Load<GameObject>(
                    "LobbyScene/QuestPart/Sprite - SelectedQuest"));
            m_LoadedQuestSelectArea.SetActive(false);
        }
    }

	void OnEnable()
    {
        GetCurrentAcceptQuestData();
    }

    private void GetCurrentAcceptQuestData()
    {
        if(DataController.GetInstance() == null)
            Debug.LogError("Cannot find Loaded Data");

        //수주받은 퀘스트 로딩.
        Player_QuestData LoadedQuestData =
            DataController.GetInstance().QuestGameData;
        int OrderedQuestCount =
            LoadedQuestData.AcceptQuest.ToList().Count;

        for (int i = 0; i < OrderedQuestCount; ++i)
        {
            if (m_OrderedQuestGrid.transform.childCount >= 4)
                break;

            CreateQuestSlot(
                LoadedQuestData.AcceptQuest.ToList()[i].Value,
                Quest_Slot.SLOTTYPE.SLOT_ORDER);
        }
    }

    void Start()
    {
        InitBasicQuest();
    }

    private void InitBasicQuest()
    {
        CreateQuestSlot(
            Quest_Interface.CreateQuest("BlueSkeleton", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("Death", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("GermanMaceSkeleton", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("GermanSkeleton", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("Mimic", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("SkeletonBomber", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("SkeletonBoomerang", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest("SkeletonWarrior", 0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
    }

    private void CreateQuestSlot(Quest_Interface _LoadedQuest,
        Quest_Slot.SLOTTYPE _QuestType)
    {
        GameObject newQuestInst = Instantiate(m_LoadedQuestInst);
        Quest_Slot newQuestSlotComp =
            MecroMethod.CheckGetComponent<Quest_Slot>(newQuestInst);

        newQuestSlotComp.SetChildQuestInfo(
            _LoadedQuest, _QuestType);
    }

    public void ActiveExtenstionQuestButton(Quest_Slot _SlotTarget)
    {
        Debug.Log("ClickButton");
        ShowSelectedQuestArea(_SlotTarget);

        m_QuestExtensionButtons.gameObject.SetActive(true);
        TweenAlpha.Begin(m_QuestExtensionButtons.gameObject, 0.25f, 1f);
    }

    private void ShowSelectedQuestArea(Quest_Slot _SlotTarget)
    {
        m_LoadedQuestSelectArea.SetActive(false);
        UISprite SquareSprite = m_LoadedQuestSelectArea.GetComponent<UISprite>();
        if (SquareSprite == null)
            Debug.Log("Cannot Find " + SquareSprite.name);
        SquareSprite.depth = 4;
        m_LoadedQuestSelectArea.transform.SetParent(_SlotTarget.transform, false);
        m_LoadedQuestSelectArea.SetActive(true);

    }

    public void HideExtensionQuestButton()
    {
        m_LoadedQuestSelectArea.gameObject.SetActive(false);
        m_LoadedQuestSelectArea.transform.SetParent(this.transform, false);
        TweenAlpha.Begin(m_QuestExtensionButtons.gameObject, 0.25f, 0f);
        Invoke("HideExtensionObject", 0.25f);
    }

    private void HideExtensionObject()
    {
        m_QuestExtensionButtons.gameObject.SetActive(false);
    }
}
