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

    void Awake()
    {
        CreateInstance();
        MecroMethod.CheckExistComponent<UIGrid>(m_OrderedQuestGrid);
        MecroMethod.CheckExistComponent<UIGrid>(m_WaitQuestGrid);
        if (m_LoadedQuestInst == null)
        {
            m_LoadedQuestInst = 
                Resources.Load<GameObject>("LobbyScene/QuestPart/QuestItem");
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
}
