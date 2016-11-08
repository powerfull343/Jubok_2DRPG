using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;

public class Vilage_QuestManager : MonoBehaviour {

    [SerializeField]
    private UIGrid m_OrderedQuestGrid;

    [SerializeField]
    private UIGrid m_WaitQuestGrid;

    private static GameObject m_LoadedQuestInst;

    void Awake()
    {
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
                LoadedQuestData.AcceptQuest.ToList()[i].Value);
        }
    }

    private void CreateQuestSlot(Quest_Interface LoadedQuest)
    {
        GameObject newQuestInst = Instantiate(m_LoadedQuestInst);
        Quest_Slot newQuestSlotComp =
            MecroMethod.CheckGetComponent<Quest_Slot>(newQuestInst);

        newQuestSlotComp.SetChildQuestInfo(
            LoadedQuest, Quest_Slot.SLOTTYPE.SLOT_ORDER,
            m_OrderedQuestGrid);
    }

    void Start()
    {
        InitQuestManager();
    }

    private void InitQuestManager()
    {

    }
}
