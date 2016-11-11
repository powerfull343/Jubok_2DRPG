using UnityEngine;
using System.Collections;
using Mecro;

public class Vilage_NPCFunction : MonoBehaviour {

    public enum NPC_OKBUTTON_STAT
    {
        STAT_ACCEPT,
        STAT_DISCARD,
        STAT_COMPLETE,
    };

    private UIWidget m_OwnWidget;
    private GameObject_Extension m_OwnExtension;

    private NPC_OKBUTTON_STAT m_OkStat;
    [SerializeField]
    private UILabel m_OkButtonText;
    [SerializeField]
    private UIButton m_OkButtonFunc;
    [SerializeField]
    private BoxCollider[] m_ButtonColliders;

	void Awake()
    {
        m_OwnWidget = 
            MecroMethod.CheckGetComponent<UIWidget>(gameObject);
        m_OwnWidget.alpha = 0f;
        m_OwnExtension =
            MecroMethod.CheckGetComponent<GameObject_Extension>(gameObject);

        MecroMethod.CheckExistComponent<UILabel>(m_OkButtonText);
        MecroMethod.CheckExistComponent<UIButton>(m_OkButtonFunc);
        if (m_ButtonColliders.Length <= 0)
            Debug.LogError("Cannot Find Button Colliders");
    }

    public void ShowNPCExtensionFunc(Quest_Slot _QuestSlot)
    {
        OkButtonSetting(_QuestSlot);
        foreach (BoxCollider boxcol in m_ButtonColliders)
            boxcol.enabled = true;

        gameObject.SetActive(true);
        TweenAlpha.Begin(gameObject, 0.25f, 1f);
    }

    private void OkButtonSetting(Quest_Slot _QuestSlot)
    {
        m_OkButtonFunc.onClick.Clear();

        switch(_QuestSlot.QuestSlotType)
        {
            case Quest_Slot.SLOTTYPE.SLOT_WAIT:
                m_OkStat = NPC_OKBUTTON_STAT.STAT_ACCEPT;
                m_OkButtonText.text = "Accept";
                AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                    "AcceptQuest");
                AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                    "ResetQuestSelection");
                break;

            case Quest_Slot.SLOTTYPE.SLOT_ORDER:
                if(!_QuestSlot.GetClearQuestResult())
                {
                    m_OkStat = NPC_OKBUTTON_STAT.STAT_DISCARD;
                    m_OkButtonText.text = "Discard";
                    AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                        "DiscardQuest");
                    AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                        "ResetQuestSelection");
                }
                else
                {
                    m_OkStat = NPC_OKBUTTON_STAT.STAT_COMPLETE;
                    m_OkButtonText.text = "Complete";
                    AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                        "QuestClear");
                    AddButtonEventDelegate(Vilage_QuestManager.GetInstance(),
                        "ResetQuestSelection");
                }
                break;
        }
    }

    public void HideNPCExtensionFunc()
    {
        foreach (BoxCollider boxcol in m_ButtonColliders)
            boxcol.enabled = false;

        TweenAlpha.Begin(gameObject, 0.25f, 0f);
        m_OwnExtension.Invoke("SelfHide", 0.25f);
    }

    public void AddButtonEventDelegate(MonoBehaviour _ObjectTarget, 
        string _MethodName)
    {
        EventDelegate newEvent = new EventDelegate(
            _ObjectTarget, _MethodName);
        m_OkButtonFunc.onClick.Add(newEvent);
    }
}
