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
        Debug.Log("QuestManager_OnEnable");
        GetCurrentAcceptQuestData();
        m_NPCChating("Hello? Welcome to our vilage!");
    }

    void Start()
    {
        InitBasicQuest();
        GridsRepositions();
    }

    private void GetCurrentAcceptQuestData()
    {
        Debug.Log(DataController.GetInstance().transform.parent);
        //AcceptQuestContainer에 있는 데이터를 불러온다.
        int AcceptQuestcount =
            AcceptQuestContainer.GetInstance().transform.childCount;

        Quest_Slot SelectedQuestTarget = null;
        for(int i = 0; i < AcceptQuestcount; ++i)
        {
            SelectedQuestTarget = 
                AcceptQuestContainer.GetInstance().GetChildQuestSlot(i);
            //SelectedQuestTarget.ChangeButtonTrans();
            m_OrderedQuestGrid.AddChild(SelectedQuestTarget.transform);
            SelectedQuestTarget.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            SelectedQuestTarget.ButtonWidget.ParentHasChanged();
            SelectedQuestTarget.gameObject.SetActive(true);
        }

        Debug.Log(DataController.GetInstance().transform.parent);
        //m_OrderedQuestGrid.Reposition();
    }

    private void InitBasicQuest()
    {
        Quest_Slot CreatedSlot = null;
        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "BlueSkeleton", FIELDID.ID_BATTLEFIELD01, 
                "It's Very Dangerous to hunt Blue Skeleton!", 
                0, Random.RandomRange(10, 20), 100),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if(CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Death", FIELDID.ID_BATTLEFIELD01,
                 "Legendery Monster Calling The 'Death'..",
                0, Random.RandomRange(3, 10), 2000),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanMaceSkeleton", FIELDID.ID_BATTLEFIELD01,
                 "Mace Skeleton is very powerful Attacker",
                0, Random.RandomRange(10, 20), 300),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "GermanSkeleton", FIELDID.ID_BATTLEFIELD01,
                 "it's too simple! Destroy it!",
                0, 18, 150),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "Mimic", FIELDID.ID_BATTLEFIELD01,
                 "did you hear the weird tresure box?",
                0, Random.RandomRange(10, 20), 500),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBomber", FIELDID.ID_BATTLEFIELD01,
                 "Do you want find some of Crazy Monster?",
                0, Random.RandomRange(10, 20), 300),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonBoomerang", FIELDID.ID_BATTLEFIELD01,
                 "Do you think what the... Boomerang is weak?",
                0, Random.RandomRange(10, 20), 100),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();

        CreatedSlot = Quest_Slot.CreateQuestSlot(
            Quest_Interface.CreateQuest(
                "SkeletonWarrior", FIELDID.ID_BATTLEFIELD01,
                 "many people says 'it calls unknown'",
                0, Random.RandomRange(10, 20), 1000),
            Quest_Slot.SLOTTYPE.SLOT_WAIT);
        if (CreatedSlot != null)
            CreatedSlot.ChangeButtonTrans();
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

    //선택된 퀘스트의 테두리를 추가하여 표시한다.
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

        //2. AcceptQuestContainer에 등록
        AcceptQuestContainer.GetInstance().AddchildQuestSlot(
            m_SelectedQuestSlot);

        //3. 실질적 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Add(
            m_SelectedQuestSlot.ChildQuest.QuestTarget,
            m_SelectedQuestSlot.ChildQuest);

        //4. 실질적 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //4. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Thank You! anything else?");
    }

    public void DiscardQuest()
    {
        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeQuestCondition();
        m_SelectedQuestSlot.ResetQuestCount();

        //2. AcceptQuestContainer에 갱신
        AcceptQuestContainer.GetInstance().RemoveChildQuestSlot(
            m_SelectedQuestSlot);

        //3. 실질적 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Remove(
            m_SelectedQuestSlot.ChildQuest.QuestTarget);

        //4. 실질적 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //5. NPC 멘트 장전
        m_NPCTalkingText.StartWriting("Well..");
    }

    public void QuestClear()
    {
        int QuestReward = m_SelectedQuestSlot.ChildQuest.QuestReward;
        //1. UI 퀘스트 갱신
        m_SelectedQuestSlot.ChangeQuestCondition();
        m_SelectedQuestSlot.ResetQuestCount();

        //2. AcceptQuestContainter에 갱신
        AcceptQuestContainer.GetInstance().RemoveChildQuestSlot(
            m_SelectedQuestSlot);

        //3. 퀘스트 데이터 갱신
        DataController.GetInstance().QuestGameData.AcceptQuest.Remove(
            m_SelectedQuestSlot.ChildQuest.QuestTarget);

        //4. 보상 추가
        DataController.GetInstance().InGameData.Money += QuestReward;

        //5. Money UI 최신화
        LobbyController.GetInstance(
            ).UpperStatusPanel.SetMoney(QuestReward);

        //6. 퀘스트 데이터 저장
        DataController.GetInstance().QuestSave();

        //7. 실질적 데이터 저장
        DataController.GetInstance().Save();

        //8. NPC 멘트
        m_NPCTalkingText.StartWriting("Good Job!\n Rewards : " +
            QuestReward.ToString() + " Gold!");
    }

    public void ResetQuestSelection()
    {
        //선택되어 있는 슬롯 해제.
        m_SelectedQuestSlot = null;

        //선택영역을 표시하는 오브젝트 숨기기.
        m_LoadedQuestSelectArea.gameObject.SetActive(false);
        m_LoadedQuestSelectArea.transform.SetParent(OwnTrans, false);

        if(m_NPCExtensionButtons.gameObject.activeSelf == true)
            m_NPCExtensionButtons.HideNPCExtensionFunc();

        GridsRepositions();
    }

    public void MoveAcceptQuestContainer()
    {
        //Accept 상태인 퀘스트 아이템을 
        //AcceptQuestContainter으로 전송
        AcceptQuestContainer.GetInstance(
            ).GetAllQuestSlotToChild(m_OrderedQuestGrid.transform);
        
    }

    public void GridsRepositions()
    {
        m_OrderedQuestGrid.Reposition();
        m_WaitQuestGrid.Reposition();
    }

    public void NPCTalking(string _Script)
    {
        m_NPCTalkingText.StartWriting(_Script);
    }
}
