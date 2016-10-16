using UnityEngine;
using System.Collections;

public class Scene_Panel_Interface : MonoBehaviour {

    public static float fScreenWidth = 1280;
    public static float fScreenHeight = 720;

    protected GameObject m_TempCollider;
    protected UIPanel m_TempColliderComp;

    protected virtual void InitComponents()
    {
        if (!m_TempCollider)
        {
            m_TempCollider =
                Instantiate(Resources.Load(
                    "UIPanels/Panel - CannotControl") as GameObject);
            Mecro.MecroMethod.SetPartent(m_TempCollider.transform,
                this.transform);
        }
    }

    protected virtual void UpperPanelMoving()
    {
        LobbyController.GetInstance().UpperStatusPanel.MovingUpperStatusUI(
                this.transform, fScreenHeight);
    }

    public void OpenBehindCollider()
    {
        m_TempCollider.SetActive(true);
    }

    public virtual void CloseBehindCollider()
    {
        m_TempCollider.SetActive(false);
    }

    public void SetBehindColliderDepth(int ChangeDepthAmount)
    {
        if (!m_TempColliderComp)
        {
            m_TempColliderComp =
                Mecro.MecroMethod.CheckGetComponent<UIPanel>(m_TempCollider);
        }

        m_TempColliderComp.depth = ChangeDepthAmount;
        m_TempColliderComp.Update();
    }

}
