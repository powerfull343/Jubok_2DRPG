using UnityEngine;
using System.Collections;

public class Skill_PinPanel_FireBall : MonoBehaviour {

    private ParticleSystem m_FireParticle;
    private GameObject_Extension m_GameObjectInfo;
    private Vector3 m_vDirection;

    private Skill_PinPanel m_ParentSkillInfo;
    public Skill_PinPanel ParentSkillInfo
    {
        get { return m_ParentSkillInfo; }
        set { m_ParentSkillInfo = value; }
    }

    private int m_AtkPower;
    public int AtkPower
    {
        get { return m_AtkPower; }
        set { m_AtkPower = value; }
    }
    
	// Use this for initialization
	void OnEnable () {

        m_FireParticle = 
            Mecro.MecroMethod.CheckGetComponent<ParticleSystem>(this.gameObject);

        m_GameObjectInfo =
            Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
    }

    public void FireBallStartPlay()
    {
        if (!m_FireParticle.isPlaying)
            m_FireParticle.Play();
    }

    public void FireBallShot()
    {
        StartCoroutine("FireBallMoving");
    }

    IEnumerator FireBallMoving()
    {
        yield return new WaitForSeconds(1f);
        SetDirection(m_GameObjectInfo.TagNum, false);
        float fTime = 0f;

        while (true)
        {
            transform.position += m_vDirection * 0.25f;

            if (fTime >= 9f)
            {
                m_ParentSkillInfo.ChildFireBalls.Remove(this);
                Destroy(this.gameObject);
                break;
            }
            else
                fTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    private void SetDirection(int CompareNumber, bool isReflection)
    {
        m_vDirection = Vector3.zero;

        switch (CompareNumber)
        {
            case 0://down
                if (!isReflection)
                    m_vDirection = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), -1f, 0f);
                else
                {
                    m_vDirection = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f),
                        UnityEngine.Random.Range(-0.25f, -1f), 0f);
                }
                break;

            case 1://up
                if(!isReflection)
                    m_vDirection = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 1f, 0f);
                else
                {
                    m_vDirection = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f),
                        UnityEngine.Random.Range(0.25f, 1f), 0f);
                }
                break;

            case 2://right
                if (!isReflection)
                    m_vDirection = new Vector3(1f, UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
                else
                {
                    m_vDirection = new Vector3(UnityEngine.Random.Range(0.25f, 1f),
                       UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
                }
                break;

            case 3://left
                if(!isReflection)
                    m_vDirection = new Vector3(-1f, UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
                else
                {
                    m_vDirection = new Vector3(UnityEngine.Random.Range(-0.25f, -1f),
                       UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
                }
                break;

            default:
                Debug.LogError("Cannot Find TagNumber");
                break;
        }

        SetVelocity();
    }

    private void SetVelocity()
    {
        Vector3 vCurveInfo = m_vDirection * -1f;

        ParticleSystem.Particle[] tempparticles = new ParticleSystem.Particle[m_FireParticle.particleCount];
        int Count = m_FireParticle.GetParticles(tempparticles);

        for (int i = 0; i < Count; i++)
        {
            tempparticles[i].velocity = new Vector3(vCurveInfo.x * 0.5f,
                vCurveInfo.y, vCurveInfo.z);
        }

        m_FireParticle.SetParticles(tempparticles, Count);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("OverTheArea"))
        {
            OvertheAreaCollider ColliderInfo =
                other.gameObject.GetComponent<OvertheAreaCollider>();

            if (!ColliderInfo)
                Debug.LogError("Collider Comp is null");

            SetDirection((int)ColliderInfo.m_ColDir, true);
        }
        else if(other.gameObject.CompareTag("Monster"))
        {
            Moveable_Object ObjectInfo =
                Mecro.MecroMethod.CheckGetComponent<Moveable_Object>(other.gameObject);

            if(ObjectInfo.Hp > 0)
                ObjectInfo.SetHp(AtkPower);

            //if (ObjectInfo.Hp <= 0)
            //    SkillManager.GetInstance().MonsterHpZero(ObjectInfo);
        }
        
    }
	
}
