using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Quest_Interface {

    private string m_QuestTarget;
    public string QuestTarget
    { get { return m_QuestTarget; } }

    private string m_QuestTalking;
    public string QuestTalking
    { get { return m_QuestTalking; } }

    private int m_QuestTargetCount;
    public int QuestTargetCount
    { get { return m_QuestTargetCount; } }

    private int m_QuestTargetMaxCount;
    public int QuestTargetMaxCount
    { get { return m_QuestTargetMaxCount; } }

    public Quest_Interface(string _QuestTarget,
        string _QuestTalking,
        int _QuestCount, int _QuestMaxCount)
    {
        m_QuestTarget = _QuestTarget;
        m_QuestTalking = _QuestTalking;
        m_QuestTargetCount = _QuestCount;
        m_QuestTargetMaxCount = _QuestMaxCount;
    }

    public static Quest_Interface CreateQuest(string _QuestTarget,
        string _QuestTalking, int _QuestCount, int _QuestMaxCount)
    {
        Quest_Interface newQuest =
            new Quest_Interface(_QuestTarget, _QuestTalking, 
            _QuestCount, _QuestMaxCount);
        return newQuest;
    }

}
