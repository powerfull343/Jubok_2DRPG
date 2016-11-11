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

    [SerializeField]
    private AutoTyping m_NPCTalkingText;
    public AutoTyping NPCTalkingText
    { get { return m_NPCTalkingText; } }

    [SerializeField]
    private UIWidget m_QuestExtensionButtons;

    private static GameObject m_LoadedQuestInst;
    private static GameObject m_LoadedQuestSelectArea;
    
    private Transform OwnTrans;
    private Quest_Slot m_SelectedQuestSlot = null;

    void Awake()
    {
        CreateInstance();
        OwnTrans = MecroMethod.CheckGetComponent<Transform>(this.gameObject);
        MecroMethod.CheckExistComponent<UIGrid>(m_OrderedQuestGrid);
        MecroMethod.CheckExistComponent<UIGrid>(m_WaitQuestGrid);
        MecroMethod.CheckExistComponent<UIWidget>(m_QuestExtensionButtons);
        MecroMethod.CheckExistComponent<AutoTyping>(m_NPCTalkingText);
        m_QuestExtensionButtons.alpha = 0f;
        if (m_LoadedQuestInst == null)
        {
            m_LoadedQuestInst =
                Resources.Load<GameObject>("LobbyScene/QuestPart/QuestItem");
        }

        if (m_LoadedQuestSelectArea == null)
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
        Invoke("NPCSayHello", 0.5f);
        
    }

    private void GetCurrentAcceptQuestData()
    {
        if (DataController.GetInstance() == null)
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

    private void NPCSayHello()
    {
        m_NPCTalkingText.StartWriting("Hello? Welcome to our vilage!");
    }

    public void NPCSayMayIHelpYou()
    {
        m_NPCTalkingText.StartWriting("May I Help You?");
    }

    private void InitBasicQuest()
    {
        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "BlueSkeleton", "It's Very Dangerous to hunt Blue Skeleton!", 
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Death", "Legendery Monster Calling The 'Death'..",
                0, Random.RandomRange(3, 10)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanMaceSkeleton", "Mace Skeleton is very powerful Attacker",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanSkeleton", "it's too simple! Destroy it!",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Mimic", "did you hear the weird tresure box?",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBomber", "Do you want find some of Crazy Monster?",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBoomerang", "Do you think what the... Boomerang is weak?",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonWarrior", "mad, crazy, hell.. every calls many names",
                0, Random.RandomRange(10, 40)),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
    }

    private void CreateQuestSlot(Quest_Interface _LoadedQuest,
        Quest_Slot.SLOTTYPE _QuestType)
    {
        if (_QuestType == Quest_Slot.SLOTTYPE.SLOT_WAIT &&
             DataController.GetInstance().QuestGameData.AcceptQuest.ContainsKey(
            _LoadedQuest.QuestTarget))
            return;

        GameObject newQuestInst = Instantiate(m_LoadedQuestInst);
        Quest_Slot newQuestSlotComp =
            MecroMethod.CheckGetComponent<Quest_Slot>(newQuestInst);

        newQuestSlotComp.SetChildQuestInfo(
            _LoadedQuest, _QuestType);
    }

    public void ActiveExtenstionQuestButton(Quest_Slot _SlotTarget)
    {
        m_SelectedQuestSlot = _SlotTarget;
        ShowSelectedQuestArea(_SlotTarget);
        m_NPCTalkingText.StartWriting(_SlotTarget.ChildQuest.QuestTalking);

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

    public void AcceptQuest()
    {
        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeConditionQuest();

        //2. 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Add(
            m_SelectedQuestSlot.ChildQuest.QuestTarget,
            m_SelectedQuestSlot.ChildQuest);

        //3. 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //4. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Thank You! anything else?");

    }

    public void HideExtensionQuestButton()
    {
        m_SelectedQuestSlot = null;
        m_LoadedQuestSelectArea.gameObject.SetActive(false);
        m_LoadedQuestSelectArea.transform.SetParent(OwnTrans, false);
        TweenAlpha.Begin(m_QuestExtensionButtons.gameObject, 0.25f, 0f);
        Invoke("HideExtensionObject", 0.25f);
    }

    private void HideExtensionObject()
    {
        m_QuestExtensionButtons.gameObject.SetActive(false);
    }

    
}
