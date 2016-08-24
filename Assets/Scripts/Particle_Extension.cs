using UnityEngine;
using System.Collections;

public class Particle_Extension : GameObject_Extension {

    //Cannot Control Variable
    private float m_fCurvingStartTime;

    private Vector3 m_vCurvingStartPos;
    private Vector3 m_vCurvingEndPosition;
    private Vector3 m_vCurvingCenterPos;

    private bool m_isEndCurving;

    //Control Variable
    private float m_fCurvingEndTime = 1.5f;
    private float m_fJumpingPower = 0.5f;

    [SerializeField]
    private bool m_isRotation = false;

    // Use this for initialization
    void Start () {
        //======Init======//
        m_vCurvingStartPos = transform.localPosition;
        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(0f, 0.25f);
        float RandomJump = Random.Range(m_fJumpingPower, (m_fJumpingPower + 0.5f));
        m_fCurvingEndTime = Random.Range(m_fCurvingEndTime, m_fCurvingEndTime + 0.5f);

        m_vCurvingEndPosition = new Vector3(x, y, 0f);
        m_vCurvingCenterPos = ((m_vCurvingStartPos + m_vCurvingEndPosition) * 0.5f) +
             new Vector3(0f, RandomJump, 0f);

        m_fCurvingStartTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (!m_isEndCurving)
        {
            MovingParticle();
            Rotation();
        }
    }

    void MovingParticle()
    {
        float frecComplete = (Time.time - m_fCurvingStartTime) / m_fCurvingEndTime;

        if (frecComplete >= 1f)
        {
            m_isEndCurving = true;
            return;
        }

        Vector3 StartRelCenter = Vector3.Lerp(m_vCurvingStartPos, m_vCurvingCenterPos, frecComplete);
        Vector3 DropRelCenter = Vector3.Lerp(m_vCurvingCenterPos, m_vCurvingEndPosition, frecComplete);
        transform.localPosition = Vector3.Lerp(StartRelCenter, DropRelCenter, frecComplete);
    }

    void Rotation()
    {
        if (!m_isRotation)
            return;

        float fRotatePowerRange = Random.Range(5f, 30f);
        float RotateDir = Random.Range(0, 1) == 0 ? -1f : 1f;

        transform.Rotate(Vector3.forward * RotateDir * fRotatePowerRange);
    }
}
