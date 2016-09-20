using UnityEngine;
using System.Collections;

public class Death_Scythe_Rotation : MonoBehaviour {

    private Death m_MonsterBody;
    public Death MonsterBody
    {
        get { return m_MonsterBody; }
        set { m_MonsterBody = value; }
    }
    private ProgressTime m_innerTime;
    private int m_nRotationCount = 0;
    private bool m_isRotationCheck = false;
    private float m_fAtkPower = 1;
    public float AtkPower
    {
        get { return m_fAtkPower; }
        set { m_fAtkPower = value; }
    }

    public bool m_isLastScythe = false;

    void Start()
    {
        StartCoroutine("AutoRotation");
        MoveUpperScythe();
    }

    IEnumerator AutoRotation()
    {
        float fRotateDir = -1f;
        float fRotatePowerRange = 10f;
        while (true)
        {
            transform.Rotate(Vector3.forward * fRotateDir * fRotatePowerRange);
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    private void MoveUpperScythe()
    {
        float fDist = Vector3.zero.x - 
            PlayerCtrlManager.GetInstance().Player.position.x;
        Vector3 vTargetUpper = new Vector3(0f, fDist, 0f);

        TweenPosition.Begin(this.gameObject, 0.75f, vTargetUpper);
        StartCoroutine("AutoRotateAxis");
    }

    
    IEnumerator AutoRotateAxis()
    {
        yield return new WaitForSeconds(0.75f);
        float fStartTime = Time.time;
        float fEndTime = 10f;

        while(true)
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, 3f);
            //transform.RotateAround()

            if (Time.time - fStartTime > fEndTime)
                break;

            yield return new WaitForFixedUpdate();
        }

        if (m_MonsterBody)
            m_MonsterBody.m_ScytheSummoned = false;
        Destroy(this.gameObject);
        yield break;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Moveable_Object otherInfo = 
                Mecro.MecroMethod.CheckGetComponent<Moveable_Object>(other.gameObject);
            otherInfo.SetHp((int)AtkPower);
        }
    }
}
