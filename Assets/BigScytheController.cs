using UnityEngine;
using System.Collections;

public class BigScytheController : MonoBehaviour
{
    private GameObject_Extension Extension;
    private float m_fAtkPower = 10f;
    private float m_fStartTime = 0f;
    private float m_fEndTime = 2f;

    private Transform m_MiddlePoint;
    public Transform MiddlePoint
    {
        get { return m_MiddlePoint; }
        set { m_MiddlePoint = value; }
    }

    public BigScytheController(Transform MiddlePointTrans, float fAtkPower)
    {
        MiddlePoint = MiddlePointTrans;
        m_fAtkPower = fAtkPower;
    }

    public void CopyInstance(BigScytheController origin)
    {
        MiddlePoint = origin.MiddlePoint;
        m_fAtkPower = origin.m_fAtkPower;
    }

    void Start()
    {
        m_fStartTime = Time.time;
        Extension =
            Mecro.MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
    }

    void Update()
    {
        float freqency = Time.deltaTime * 50f;
        transform.RotateAround(m_MiddlePoint.position, Vector3.forward, freqency);

        
        if(transform.rotation.eulerAngles.z > 120f)
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            Extension.SelfHide();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collider");
            Debug.Log(m_fAtkPower);
            PlayerCtrlManager.GetInstance().PlayerCtrl.Hp -= (int)m_fAtkPower;
        }
    }
}