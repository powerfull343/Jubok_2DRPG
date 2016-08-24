using UnityEngine;
using System.Collections;

public class SkeletonBoomerang_Boomerang : MonoBehaviour {

    private SkeletonBoomerang m_ThrowMonster;

    private Vector3 m_vPivot = new Vector3(1f, 0f, 0f);
    private bool m_isReturnBoomerang = false;
    private Vector3 m_vAxisPoint;
    private Vector3 m_ReturnTargetPos;

    private Bullet_Extension BulletInfo;

    void Start()
    {
        BulletInfo =
            Mecro.MecroMethod.CheckGetComponent<Bullet_Extension>(this.gameObject);
    }

    void Update()
    {
        if (m_isReturnBoomerang)
            ReturnMovingBoomerang();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!m_isReturnBoomerang && other.gameObject.CompareTag("Player"))
            m_vAxisPoint = other.transform.position + m_vPivot;
        else if(m_isReturnBoomerang)
            CheckingColThrowMonster(other);
    }

    void OnTriggerStay(Collider other)
    {
        if (!m_isReturnBoomerang && other.gameObject.CompareTag("Player"))
        {
            transform.RotateAround(m_vAxisPoint, Vector3.forward, -Time.time);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_isReturnBoomerang = true;
            if (BulletInfo.enabled)
                BulletInfo.enabled = false;

            SetReturnTargetPos();
        }
    }

    void ReturnMovingBoomerang()
    {
        transform.position += Vector3.Normalize(
            m_ReturnTargetPos - transform.position) * 0.1f;

        if (!m_ThrowMonster || m_ThrowMonster.Hp <= 0)
            BulletInfo.SelfDestroy();
    }

    void CheckingColThrowMonster(Collider other)
    {
        if(other && m_ThrowMonster &&
            other.gameObject == m_ThrowMonster.gameObject)
        {
            m_isReturnBoomerang = false;
            Invoke("BoomerangAtkDelay", m_ThrowMonster.AtkDelayTime);
            this.gameObject.SetActive(false);
            BulletInfo.enabled = true;
        }
    }

    void BoomerangAtkDelay()
    {
        m_ThrowMonster.IsThrow = false;
    }

    void SetReturnTargetPos()
    {
        if (m_ThrowMonster)
            m_ReturnTargetPos = m_ThrowMonster.transform.position;
    }

    public void SetThrowMonsterinfo(SkeletonBoomerang ThrowMonster)
    {
        m_ThrowMonster = ThrowMonster;
    }
}
