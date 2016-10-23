using UnityEngine;
using System.Collections;

public class Battle_NGUI_EventMsg : MonoBehaviour {

    private static GameObject m_OriginEventLabel;
    private Transform m_TargetTrans;
    private bool m_isBiggerObject;

    public void InitEventMsg(Transform TargetTrans, bool isBiggerObject)
    {
        if (!m_OriginEventLabel)
            m_OriginEventLabel = Resources.Load("BattleScene/UI/Label - EventMessage") as GameObject;

        m_TargetTrans = TargetTrans;
        m_isBiggerObject = isBiggerObject;
    }

    private void SetCallingPosition()
    {
        transform.position =
            Mecro.MecroMethod.NormalToNGUIWorldPos(
                m_TargetTrans.position);

        if (!m_isBiggerObject)
            transform.localPosition += new Vector3(0f, 50f, 0f);
        else
            transform.localPosition += new Vector3(0f, 100f, 0f);
    }

    public void CallEventMessage(string strMessage, Color LabelColor)
    {
        GameObject EventText = Instantiate(m_OriginEventLabel);
        EventText.SetActive(false);
        EventText.transform.SetParent(this.transform, false);

        SetCallingPosition();

        EventText.transform.localScale = Vector3.one * 2f;

        UILabel EventTextInfo = Mecro.MecroMethod.CheckGetComponent<UILabel>(EventText);

        EventTextInfo.text = strMessage;
        EventTextInfo.color = LabelColor;

        EventText.SetActive(true);
    }
}
