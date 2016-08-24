using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Bullet_Extension : GameObject_Extension {

    public bool m_isAutoRotation = false;

    public float m_AtkPower = 1f;
    public Moveable_Type m_TargetType;
    public float m_Speed = 4f;

    private string m_TargetTagName;
    private Vector3 m_vDir = new Vector3(1f, 0f, 0f);
    public Vector3 vDirection
    {
        get { return m_vDir; }
        set { m_vDir = value; }
    }
    private Vector3 m_TargetPosition;

    public bool m_isCollisionDestroy = true;

    public BULLETTYPEID mBulletID;

    //Curving
    public float m_fCurvingJump = 2f;
    public float m_fCurvingEndTime = 2f;

    private float m_fCurvingStartTime;
    private Vector3 m_vCurvingCenter;
    private Vector3 m_vCurvingStartPos;
    private bool m_isEndCurving = false;
    

    void Start()
    {
        if (m_isAutoRotation)
            StartCoroutine("AutoRotation");

        BulletInitalizing();
        
    }

    void BulletInitalizing()
    {
        switch (m_TargetType)
        {
            case Moveable_Type.TYPE_PLAYER:
                m_TargetTagName = "Player";
                m_vDir = new Vector3(-1f, 0f, 0f);
                m_TargetPosition = PlayerCtrlManager.GetInstance().transform.position;
                if (mBulletID == BULLETTYPEID.BULLET_STRAIGHT_TOANGLE)
                    m_vDir = Vector3.Normalize(m_TargetPosition - transform.position);
                
                break;
            case Moveable_Type.TYPE_MONSTER:
                m_TargetTagName = "Monster";
                m_vDir = new Vector3(1f, 0f, 0f);
                break;
        }

        //Curving
        if(mBulletID != BULLETTYPEID.BULLET_STRAIGHT &&
            mBulletID != BULLETTYPEID.BULLET_STRAIGHT_TOANGLE)
            CurvingBulletInit();
    }

    void CurvingBulletInit()
    {
        m_vCurvingStartPos = transform.position;
        m_fCurvingStartTime = Time.time;

        m_vCurvingCenter = ((m_vCurvingStartPos + m_TargetPosition) * 0.5f) + new Vector3(0f, m_fCurvingJump, 0f);
    }

    void Update()
    {
        if (mBulletID == BULLETTYPEID.BULLET_STRAIGHT ||
            mBulletID == BULLETTYPEID.BULLET_STRAIGHT_TOANGLE)
            Straight();
        else
            Curving();
    }

    void Straight()
    {
        if (mBulletID == BULLETTYPEID.BULLET_STRAIGHT)
            transform.position += m_vDir * 0.03f * m_Speed;
        else
        {
            transform.localPosition += m_vDir * m_Speed;
        }
    }

    void Curving()
    {
        float frecComplete = (Time.time - m_fCurvingStartTime) / m_fCurvingEndTime;

        if (frecComplete >= 1f)
        {
            m_isEndCurving = true;
            return;
        }

        Vector3 StartRelCenter = Vector3.Lerp(m_vCurvingStartPos, m_vCurvingCenter, frecComplete);
        Vector3 DropRelCenter = Vector3.Lerp(m_vCurvingCenter, m_TargetPosition, frecComplete);
        transform.localPosition = Vector3.Lerp(StartRelCenter, DropRelCenter, frecComplete);
    }

    IEnumerator AutoRotation()
    {
        float fRotatePowerRange = Random.Range(2f, 5f);
        float RotateDir =
            Random.Range(0, 1) == 0 ? -1f : 1f;

        while(this.gameObject != null)
        {
            transform.Rotate(Vector3.forward * RotateDir * fRotatePowerRange);

            if (m_isEndCurving)
                break;

            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(m_TargetTagName))
        {
            Moveable_Object otherInfo = Mecro.MecroMethod.CheckGetComponent<
                Moveable_Object>(other.gameObject);
            //공격전에 0이 된걸 확인.
            if (otherInfo.Hp <= 0)
                return;

            otherInfo.SetHp((int)m_AtkPower);
            //공격하다가 0이됨.
            //if (otherInfo.Hp <= 0)
            //{
            //    switch(m_TargetTagName)
            //    {
            //        case "Monster":
            //            //if Monster Colliding To RangeAtkDam
            //            MagicianCtrl.ColMonsters.Remove(other.gameObject.GetComponent<Monster_Interface>());
            //            MonsterManager.GetInstance().RemoveMonster(otherInfo.ObjectName,
            //                other.transform.parent.gameObject);
            //            break;

            //        case "Player":
            //            SelfDestroy();
            //            break;

            //        default:
            //            break;
            //    }
            //}
            //몬스터 HP 0일시 관통 그 외에는 삭제
            if (m_isCollisionDestroy)
                SelfDestroy();
        }

        //영역 초과시 삭제
        if (other.gameObject.CompareTag("OverTheArea"))
            SelfDestroy();
    }
	
}
