using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowDetailStat : MonoBehaviour {

    [SerializeField]
    private Transform m_DetailTrans;
    private UIPanel m_DetailPanelComp;

    private BoxCollider m_BoxCollider;

    [SerializeField]
    private Transform m_ButtonScale;
    [SerializeField]
    private int m_StatIndex = 0;

    public DetailStat_ClassInfo m_ClassInfo;

    void Start()
    {
        m_BoxCollider = Mecro.MecroMethod.CheckGetComponent<BoxCollider>(this.gameObject);
        Mecro.MecroMethod.CheckExistComponent<Transform>(m_DetailTrans);
        m_DetailPanelComp = Mecro.MecroMethod.CheckGetComponent<UIPanel>(m_DetailTrans);
        Mecro.MecroMethod.CheckExistComponent<Transform>(m_ButtonScale);
        Mecro.MecroMethod.CheckExistComponent<DetailStat_ClassInfo>(m_ClassInfo);
    }

    public void ShowPopupBox()
    {
        float fScaleY = m_ButtonScale.localScale.y >= 0 ? -1f : 1f;
        m_ButtonScale.localScale = new Vector3(1f, fScaleY, 1f);
        Vector3 vDebugingPos = new Vector3(m_DetailPanelComp.GetViewSize().x,
            m_DetailPanelComp.GetViewSize().y);
        Debug.Log(vDebugingPos);

        if (!m_DetailTrans.gameObject.activeSelf)
        {
            m_ClassInfo.MoveOtherElements(m_StatIndex, true, vDebugingPos.y);
            m_DetailTrans.localPosition = -new Vector3(0f,
                (m_BoxCollider.size.y / 2f) + (vDebugingPos.y / 2f),
                0f);
            m_DetailTrans.gameObject.SetActive(true);
        }
        else
        {
            m_DetailTrans.gameObject.SetActive(false);
            m_ClassInfo.MoveOtherElements(m_StatIndex, false, vDebugingPos.y);
        }
    }

    
}
