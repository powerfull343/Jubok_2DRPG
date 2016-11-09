﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mecro;

public class Quest_Slot : MonoBehaviour {

    private static Dictionary<string, GameObject> m_LoadedQuestIcons =
        new Dictionary<string, GameObject>();

    public enum SLOTTYPE
    {
        SLOT_WAIT = 0,
        SLOT_ORDER,
    };

    private Quest_Interface m_ChildQuest;
    public Quest_Interface ChildQuest
    { get { return m_ChildQuest; } }

    private Quest_Slot.SLOTTYPE m_QuestSlotType;
    public Quest_Slot.SLOTTYPE QuestSlotType
    {
        get { return m_QuestSlotType; }
        set { m_QuestSlotType = value; }
    }

    [SerializeField]
    private UILabel m_QuestExpression;
    public UILabel QuestExpression
    { get { return m_QuestExpression; } }

    private UIButtonScale m_ButtonScale;

    static Quest_Slot()
    {
        string FilePath = "LobbyScene/QuestPart/QuestStillImage/QuestImage - ";

        m_LoadedQuestIcons.Add("BlueSkeleton",
            Resources.Load<GameObject>(FilePath + "BlueSkeleton"));
        m_LoadedQuestIcons.Add("Death",
            Resources.Load<GameObject>(FilePath + "Death"));
        m_LoadedQuestIcons.Add("GermanMaceSkeleton",
            Resources.Load<GameObject>(FilePath + "GermanMaceSkeleton"));
        m_LoadedQuestIcons.Add("GermanSkeleton",
            Resources.Load<GameObject>(FilePath + "GermanSkeleton"));
        m_LoadedQuestIcons.Add("Mimic",
            Resources.Load<GameObject>(FilePath + "Mimic"));
        m_LoadedQuestIcons.Add("SkeletonBomber",
            Resources.Load<GameObject>(FilePath + "SkeletonBomber"));
        m_LoadedQuestIcons.Add("SkeletonBoomerang",
            Resources.Load<GameObject>(FilePath + "SkeletonBoomerang"));
        m_LoadedQuestIcons.Add("SkeletonWarrior",
            Resources.Load<GameObject>(FilePath + "SkeletonWarrior"));
    }

    void Awake()
    {
        MecroMethod.CheckExistComponent<UILabel>(m_QuestExpression);
        m_ButtonScale =
            MecroMethod.CheckGetComponent<UIButtonScale>(this.gameObject);
    }

    public void SetChildQuestInfo(Quest_Interface _QuestInfo, 
        Quest_Slot.SLOTTYPE _QuestSlotType)
    {
        //m_ChildQuest = new Quest_Interface_Comp(_QuestInfo);
        m_ChildQuest = _QuestInfo;
        m_QuestSlotType = _QuestSlotType;

        //Image, Quest Info Setting
        AddIconImage();
        WriteExpressionLabel();

        //transform Setting
        ChangeButtonTrans();
    }

    private void AddIconImage()
    {
        GameObject IconImage =
            Instantiate(m_LoadedQuestIcons[m_ChildQuest.QuestTarget]);
        IconImage.transform.SetParent(transform);
        IconImage.transform.localPosition = new Vector3(-98f, 0f, 0f);
        IconImage.SetActive(true);
    }

    private void WriteExpressionLabel()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(m_ChildQuest.QuestTarget);
        builder.Append(m_ChildQuest.QuestTargetCount);
        builder.Append(" / ");
        builder.Append(m_ChildQuest.QuestTargetMaxCount);

        m_QuestExpression.text = builder.ToString();
    }

    private void ChangeButtonTrans()
    {
        switch (m_QuestSlotType)
        {
            case SLOTTYPE.SLOT_WAIT:
                Vilage_QuestManager.GetInstance(
                    ).WaitQuestGrid.AddChild(this.transform);
                transform.localScale = Vector3.one;
                m_ButtonScale.hover = new Vector3(1.1f, 1.1f, 1f);
                m_ButtonScale.pressed = new Vector3(1.05f, 1.05f, 1f);
                break;

            case SLOTTYPE.SLOT_ORDER:
                Vilage_QuestManager.GetInstance(
                    ).OrderedQuestGrid.AddChild(this.transform);
                transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                m_ButtonScale.hover = new Vector3(0.9f, 0.9f, 1f);
                m_ButtonScale.pressed = new Vector3(0.85f, 0.85f, 1f);
                break;
        }
    }

    public void QuestSlotClick()
    {
        switch(m_QuestSlotType)
        {
            case SLOTTYPE.SLOT_ORDER:
                gameObject.SetActive(false);

                gameObject.SetActive(true);
                break;

            case SLOTTYPE.SLOT_WAIT:
                gameObject.SetActive(false);
                m_QuestSlotType = SLOTTYPE.SLOT_ORDER;
                ChangeButtonTrans();
                gameObject.SetActive(true);
                break;
        }
    }

}