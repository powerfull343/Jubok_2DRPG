using UnityEngine;
using System.Collections;

public class Quest_Interface {

    private string m_QuestTarget;
    public string QuestTarget
    { get { return m_QuestTarget; } }

    private int m_QuestTargetCount;
    public int QuestTargetCount
    { get { return m_QuestTargetCount; } }

    private int m_QuestTargetMaxCount;
    public int QuestTargetMaxCount
    { get { return m_QuestTargetMaxCount; } }

    public Quest_Interface(string _QuestTarget,
        int _QuestCount, int _QuestMaxCount)
    {
        m_QuestTarget = _QuestTarget;
        m_QuestTargetCount = _QuestCount;
        m_QuestTargetMaxCount = _QuestMaxCount;
    }

    public static Quest_Interface CreateQuest(string _QuestTarget,
        int _QuestCount, int _QuestMaxCount)
    {
        Quest_Interface newQuest =
            new Quest_Interface(_QuestTarget, _QuestCount, _QuestMaxCount);
        return newQuest;
    }

}
