using UnityEngine;
using System.Collections;
using Mecro;

public class Skill_CriticalLighting_Effect : MonoBehaviour {

    private ParticleSystem m_LightingParticle;
    private Skill_Interface m_ParentSkillInfo;
    public Skill_Interface ParentSKillInfo
    {
        get { return m_ParentSkillInfo; }
        set { m_ParentSkillInfo = value; }
    }

    void Start () {
        m_LightingParticle =
           MecroMethod.CheckGetComponent<ParticleSystem>(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
    {
        ParentSKillInfo.AttackToCollider(other);
    }
}
    