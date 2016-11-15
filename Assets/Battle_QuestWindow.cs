using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battle_QuestWindow : MonoBehaviour {

    public UIWidget m_QuestButtonWidget;
    [SerializeField]
    private UIGrid m_QuestGridPosition;
    private bool m_isQuestShow = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UIWidget>(m_QuestButtonWidget);
        Mecro.MecroMethod.CheckExistComponent<UIGrid>(m_QuestGridPosition);

        float fXPos = -(BattleScene_NGUI_Panel.fScreenWidth / 2f) +
            (m_QuestButtonWidget.localSize.x);
        float fYPos = BattleScene_NGUI_Panel.fScreenHeight / 2f -
            LobbyController.GetInstance().UpperStatusPanel.m_fUIYSize -
            m_QuestButtonWidget.localSize.y;

        m_QuestButtonWidget.transform.localPosition = new Vector3(fXPos, fYPos, 1f);
        m_QuestGridPosition.transform.position = m_QuestButtonWidget.transform.position;
        m_QuestGridPosition.transform.localPosition += new Vector3(0f, -65f, 0f);
            
    }

    public void QuestWindowOpenAndHide()
    {
        if(!m_isQuestShow)
        {
            m_isQuestShow = true;
            var LoadedQuestSlots = 
                AcceptQuestContainer.GetInstance().GetChildQuestSlots(
                LobbyController.GetInstance().mCurrentSceneID);

            Quest_Slot CalledQuestSlot = null;
            for(int i = 0; i < LoadedQuestSlots.ToList().Count; ++i)
            {
                CalledQuestSlot = LoadedQuestSlots.ToList()[i];
                CalledQuestSlot.m_QuestNaviSetting = true;
                m_QuestGridPosition.AddChild(CalledQuestSlot.transform);
                CalledQuestSlot.transform.localScale = 
                    new Vector3(0.8f, 0.8f, 1f);
                CalledQuestSlot.gameObject.SetActive(true);
            }
            m_QuestGridPosition.Reposition();
        }
        else
            ResetQuestGrid();
    }

    public void ResetQuestGrid()
    {
        m_isQuestShow = false;
        AcceptQuestContainer.GetInstance().GetAllQuestSlotToChild(
            m_QuestGridPosition.transform);
    }

}
