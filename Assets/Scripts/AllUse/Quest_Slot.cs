using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest_Slot : MonoBehaviour {

    private static Dictionary<string, GameObject> m_LoadedQuestIcons =
        new Dictionary<string, GameObject>();

    public enum SLOTTYPE
    {
        SLOT_WAIT = 0,
        SLOT_ORDER,
    };

    private Quest_Interface_Comp m_ChildQuest;
    public Quest_Interface_Comp ChildQuest
    { get { return m_ChildQuest; } }

    private Quest_Slot.SLOTTYPE m_QuestSlotType;
    public Quest_Slot.SLOTTYPE QuestSlotType
    {
        get { return m_QuestSlotType; }
        set { m_QuestSlotType = value; }
    }

    public void SetChildQuestInfo(Quest_Interface _QuestInfo, 
        Quest_Slot.SLOTTYPE _QuestSlotType, UIGrid ParentGrid)
    {
        m_ChildQuest = new Quest_Interface_Comp(_QuestInfo);
        m_QuestSlotType = _QuestSlotType;

        //Image, Quest Info Setting
        AddIconImage();

        //transform Setting
        switch (m_QuestSlotType)
        {
            case SLOTTYPE.SLOT_WAIT:
                transform.localScale = Vector3.one;
                break;

            case SLOTTYPE.SLOT_ORDER:
                transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                break;
        }
        ParentGrid.AddChild(this.transform);
    }

    private void AddIconImage()
    {

    }
}
