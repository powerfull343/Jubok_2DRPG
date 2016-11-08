using UnityEngine;
using System.Collections;

public class Quest_Interface_Comp : MonoBehaviour {

    private Quest_Interface m_QuestInfo;
    public Quest_Interface QuestInfo
    {
        get { return m_QuestInfo; }
        set { m_QuestInfo = value; }
    }

    public Quest_Interface_Comp(Quest_Interface CopyQuestInfo)
    {
        this.m_QuestInfo = CopyQuestInfo;
    }

    public Quest_Interface_Comp(Quest_Interface_Comp Origin)
    {
        this.m_QuestInfo = Origin.QuestInfo;
    }
}
