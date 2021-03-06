﻿using UnityEngine;
using System.Collections;
using LobbyManager;

public class UI_UpperStat_Health : MonoBehaviour {

    [SerializeField]
    private UILabel m_HealthText;

    void OnEnable()
    {
        if (LobbyController.GetInstance().mCurrentID != FIELDID.ID_VILAGE)
            StartCoroutine("AutoHealthRendering");

    }

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_HealthText);
    }

    //void LateUpdate()
    //{
    //    m_HealthText.text = PlayerCtrlManager.GetInstance().PlayerCtrl.Hp.ToString() +
    //        " / " + PlayerCtrlManager.GetInstance().PlayerCtrl.MaxHp.ToString();
    //}

    IEnumerator AutoHealthRendering()
    {
        Debug.Log("Health");

        while(true)
        {
            m_HealthText.text = PlayerCtrlManager.GetInstance().PlayerCtrl.Hp.ToString() +
            " / " + PlayerCtrlManager.GetInstance().PlayerCtrl.MaxHp.ToString();
            yield return new WaitForFixedUpdate();
        }

        yield return null; 
    }

}
