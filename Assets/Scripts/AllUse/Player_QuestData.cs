using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Player_QuestData {

    private Dictionary<string, Quest_Interface> m_AcceptedQuest
        = new Dictionary<string, Quest_Interface>();
    public Dictionary<string, Quest_Interface> AcceptQuest
    {
        get { return m_AcceptedQuest; }
    }
}
