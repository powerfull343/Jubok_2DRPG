using UnityEngine;
using System.Collections;
using Mecro;

public class MonsterHpUICtrl : MonoBehaviour {

    private Transform ForeHpUI;
    private Monster_Interface MonsterInfo;
    private BossHpBar _HpBar;
    public BossHpBar HpBar
    {
        get { return _HpBar; }
        set { _HpBar = value; }
    }

    private Vector3 m_HpBarOffset;

    void Start()
    {
        ForeHpUI =
            MecroMethod.CheckGetComponent<Transform>(this.transform.FindChild("ForeUI"));
        MonsterInfo =
            MecroMethod.CheckGetComponent<Monster_Interface>(this.transform.parent.FindChild("MonsterBody"));

        m_HpBarOffset = transform.localPosition;

        if (MonsterInfo.grade >= MONSTERGRADEID.GRADE_BOSS)
            ShowBossHpUI();
        else
            StartCoroutine("NormalHpUI");
    }

    IEnumerator NormalHpUI()
    {
        while(true)
        {
            float fRate = (float)MonsterInfo.Hp / MonsterInfo.MaxHp;

            if (fRate <= 0f)
                fRate = 0f;

            Vector3 vUIscale = new Vector3(fRate,
                ForeHpUI.localScale.y,
                ForeHpUI.localScale.z);

            ForeHpUI.localScale = Vector3.Lerp(ForeHpUI.localScale, vUIscale, 1f);

            transform.localPosition = MonsterInfo.transform.localPosition + m_HpBarOffset;

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    public void ShowBossHpUI()
    {
        if (MonsterInfo.isOutSummonMonster)
        {
            Debug.Log("OutMonster");
            return;
        }

        BattleScene_NGUI_Panel.GetInstance().CreateBossHpBar(MonsterInfo.LoadPrefabName);
        _HpBar = BattleScene_NGUI_Panel.GetInstance().GetSummonedHpBar();
        _HpBar.MonsterInfo = MonsterInfo;
        _HpBar.gameObject.SetActive(true);
    }
	
}
