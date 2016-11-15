using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AcceptQuestContainer 
    : Singleton<AcceptQuestContainer> {

    private Transform m_OwnTrans;
    private Dictionary<string, Quest_Slot> m_ChildAcceptQuestSlot =
        new Dictionary<string, Quest_Slot>();

    public int m_MaxAcceptQuestCount = 3;

    void Awake()
    {
        CreateInstance();
        m_OwnTrans = Mecro.MecroMethod.CheckGetComponent<Transform>(this.gameObject);
        LoadAcceptQuest();
    }

    private void LoadAcceptQuest()
    {
        if (DataController.GetInstance() == null)
            Debug.LogError("Cannot find Loaded Data");

        //수주받은 퀘스트 로딩.
        Player_QuestData LoadedQuestData =
            DataController.GetInstance().QuestGameData;
        int OrderedQuestCount =
            LoadedQuestData.AcceptQuest.ToList().Count;

        Quest_Slot CreatedSlot = null;
        for (int i = 0; i < OrderedQuestCount; ++i)
        {
            if (m_OwnTrans.childCount >= m_MaxAcceptQuestCount)
                break;

            CreatedSlot = Quest_Slot.CreateQuestSlot(
                LoadedQuestData.AcceptQuest.ToList()[i].Value,
                Quest_Slot.SLOTTYPE.SLOT_ORDER);
            CreatedSlot.transform.SetParent(m_OwnTrans, false);
            CreatedSlot.gameObject.SetActive(false);
            m_ChildAcceptQuestSlot.Add(
                CreatedSlot.ChildQuest.QuestTarget,
                CreatedSlot);
        }
    }

    public IEnumerable<Quest_Slot> GetChildQuestSlots(FIELDID _FieldId)
    {
        return from Quest in m_ChildAcceptQuestSlot.Values
               where Quest.ChildQuest.MonsterRegenPos.Equals(_FieldId)
               select Quest;
    }

    public Quest_Slot GetChildQuestSlot(int index)
    {
        return m_ChildAcceptQuestSlot.ToList()[index].Value;
    }

    public void AddchildQuestSlot(Quest_Slot AddSlot)
    {
        if (m_ChildAcceptQuestSlot.Count >= m_MaxAcceptQuestCount)
            return;

        m_ChildAcceptQuestSlot.Add(AddSlot.ChildQuest.QuestTarget
            , AddSlot);
    }

    public void RemoveChildQuestSlot(Quest_Slot RemoveSlot)
    {
        if (m_ChildAcceptQuestSlot.Count <= 0)
            return;

        m_ChildAcceptQuestSlot.Remove(
            RemoveSlot.ChildQuest.QuestTarget);
    }

    public void GetAllQuestSlotToChild(Transform ParentTrans)
    {
        Quest_Slot MovingTarget = null;
        int AccpetQuestCount = ParentTrans.childCount;
        for (int i = 0; i < AccpetQuestCount; ++i)
        {
            MovingTarget =
                ParentTrans.GetChild(0).GetComponent<Quest_Slot>();
            MovingTarget.m_QuestNaviSetting = false;
            MovingTarget.ButtonComp.enabled = true;
            MovingTarget.gameObject.SetActive(false);
            MovingTarget.transform.SetParent(m_OwnTrans);
        }
    }

    public void UpdateQuestCount(string _QuestTarget)
    {
        if (!m_ChildAcceptQuestSlot.ContainsKey(_QuestTarget))
            return;

        Quest_Slot SelectedTarget = m_ChildAcceptQuestSlot[_QuestTarget];
        if (SelectedTarget.ChildQuest.QuestTargetCount >=
            SelectedTarget.ChildQuest.QuestTargetMaxCount)
            return;

        Debug.Log("Up");
        ++SelectedTarget.ChildQuest.QuestTargetCount;
        SelectedTarget.WriteExpressionLabel();
        DataController.GetInstance().QuestSave();

    }
	
}
