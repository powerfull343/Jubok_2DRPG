using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Money : MonoBehaviour {

    [SerializeField]
    private UILabel m_MoneyText;
    private int m_nRenderingMoney;
    private int m_nRenderingResult;
    private int m_ChangeAmount = 0;
    private static Queue<int> m_ResultContainer =
        new Queue<int>();

    [SerializeField]
    private Transform m_MoneyIcon;
    private bool m_isMoneyUpdating = false;
    private bool m_isUpdatingChange = false;

    public float m_fMoneyMaxTime = 0.1f;
    public float m_fMoneyTickCount = 0.001f;
    public int m_nMoneyChangeMultiplyRate = 8;

    void OnEnable()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_MoneyText);
        m_nRenderingMoney = DataController.GetInstance().InGameData.Money;
        m_nRenderingResult = m_nRenderingMoney;
        m_MoneyText.text = m_nRenderingMoney.ToString();
    }

	void Start () {

        Mecro.MecroMethod.CheckExistComponent<Transform>(m_MoneyIcon);
        StartCoroutine("MoneySpriteRotate");
    }

    public void UpdateMoneySize(int ChangeMoneySize)
    {
        m_ChangeAmount += ChangeMoneySize;
        //int TickResult = ChangeMoneySize + BeforeMoneySize;

        //Debug.Log("ChangeMoneySize : " + ChangeMoneySize);
        //Debug.Log("BeforeMoneySize : " + BeforeMoneySize);
        //Debug.Log("TickResult : " + TickResult);
        //m_ResultContainer.Enqueue(TickResult);

        if (!m_isMoneyUpdating)
        {
            m_nRenderingResult = m_nRenderingMoney + ChangeMoneySize;
            StartCoroutine("StartUpdateMoneySize");
        }
        else
        {
            m_nRenderingResult += ChangeMoneySize;
            m_isUpdatingChange = true;
        }

        //DataController.GetInstance().InGameData.Money += MoneySize;
    }

    IEnumerator StartUpdateMoneySize()
    {
        //m_nRenderingMoney = m_BeforeMoneyAmount;
        int nUpdateMoneyAmount = SetTickMoneySize();

        bool nMoneyRate = ChangeAmountCheck(m_nRenderingMoney,
                m_nRenderingMoney + m_ChangeAmount);
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
                    m_nRenderingMoney -= nUpdateMoneyAmount;
                    m_ChangeAmount += nUpdateMoneyAmount;
                }
                else
                {
                    //m_nRenderingMoney =
                    //    DataController.GetInstance().InGameData.Money;
                    m_nRenderingMoney = m_nRenderingResult;
                    m_ChangeAmount = 0;
                    m_MoneyText.text = m_nRenderingMoney.ToString();
                    //Debug.Log(DataController.GetInstance().InGameData.Money);
                    break;
                }

                m_MoneyText.text = m_nRenderingMoney.ToString();
            }
            else //소지금이 증가할때
            {
                if (m_ChangeAmount >= 0)
                {
                    m_nRenderingMoney += nUpdateMoneyAmount;
                    m_ChangeAmount -= nUpdateMoneyAmount;
                }
                else
                {
                    //m_nRenderingMoney =
                    //    DataController.GetInstance().InGameData.Money;
                    m_nRenderingMoney = m_nRenderingResult;
                    m_ChangeAmount = 0;
                    m_MoneyText.text = m_nRenderingMoney.ToString();
                    //Debug.Log(DataController.GetInstance().InGameData.Money);
                    break;
                }
                m_MoneyText.text = m_nRenderingMoney.ToString();
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

    /// <summary>
    /// 소지금의 결과물이 여러번 변경되기 때문에 
    /// 해당 함수를 통해 결과물을 1차적으로 저장한다.
    /// </summary>
    public static void AddResultContainer(int nResult)
    {

    }
}
