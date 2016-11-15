using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;

public class Vilage_BattleField_QuestNavi : MonoBehaviour {

    [SerializeField]
    private UIGrid m_QuestNaviGrid;
    [SerializeField]
    private UILabel m_NaviNotice;

    void Awake()
    {
        MecroMethod.CheckExistComponent<UIGrid>(m_QuestNaviGrid);
        MecroMethod.CheckExistComponent<UILabel>(m_NaviNotice);
    }

    public void QuestNaviGridUpdate()
    {
        //AcceptQuestContainter의 child에서 
        //찾고난 뒤에 퀘스트 슬롯을 배치한다.
        var SelectedQuests = AcceptQuestContainer.GetInstance(
            ).GetChildQuestSlots(LobbyController.mSelectedSceneID);

        Quest_Slot CurrentSlot = null;
        for(int i = 0; i < SelectedQuests.ToList().Count; ++i)
        {
            CurrentSlot = SelectedQuests.ToList()[i];
            m_QuestNaviGrid.AddChild(CurrentSlot.transform);
            CurrentSlot.ButtonWidget.ParentHasChanged();
            CurrentSlot.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            CurrentSlot.m_QuestNaviSetting = true;
            CurrentSlot.ButtonComp.enabled = false;
            CurrentSlot.gameObject.SetActive(true);
        }
    }

    public void QuestNaviGridReset()
    {
        //다른 선택지로 넘어갈 때에 퀘스트 슬롯을
        //해당 AcceptQuestContainter로 배치한다.
        AcceptQuestContainer.GetInstance(
            ).GetAllQuestSlotToChild(m_QuestNaviGrid.transform);
        
    }


    
}
