using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battle_QuestWindow : MonoBehaviour {

    public UIWidget m_QuestButtonWidget;
    [SerializeField]
    private Transform m_QuestShowPosition;
    private bool m_isQuestShow = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UIWidget>(m_QuestButtonWidget);
        Mecro.MecroMethod.CheckExistComponent<Transform>(m_QuestShowPosition);

        float fXPos = -(BattleScene_NGUI_Panel.fScreenWidth / 2f) +
            (m_QuestButtonWidget.localSize.x);
        float fYPos = BattleScene_NGUI_Panel.fScreenHeight / 2f -
            LobbyController.GetInstance().UpperStatusPanel.m_fUIYSize -
            m_QuestButtonWidget.localSize.y;

        m_QuestButtonWidget.transform.localPosition = new Vector3(fXPos, fYPos, 1f);
    }

    public void QuestWindowOpenAndHide()
    {
        
        if(!m_isQuestShow)
        {
            var LoadedQuestSlots = 
                AcceptQuestContainer.GetInstance().GetChildQuestSlots(
                LobbyController.GetInstance().mCurrentSceneID);

            Quest_Slot CalledQuestSlot = null;
            for(int i = 0; i < LoadedQuestSlots.ToList().Count; ++i)
            {
                CalledQuestSlot = LoadedQuestSlots.ToList()[i];
                CalledQuestSlot.m_QuestNaviSetting = true;
                CalledQuestSlot.transform.SetParent(m_QuestShowPosition, false);
                CalledQuestSlot.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                CalledQuestSlot.gameObject.SetActive(true);
            }
        }
        else
        {
            AcceptQuestContainer.GetInstance().GetAllQuestSlotToChild(
                m_QuestShowPosition);
        }
    }

}
