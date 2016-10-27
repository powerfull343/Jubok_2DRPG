using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

using LobbyButtonFunc;

public class NGUI_PanelManager :
    Singleton<NGUI_PanelManager>
{
    [SerializeField]
    private VilageScene_NGUI_Panel m_VilageScenePanel;
    [SerializeField]
    private BattleScene_NGUI_Panel m_BattleScenePanel;

    private Dictionary<FIELDID, Scene_Panel_Interface> m_PanelContainer =
        new Dictionary<FIELDID, Scene_Panel_Interface>();

	// Use this for initialization
    void Awake()
    {
        CreateInstance();
        CheckingExistComps();
        InitScenePanelContainers();
    }

    private void CheckingExistComps()
    {
        m_VilageScenePanel =
            MecroMethod.CheckGetComponent<VilageScene_NGUI_Panel>(
        LobbyController.GetInstance().GetLobbyPanel);
        m_BattleScenePanel =
            MecroMethod.CheckGetComponent<BattleScene_NGUI_Panel>(
        LobbyController.GetInstance().GetBattlePanel.FindChild("NGUIPanel - UI Root"));
    }

    private void InitScenePanelContainers()
    {
        m_PanelContainer.Add(FIELDID.ID_VILAGE, m_VilageScenePanel);
        m_PanelContainer.Add(FIELDID.ID_BATTLEFIELD01, m_BattleScenePanel);
    }

    public Scene_Panel_Interface GetCurrentScenePanel()
    {
        if(m_PanelContainer[
            LobbyController.GetInstance().mCurrentSceneID].gameObject == null)
        {
            Debug.LogError("Panel Object is null");
            return null;
        }

        return m_PanelContainer[
            LobbyController.GetInstance().mCurrentSceneID];
    }

    public BattleScene_NGUI_Panel GetBattleScenePanel()
    {
        if(m_PanelContainer[FIELDID.ID_BATTLEFIELD01].gameObject == null)
        {
            Debug.LogError("BattleScene Panel Object is Null");
            return null;
        }

        return (BattleScene_NGUI_Panel)m_PanelContainer[FIELDID.ID_BATTLEFIELD01];
    }

    public VilageScene_NGUI_Panel GetVilageScenePanel()
    {
        if (m_PanelContainer[FIELDID.ID_VILAGE].gameObject == null)
        {
            Debug.LogError("BattleScene Panel Object is Null");
            return null;
        }

        return (VilageScene_NGUI_Panel)m_PanelContainer[FIELDID.ID_VILAGE];
    }
	
}
