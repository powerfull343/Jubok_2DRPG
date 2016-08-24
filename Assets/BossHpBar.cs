using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossHpBar : MonoBehaviour {

    private UISprite m_ForeFrontHpBar;
    private UISprite m_ForeBackHpBar;
    private int m_ColorFrontIdx = 0;

    private GameObject_Extension m_Extension;

    private static List<Color> m_HpColorList =
        new List<Color>();
    private static bool m_isHpColorSetting = false;

    private Monster_Interface m_MonsterInfo;
    public Monster_Interface MonsterInfo
    {
        get { return m_MonsterInfo; }
        set { m_MonsterInfo = value; }
    }

    float m_fStartTime = 0, m_fEndTime = 0, m_fDiscountAmount = 0;

    void Awake()
    {
        if (!m_isHpColorSetting)
            SetHpColorOrder();
    }

    void SetHpColorOrder()
    {
        m_HpColorList.Add(new Color(0.54f, 0f, 0.54f));     //dark Magenta
        m_HpColorList.Add(new Color(0.29f, 0f, 0.5f));      //Indigo
        m_HpColorList.Add(new Color(0f, 0f, 0.85f));        //blue
        m_HpColorList.Add(new Color(0f, 0.85f, 0f));        //green
        m_HpColorList.Add(new Color(0.85f, 0.85f, 0f));     //yellow
        m_HpColorList.Add(new Color(0.85f, 0.65f, 0f));     //orange
        m_HpColorList.Add(new Color(0.85f, 0f, 0f));        //red
        m_isHpColorSetting = true;
        //Debug.Log("HpColorlistCount : " + m_HpColorList.Count);
        //Debug.Log("Awaking ColorOrder");
    }

    void OnEnable()
    {
        m_ForeFrontHpBar = Mecro.MecroMethod.CheckGetComponent<
            UISprite>(transform.FindChild("Sprite - ForeGround01"));
        m_ForeBackHpBar = Mecro.MecroMethod.CheckGetComponent<
            UISprite>(transform.FindChild("Sprite - ForeGround02"));

        InitHpBar();
        UpdateHpForeGrounds();

        m_Extension = Mecro.MecroMethod.CheckGetComponent<
            GameObject_Extension>(this.gameObject);
        StartCoroutine("EnableHpRendering");
    }

    void InitHpBar()
    {
        m_ForeFrontHpBar.color = m_HpColorList[m_ColorFrontIdx];
        m_ForeBackHpBar.color = m_HpColorList[m_ColorFrontIdx + 1];
    }

    IEnumerator EnableHpRendering()
    {
        m_ForeFrontHpBar.fillAmount = 0f;
        m_ForeBackHpBar.fillAmount = 0f;

        while (true)
        {
            if(m_ForeFrontHpBar.fillAmount <= 1f)
                m_ForeFrontHpBar.fillAmount += 0.01f;
            if (m_ForeBackHpBar.fillAmount <= 1f)
                m_ForeBackHpBar.fillAmount += 0.02f;

            UpdateHpForeGrounds();

            if (m_ForeFrontHpBar.fillAmount >= 1f &&
                m_ForeBackHpBar.fillAmount >= 1f)
            {
                m_ForeFrontHpBar.fillAmount = 1f;
                m_ForeBackHpBar.fillAmount = 1f;
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine("AutoHpAmountRendering");

        yield break;
    }

    /*
    100 - 96 = 4;
    4 % 14 = 4;
    4 / 14 = 0.14;
    1 - 0.14 = 0.86;

    100 - 54 = 46;
    46 % 14 = 4;
    4 / 14 = 0.14;
    1 - 0.14 = 0.86;
    */
    IEnumerator AutoHpAmountRendering()
    {
        float fDivide = (float)1 / m_HpColorList.Count;
        
        while (true)
        {
            float fMinusHpRate = 1f - ((float)m_MonsterInfo.Hp / m_MonsterInfo.MaxHp);
            if (fMinusHpRate >= 1f)
            {
                m_ForeFrontHpBar.fillAmount = 0f;
                break;
            }
            else if (fMinusHpRate >= (fDivide * (m_ColorFrontIdx + 1)))
            {
                ChangeFrontToBack();
                ForeHpGroundReset();
                ChangeBackColor();
                UpdateHpForeGrounds();
            }
            else
            {
                //Origin
                float fDivideReminder = (fMinusHpRate % fDivide);
                fDivideReminder = (fDivideReminder / fDivide);
                float fResult = 1 - fDivideReminder;

                m_ForeFrontHpBar.fillAmount = fResult;

                //Lerp
                //HitTimeCheck(fResult);

                //float fFreq = ((Time.deltaTime - m_fStartTime) / m_fEndTime);
                //fResult = fResult - (m_fDiscountAmount * fFreq);

                //Debug.Log(fFreq);
                //Debug.Log((m_fDiscountAmount * fFreq));
                //Debug.LogError(m_fDiscountAmount);

                //m_ForeFrontHpBar.fillAmount = fResult;
                
            }

            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
        yield break;
    }

    //void HitTimeCheck(float fResult)
    //{
    //    if (!m_isHited)
    //        return;

    //    m_fStartTime = Time.deltaTime;
    //    m_fEndTime = 1f;
    //    m_fDiscountAmount = m_ForeFrontHpBar.fillAmount - fResult;
    //    Invoke("isHitReset", m_fEndTime);
    //}
    
    //void isHitReset()
    //{
    //    m_isHited = false;
    //}


    void ChangeFrontToBack()
    {
        UISprite TempHpBar = m_ForeFrontHpBar;
        m_ForeFrontHpBar = m_ForeBackHpBar;
        m_ForeBackHpBar = TempHpBar;
    }

    void ForeHpGroundReset()
    {
        //Debug.Log("Reset");
        m_ForeFrontHpBar.depth = 1;

        m_ForeBackHpBar.fillAmount = 1f;
        m_ForeBackHpBar.depth = 0;
        
    }

    void ChangeBackColor()
    {
        if (++m_ColorFrontIdx + 1 > m_HpColorList.Count - 1)
        {
            m_ForeBackHpBar.gameObject.SetActive(false);
            return;
        }

        //Debug.LogError("m_ColorFrontIdx = " + m_ColorFrontIdx);
        m_ForeBackHpBar.color = m_HpColorList[m_ColorFrontIdx + 1];
    }

    void UpdateHpForeGrounds()
    {
        m_ForeFrontHpBar.Update();
        m_ForeBackHpBar.Update();
    }

}
