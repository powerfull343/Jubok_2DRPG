using UnityEngine;
using System.Collections;

public class PlayerLowerPointUICtrl : MonoBehaviour {

    /// <summary>
    /// true = Hp, false = stamina
    /// </summary>
    public bool m_isHp = true;
    private Moveable_Object m_PlayerInfo;
    private UISprite m_HpUI;
    private int m_AtlasMaxCount;

    void Awake() {
        m_HpUI = GetComponent<UISprite>();
        m_AtlasMaxCount = m_HpUI.atlas.spriteList.Count;
    }

    void OnEnable()
    {
        Invoke("StartUIAnimation", 0.05f);
    }

    void StartUIAnimation()
    {
        m_PlayerInfo = PlayerCtrlManager.GetInstance().PlayerCtrl;
        StartCoroutine("UIAnimation");
        m_HpUI.spriteName = "UI34";
    }

    float CalcHpToAtlas()
    {
        float fResult = 
            (float)m_PlayerInfo.Hp / m_PlayerInfo.MaxHp;

        if (fResult <= 0f)
            fResult = 0f;

        return fResult;
    }

    float CalcManaToAtlas()
    {
        float fResult = (float)m_PlayerInfo.Mp / m_PlayerInfo.MaxMp;

        if (fResult <= 0f)
            fResult = 0f;

        return fResult;
    }

    IEnumerator UIAnimation()
    {
        yield return new WaitForSeconds(1f);

        while(true)
        {
            string RenderSpriteName;
            float SpriteRate = 0f;
            if (m_isHp)
                SpriteRate = m_AtlasMaxCount * CalcHpToAtlas();
            else
                SpriteRate = m_AtlasMaxCount * CalcManaToAtlas();

            RenderSpriteName = "UI" + RateNumberToString(SpriteRate);

            m_HpUI.spriteName = RenderSpriteName;

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    string RateNumberToString(float nRate)
    {
        int SpriteNumber = (int)nRate - 1;

        if (SpriteNumber <= 0)
            SpriteNumber = 0;

        if (SpriteNumber < 10)
            return "0" + SpriteNumber.ToString();
        else
            return SpriteNumber.ToString();
    }
}
