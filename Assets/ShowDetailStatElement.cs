using UnityEngine;
using System.Collections;
using System.Reflection;

public class ShowDetailStatElement : MonoBehaviour {

    [SerializeField]
    private PLAYERSTATKINDSID m_KindsOfStat;
    [SerializeField]
    private string m_StatName;
    [SerializeField]
    private UILabel m_StatLabel;
    [SerializeField]
    private UISprite m_StatExpRenderer;

    void Start()
    {
        PlayerData LoadedPlayerStat = DataController.GetInstance().InGameData;
        InitKindStat(LoadedPlayerStat);
    }

    private void InitKindStat(PlayerData _LoadedPlayerStat)
    {
        if(_LoadedPlayerStat == null)
        {
            Debug.LogError(_LoadedPlayerStat.GetType().ToString() + " Object is Null!");
            return;
        }

        switch(m_KindsOfStat)
        {
            case PLAYERSTATKINDSID.STAT_BASIC:
                CheckBasicStat(_LoadedPlayerStat);
                break;

            case PLAYERSTATKINDSID.STAT_CLASS:
                break;

            case PLAYERSTATKINDSID.STAT_CRAFTMAN:
                break;
        }
    }

    private void CheckBasicStat(PlayerData _LoadedPlayerStat)
    {
        if (m_StatName == "Str")
        {
            m_StatLabel.text = _LoadedPlayerStat.tStat.Str.ToString();
            m_StatExpRenderer.fillAmount =
                _LoadedPlayerStat.tStat.StrengthExp / 
                (100f * _LoadedPlayerStat.tStat.Str);
                
        }
        else if (m_StatName == "Dex")
        {
            m_StatLabel.text = _LoadedPlayerStat.tStat.Dex.ToString();
            m_StatExpRenderer.fillAmount =
                _LoadedPlayerStat.tStat.DexterityExp /
                (100f * _LoadedPlayerStat.tStat.Dex);

        }
        else if (m_StatName == "Int")
        {
            m_StatLabel.text = _LoadedPlayerStat.tStat.Int.ToString();
            m_StatExpRenderer.fillAmount =
                _LoadedPlayerStat.tStat.IntelligenceExp /
                (100f * _LoadedPlayerStat.tStat.Int);

        }
    }
}
