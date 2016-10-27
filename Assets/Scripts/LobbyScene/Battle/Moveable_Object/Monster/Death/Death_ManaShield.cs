using UnityEngine;
using System.Collections;

public class Death_ManaShield : MonoBehaviour {

    private int Hp = 10;
    private SpriteRenderer m_RenderComp;

    void Start()
    {
        m_RenderComp = Mecro.MecroMethod.CheckGetComponent<SpriteRenderer>(this.gameObject);
        m_RenderComp.material.color = new Color(1f, 1f, 1f, 0f);
    }

    IEnumerator StartShieldEffect()
    {
        bool isUpper = true;
        float fAmount = 0.1f, fAlphaValue = 0f;

        while(true)
        {
            if(isUpper)
            {
                fAlphaValue += fAmount;
                fAlphaValue = Mathf.Min(1f, fAlphaValue);
                m_RenderComp.material.color = new Color(1f, 1f, 1f, fAlphaValue);
                if (fAlphaValue >= 1f)
                {
                    isUpper = false;
                    fAmount *= -1f;
                }
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                fAlphaValue += fAmount;
                fAlphaValue = Mathf.Max(0f, fAlphaValue);
                m_RenderComp.material.color = new Color(1f, 1f, 1f, fAlphaValue);
                if (fAlphaValue <= 0f)
                    break;
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield break;
    }

	void OnTriggerEnter(Collider other)
    {
        Bullet_Extension BulletInfo = other.gameObject.GetComponent<Bullet_Extension>();
        if (BulletInfo == null)
            return;

        if(BulletInfo.m_TargetType == Moveable_Type.TYPE_MONSTER)
        {
            //Ahead Direction
            BulletInfo.vDirection = new Vector3(-1f, Random.Range(0.5f, 1f), 0f);

            //Rotation
            float fCosAngle = Mecro.MecroMethod.GetAngle(Vector3.right, BulletInfo.vDirection.normalized);
            BulletInfo.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, fCosAngle));

            --Hp;
            StartCoroutine("StartShieldEffect");
        }

        if (Hp <= 0)
            Destroy(this.gameObject);
    }
}
