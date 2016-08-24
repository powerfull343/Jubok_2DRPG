using UnityEngine;
using System.Collections;

public class Battle_NGUI_EventMsg 
    : Singleton<Battle_NGUI_EventMsg> {

    private GameObject m_OriginEventLabel;

    void Start()
    {
        m_OriginEventLabel = Resources.Load("BattleScene/UI/Label - EventMessage") as GameObject;
        if(!m_OriginEventLabel)
            Debug.LogError(m_OriginEventLabel.name + " Object is Null!");

        //Vector3 PlayerPosition = 
        //    Camera.main.WorldToScreenPoint(
        //        PlayerCtrlManager.GetInstance().Player.position);

        //transform.position =
        //    BattleScene_NGUI_Panel.GetInstance().NGUICamera.ScreenToWorldPoint(PlayerPosition);

        transform.position =
            Mecro.MecroMethod.NormalToNGUIWorldPos(
                PlayerCtrlManager.GetInstance().Player.position);

        transform.localPosition += new Vector3(0f, 50f, 0f);

        CallEventMessage("Test", Color.white);
    }

    public void CallEventMessage(string strMessage, Color LabelColor)
    {
        GameObject EventLabelMsg = Instantiate(m_OriginEventLabel);

        EventLabelMsg.SetActive(false);

        EventLabelMsg.transform.parent = this.transform;
        EventLabelMsg.transform.localPosition = Vector3.zero;
        EventLabelMsg.transform.localScale = Vector3.one;

        UILabel EventTextInfo = Mecro.MecroMethod.CheckGetComponent<UILabel>(EventLabelMsg);

        EventTextInfo.text = strMessage;
        EventTextInfo.color = LabelColor;

        EventLabelMsg.SetActive(true);
    }
}
