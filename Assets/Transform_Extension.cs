using UnityEngine;
using System.Collections;

public class Transform_Extension : MonoBehaviour {

    /// <summary>
    /// -1 ~ 1
    /// </summary>
    public float fX;
    /// <summary>
    /// -1 ~ 1
    /// </summary>
    public float fY;
    public bool m_isLocalPositionSetting = false;
    public Camera m_SubCamera;

	void Start()
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        SetPositionSetting();
    }

    public void SetPositionSetting()
    {
        Vector3 Position;
        if (m_SubCamera == null)
            Position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)); 
        else
            Position = m_SubCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        if (!m_isLocalPositionSetting)
            transform.position = new Vector3(Position.x * fX, Position.y * fY, 0f);
        else
            transform.localPosition = new Vector3(Position.x * fX, Position.y * fY, 0f);
    }
}
