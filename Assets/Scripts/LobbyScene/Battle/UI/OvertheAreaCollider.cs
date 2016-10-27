using UnityEngine;
using System.Collections;

public class OvertheAreaCollider : MonoBehaviour {

    private BoxCollider m_BoxCol;
    public COLLIDERDIRID m_ColDir;
    public bool m_isGrass = false;

    void Start()
    {
        m_BoxCol = Mecro.MecroMethod.CheckGetComponent<BoxCollider>(gameObject);

        switch (m_ColDir)
        {
            case COLLIDERDIRID.COL_LEFT:
                transform.position =
                    new Vector3(-EnvironmentManager.WorldScreenWidth, 0f, 0f);
                m_BoxCol.size = new Vector3(
                    0.5f, EnvironmentManager.WorldScreenHeight * 2f, 1f);
                break;

            case COLLIDERDIRID.COL_RIGHT:
                transform.position =
                    new Vector3(EnvironmentManager.WorldScreenWidth, 0f, 0f);
                m_BoxCol.size = new Vector3(
                    0.5f, EnvironmentManager.WorldScreenHeight * 2f, 1f);
                break;

            case COLLIDERDIRID.COL_UPPER:
                transform.position =
                    new Vector3(0f, EnvironmentManager.WorldScreenHeight, 0f);
                m_BoxCol.size = new Vector3(
                    EnvironmentManager.WorldScreenWidth * 2f, 0.5f, 1f);
                break;

            case COLLIDERDIRID.COL_LOWER:
                if (!m_isGrass)
                {
                    transform.position =
                        new Vector3(0f, -EnvironmentManager.WorldScreenHeight, 0f);
                    m_BoxCol.size = new Vector3(
                        EnvironmentManager.WorldScreenWidth * 2f, 0.5f, 1f);
                }
                break;
        }
    }
}
