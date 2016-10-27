using UnityEngine;
using System.Collections;
using Mecro;

public class Battle_NGUI_LowerStat : MonoBehaviour
{
    [SerializeField]
    private UIWidget m_HpStatus;
    [SerializeField]
    private UIWidget m_MpStatus;
    [SerializeField]
    private UIWidget m_StatusBackGround;
    [SerializeField]
    private UIWidget m_BagUI;

    void Start()
    {
        MecroMethod.CheckExistComponent<UIWidget>(m_HpStatus);
        MecroMethod.CheckExistComponent<UIWidget>(m_MpStatus);
        MecroMethod.CheckExistComponent<UIWidget>(m_StatusBackGround);
        MecroMethod.CheckExistComponent<UIWidget>(m_BagUI);

        Vector2 vScreen = new Vector2(BattleScene_NGUI_Panel.fScreenWidth,
            BattleScene_NGUI_Panel.fScreenHeight);

        //m_HpStatus.SetDimensions((int)(Screen.width * 0.95f),
        //    (int)(Screen.height / 2.75f));
        //m_MpStatus.SetDimensions((int)(Screen.width * 0.95f),
        //    (int)(Screen.height / 2.75f));
        //m_StatusBackGround.SetDimensions(Screen.width, (int)(Screen.height / 3.5f));

        m_HpStatus.SetDimensions((int)(vScreen.x * 0.95f),
            (int)(vScreen.y / 2.75f));
        m_MpStatus.SetDimensions((int)(vScreen.x * 0.95f),
            (int)(vScreen.y / 2.75f));
        m_StatusBackGround.SetDimensions((int)vScreen.x, (int)(vScreen.y / 3.5f));


        float fheight = ((vScreen.y / 2f) - (m_StatusBackGround.localSize.y / 2f)) * -1f;

        transform.localPosition = new Vector3(0f, fheight, 0f);
    }


}
