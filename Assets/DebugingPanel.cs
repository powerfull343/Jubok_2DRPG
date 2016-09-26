using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class DebugingPanel : MonoBehaviour {

    [SerializeField]
    private UISprite m_SpriteBg;

    [SerializeField]
    private UILabel m_Message;

    private Queue<GameObject> m_MessageQueue = 
        new Queue<GameObject>();

    private int m_nQueueMaxCount = 0;

    void Awake()
    {
        MecroMethod.CheckExistComponent<UISprite>(m_SpriteBg);
        MecroMethod.CheckExistComponent<UILabel>(m_Message);

        float fSize = m_SpriteBg.localSize.y / m_Message.localSize.y;
        m_nQueueMaxCount = (int)fSize;
    }

}
