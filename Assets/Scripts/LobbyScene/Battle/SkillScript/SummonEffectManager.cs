using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummonEffectManager : 
    Singleton<SummonEffectManager>{

    private Dictionary<string, GameObject> m_SummonEffectDic =
        new Dictionary<string, GameObject>();


    void Awake()
    {
        CreateInstance();
        SetSummonEffectContainer();
    }

    //Init SummonEffects
    private void SetSummonEffectContainer()
    {
        m_SummonEffectDic.Add("BlackHole", SetSummonEffect("BlackHole"));
        m_SummonEffectDic.Add("Thunder", SetSummonEffect("Thunder"));
    }

    private GameObject SetSummonEffect(string SummonEffectName)
    {
        string LoadPath = "BattleScene/Monster_Summon_Effect/Monster_Summon_Effect(";
        string LoadFullPath = LoadPath + SummonEffectName + ")";

        GameObject SummonEffect = Resources.Load(LoadFullPath) as GameObject;
        return SummonEffect;
    }

    //Call SummonEffects
    public void SummonEffectCall(string SummonEffectName, 
        bool _isElite, int _SummonMonsterIdx, SUMMONPOSITIONID _CreatePositionID)
    {
        if(!m_SummonEffectDic.ContainsKey(SummonEffectName))
        {
            Debug.Log("Cannot Find SummonEffect");
            return;
        }

        GameObject SummonEffect = Instantiate(m_SummonEffectDic[SummonEffectName],
            MonsterManager.GetInstance().InFieldPosition.position,
            Quaternion.identity) as GameObject;

        SummonEffectCtrl EffectInfo = Mecro.MecroMethod.CheckGetComponent<SummonEffectCtrl>(
            SummonEffect);
        EffectInfo.SetCreateMonsterInfo(_isElite, _SummonMonsterIdx, _CreatePositionID);

        SummonEffect.transform.parent = this.transform;
    }

   
}
