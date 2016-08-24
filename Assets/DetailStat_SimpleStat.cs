using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetailStat_SimpleStat : MonoBehaviour {

    [SerializeField]
    private UILabel m_HpLabel;
    [SerializeField]
    private UILabel m_AtkLabel;
    [SerializeField]
    private UILabel m_ClassLabel;

    [SerializeField]
    private Transform m_ClassDetailStat;

    void Start()
    {
        InitExistComp();
        UpdateInfo();
    }

    void InitExistComp()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_HpLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_AtkLabel);
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_ClassLabel);

        Mecro.MecroMethod.CheckExistComponent<Transform>(m_ClassDetailStat);

        //Debuging
        m_ClassDetailStat.localPosition = new Vector3(-150f, -10f, 0f);
        m_ClassDetailStat.gameObject.GetComponent<UIWidget>().alpha = 0f;
        m_ClassDetailStat.gameObject.SetActive(false);
    }

    void UpdateInfo()
    {
        PlayerData LoadedData = DataController.GetInstance().InGameData;

        m_HpLabel.text = "HP : " + LoadedData.Health;
        m_AtkLabel.text = "Attack : " + LoadedData.Attack;
        m_ClassLabel.text = "Class : Magician";
    }

    public void SetDetailClassInfo()
    {
        if (!m_ClassDetailStat.gameObject.activeSelf)
        {//Show
            m_ClassDetailStat.gameObject.SetActive(true);
            Vector3 ShowPosition = new Vector3(150f, -10f, 0f);
            TweenAlpha.Begin(m_ClassDetailStat.gameObject, 1f, 1f);
            TweenPosition.Begin(m_ClassDetailStat.gameObject, 1f, ShowPosition);
        }
        else
        {
            Vector3 HidePosition = new Vector3(-150f, -10f, 0f);
            TweenAlpha.Begin(m_ClassDetailStat.gameObject, 1f, 0f);
            TweenPosition.Begin(m_ClassDetailStat.gameObject, 1f, HidePosition);
            Invoke("HideClassInfo", 1f);
        }
    }

    private void HideClassInfo()
    {
        m_ClassDetailStat.gameObject.SetActive(false);
    }


}
