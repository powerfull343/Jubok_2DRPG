using UnityEngine;
using System.Collections;
using System.Linq;

public class Battle_NGUI_DebugingCount : MonoBehaviour {

    [SerializeField]
    private UILabel m_DebugingText;

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(m_DebugingText);
        this.transform.localPosition =
            new Vector3((BattleScene_NGUI_Panel.fScreenWidth / -2f) + 150f,
                transform.localPosition.y, 0f);
    }

    void Update()
    {
        if (MonsterManager.GetInstance().MonsterList.Count == 0)
        {
            m_DebugingText.text = "Summoned MonType : " + MonsterManager.GetInstance().MonsterList.Count + "\n" +
                "Colled Mon : " + MagicianCtrl.ColMonsters.Count;
        }
        else
        {
            m_DebugingText.text = "Summoned MonType : " + MonsterManager.GetInstance().MonsterList.Count + "\n" +
                "Colled Mon : " + MagicianCtrl.ColMonsters.Count + "\n" +
                "MonsterCount : " + MonsterManager.MonsterCount;
        }
    }
}
