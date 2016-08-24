using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battle_NGUI_ClickDamPanel_HitEffects : MonoBehaviour {

    private List<GameObject> m_HitPrefabs =
        new List<GameObject>();


    void Start()
    {
        AddHitPrefabObjects();
    }


    private void AddHitPrefabObjects()
    {
        //Test Only One
        GameObject HitEffect =
            Resources.Load("BattleScene/Particles/HitEffect01") as GameObject;

        m_HitPrefabs.Add(HitEffect);
    }

    public GameObject GetRandomHitEffect()
    {
        int nRdmIdx = UnityEngine.Random.Range(0, m_HitPrefabs.Count);
        if (!m_HitPrefabs[nRdmIdx])
            Debug.LogError("Cannot Find HitEffect Object");

        return m_HitPrefabs[nRdmIdx];
    }

	
}
