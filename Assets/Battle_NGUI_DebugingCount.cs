using UnityEngine;
using System.Collections;

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
        m_DebugingText.text = "Summoned Mon : " + MonsterManager.GetInstance().MonsterList.Count + "\n" + 
            "Colled Mon : " + MagicianCtrl.ColMonsters.Count;
    }

}
