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
    private Vilage_NPCFunction m_NPCExtensionButtons;

    private static GameObject m_LoadedQuestInst;
    private static GameObject m_LoadedQuestSelectArea;
    
    private Transform OwnTrans;
    private Quest_Slot m_SelectedQuestSlot = null;

    private System.Action<string> m_NPCChating;

    void Awake()
    {
        CreateInstance();
        OwnTrans = MecroMethod.CheckGetComponent<Transform>(this.gameObject);
        MecroMethod.CheckExistComponent<UIGrid>(m_OrderedQuestGrid);
        MecroMethod.CheckExistComponent<UIGrid>(m_WaitQuestGrid);
        MecroMethod.CheckExistComponent<Vilage_NPCFunction>(m_NPCExtensionButtons);
        MecroMethod.CheckExistComponent<AutoTyping>(m_NPCTalkingText);

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

        m_NPCChating = NPCTalking;
    }

    void OnEnable()
    {
        m_NPCChating("Hello? Welcome to our vilage!");
    }

    void Start()
    {
        GetCurrentAcceptQuestData();
        InitBasicQuest();
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

    private void InitBasicQuest()
    {
        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "BlueSkeleton", "It's Very Dangerous to hunt Blue Skeleton!", 
                0, Random.RandomRange(10, 40), 100),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Death", "Legendery Monster Calling The 'Death'..",
                0, Random.RandomRange(3, 10), 2000),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanMaceSkeleton", "Mace Skeleton is very powerful Attacker",
                0, Random.RandomRange(10, 40), 300),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanSkeleton", "it's too simple! Destroy it!",
                0, Random.RandomRange(10, 40), 150),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Mimic", "did you hear the weird tresure box?",
                0, Random.RandomRange(10, 40), 500),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBomber", "Do you want find some of Crazy Monster?",
                0, Random.RandomRange(10, 40), 300),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBoomerang", "Do you think what the... Boomerang is weak?",
                0, Random.RandomRange(10, 40), 100),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);

        CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonWarrior", "mad, crazy, hell.. every calls many names",
                0, Random.RandomRange(10, 40), 1000),
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

    public void ClickQuestButton(Quest_Slot _SlotTarget)
    {
        m_SelectedQuestSlot = _SlotTarget;
        ShowSelectedQuestArea(_SlotTarget);
        m_NPCTalkingText.StartWriting(_SlotTarget.ChildQuest.QuestTalking);

        //Quest 종류에 따라 달라진다.
        m_NPCExtensionButtons.ShowNPCExtensionFunc(_SlotTarget);
        //m_QuestExtensionButtons.gameObject.SetActive(true);
        //TweenAlpha.Begin(m_QuestExtensionButtons.gameObject, 0.25f, 1f);
    }

    //선택된 퀘스트 표시한다.
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
        //0. 수주받은 퀘스트 수를 검사한다.
        Debug.Log(DataController.GetInstance(
            ).QuestGameData.AcceptQuest.Count);

        if (DataController.GetInstance(
            ).QuestGameData.AcceptQuest.Count >= 3)
        {
            m_NPCTalkingText.StartWriting(
                "it's too much! you cannot accept more Quest..");
            return;
        }

        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeQuestCondition();

        //2. 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Add(
            m_SelectedQuestSlot.ChildQuest.QuestTarget,
            m_SelectedQuestSlot.ChildQuest);

        //3. 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //4. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Thank You! anything else?");
    }

    public void DiscardQuest()
    {
        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeQuestCondition();

        //2. 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Remove(
            m_SelectedQuestSlot.ChildQuest.QuestTarget);

        //3. 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //4. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Well..");
    }

    public void QuestClear()
    {
        int QuestReward = m_SelectedQuestSlot.ChildQuest.QuestReward;
        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeQuestCondition();

        //2. 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Remove(
            m_SelectedQuestSlot.ChildQuest.QuestTarget);

        //3. 보상 추가
        DataController.GetInstance().InGameData.Money += QuestReward;

        //4. Money UI 최신화
        LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(QuestReward);

        //5. 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //6. 실질적 데이터 저장
        DataController.GetInstance().Save();

        //7. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Good Job! i'll give " + QuestReward.ToString() + "Gold!");
    }

    public void ResetQuestSelection()
    {
        //선택되어 있는 슬롯 해제.
        m_SelectedQuestSlot = null;

        //선택영역을 표시하는 오브젝트 숨기기.
        m_LoadedQuestSelectArea.gameObject.SetActive(false);
        m_LoadedQuestSelectArea.transform.SetParent(OwnTrans, false);

        m_NPCExtensionButtons.HideNPCExtensionFunc();
        GridsRepositions();
    }

    private void GridsRepositions()
    {
        m_OrderedQuestGrid.Reposition();
        m_WaitQuestGrid.Reposition();
    }

    public void NPCTalking(string _Script)
    {
        m_NPCTalkingText.StartWriting(_Script);
    }
}
