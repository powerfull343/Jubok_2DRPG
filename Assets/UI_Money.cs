using UnityEngine;
using System.Collections;

public class UI_Money : MonoBehaviour {

    [SerializeField]
    private UILabel m_MoneyText;
    private int m_ChangeAmount = 0;
    private int m_BeforeMoneyAmount = 0;
    [SerializeField]
    private Transform m_MoneyIcon;
    private bool m_isMoneyUpdating = false;
    private bool m_isUpdatingChange = false;

    public float m_fMoneyMaxTime = 0.1f;
    public float m_fMoneyTickCount = 0.001f;
    public int m_nMoneyChangeMultiplyRate = 8;

	void Start () {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_MoneyText);
        m_MoneyText.text = 
            DataController.GetInstance().InGameData.Money.ToString();

        Mecro.MecroMethod.CheckExistComponent<Transform>(m_MoneyIcon);
        StartCoroutine("MoneySpriteRotate");
    }

    public void UpdateMoneySize(int MoneySize)
    {
        m_ChangeAmount += MoneySize;

        if (!m_isMoneyUpdating)
        {
            m_BeforeMoneyAmount = DataController.GetInstance().InGameData.Money;
            StartCoroutine("StartUpdateMoneySize");
        }
        else
            m_isUpdatingChange = true;

        DataController.GetInstance().InGameData.Money += MoneySize;
    }

    IEnumerator StartUpdateMoneySize()
    {
        int nRenderingMoney = m_BeforeMoneyAmount;
        int nUpdateMoneyAmount = SetTickMoneySize();
        //Debug.LogError(nUpdateMoneyAmount);

        bool nMoneyRate = ChangeAmountCheck(m_BeforeMoneyAmount,
                m_BeforeMoneyAmount + m_ChangeAmount);
        m_isMoneyUpdating = true;

        while(true)
        {
            if (m_isUpdatingChange)
            {
                nUpdateMoneyAmount = SetTickMoneySize();
                //Debug.LogError(nUpdateMoneyAmount);
                m_isUpdatingChange = false;
            }

            //소지금이 감소할때
            if (!nMoneyRate)
            {
                if (m_ChangeAmount <= 0)
                {
                    nRenderingMoney -= nUpdateMoneyAmount;
                    m_ChangeAmount += nUpdateMoneyAmount;
                }
                else
                {
                    nRenderingMoney =
                        DataController.GetInstance().InGameData.Money;
                    m_ChangeAmount = 0;
                    m_MoneyText.text = nRenderingMoney.ToString();
                    break;
                }

                m_MoneyText.text = nRenderingMoney.ToString();
            }
            else //소지금이 증가할때
            {
                if (m_ChangeAmount >= 0)
                {
                    nRenderingMoney += nUpdateMoneyAmount;
                    m_ChangeAmount -= nUpdateMoneyAmount;
                }
                else
                {
                    nRenderingMoney =
                        DataController.GetInstance().InGameData.Money;
                    m_ChangeAmount = 0;
                    m_MoneyText.text = nRenderingMoney.ToString();
                    break;
                }
                m_MoneyText.text = nRenderingMoney.ToString();
            }
            yield return new WaitForSeconds(m_fMoneyTickCount);
        }

        m_isMoneyUpdating = false;

        yield return null;
    }

    private int SetTickMoneySize()
    {
        float fFrameTimeTick = m_fMoneyMaxTime / m_fMoneyTickCount;
        //Debug.Log("fFrameTimeTick : " + fFrameTimeTick);
        float fFrameTick = (float)m_ChangeAmount / fFrameTimeTick;
        //Debug.Log("fFrameTick : " + fFrameTick);
        int nResult = Mathf.Abs((int)fFrameTick);

        if (Mathf.Abs(nResult) <= 1)
        {
            //Debug.LogError("nResult : 1");
            return 1 * m_nMoneyChangeMultiplyRate;
        }
        //Debug.LogError("nResult : " + nResult * m_nMoneyChangeMultiplyRate);
        return nResult * m_nMoneyChangeMultiplyRate;
    }

    IEnumerator MoneySpriteRotate()
    {
        Vector3 fRotateSpeed = new Vector3(0f, 2f, 0f);
        while(true)
        {
            m_MoneyIcon.Rotate(fRotateSpeed);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    

    /// <summary>
    /// return false는 소지금이 감소하였을시 true는 증가
    /// </summary>
    private bool ChangeAmountCheck(int nBeforeSize, int nCurrentSize)
    {
        //소지금이 감소하였을때
        if(nBeforeSize > nCurrentSize)
            return false;

        return true;
    }
}
