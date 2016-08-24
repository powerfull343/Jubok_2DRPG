using UnityEngine;
using System.Collections;
using LobbyManager;

public class UI_UpperStat_Stamina : MonoBehaviour {

    [SerializeField]
    private UILabel m_StaminaText;
    [SerializeField]
    private UILabel m_DetailStatminaText;
    private int m_StaminaAmount;
    private int m_MaxStaminaAmount;

    private float m_StaminaReduceRate = 60f;

	void Start () {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_StaminaText);

        if (LobbyController.GetInstance().mCurrentID != FIELDID.ID_VILAGE &&
            LobbyController.GetInstance().mCurrentID != FIELDID.ID_CASTLE)
        {
            ResetStaminaPoint();
            Invoke("ReducingStatmina", 1f);
        }
        else
            m_StaminaText.text = "Stamina : " + 
                PlayerDataManager.GetInstance().ResultStat.Stamina.ToString();
    }

    public void ReducingStatmina()
    {
        StartCoroutine("StaminaReduce");
    }

    private void ResetStaminaPoint()
    {
        m_StaminaAmount = PlayerCtrlManager.GetInstance().PlayerCtrl.Stamina;
        m_MaxStaminaAmount = m_StaminaAmount;
        m_StaminaText.text = "Stamina : " + m_StaminaAmount;
    }

    IEnumerator StaminaReduce()
    {
        yield return new WaitForSeconds(m_StaminaReduceRate);

        while (m_StaminaAmount >= 1)
        {
            --m_StaminaAmount;
            m_StaminaText.text = "Stamina : " + m_StaminaAmount;
            DetailStatminaLabel();
            yield return new WaitForSeconds(m_StaminaReduceRate);
        }

        yield break;
    }

    public void DetailStatminaLabel()
    {
        if (m_DetailStatminaText.gameObject.activeSelf)
        {
            //m_DetailStatminaText.text =
            //    PlayerCtrlManager.GetInstance().PlayerCtrl.Stamina.ToString() + " / " + 
            //PlayerCtrlManager.GetInstance().PlayerCtrl.MaxStamina.ToString() + "\n" + 
            //"-1 / " + m_StaminaReduceRate.ToString() + "(Sec)";

            m_DetailStatminaText.text =
                m_StaminaAmount.ToString() + " / " +
            m_MaxStaminaAmount.ToString() + "\n" +
            "-1 / " + m_StaminaReduceRate.ToString() + "(Sec)";

        }
    }
}
    