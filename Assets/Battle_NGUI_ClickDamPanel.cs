using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battle_NGUI_ClickDamPanel : MonoBehaviour {

    [SerializeField]
    private UIWidget m_ClickButton;
    [SerializeField]
    private Battle_NGUI_ClickDamPanel_HitEffects m_EffectContainers;

    public static int nEffectCount = 0;

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UIWidget>(m_ClickButton);
        Mecro.MecroMethod.CheckExistComponent<
            Battle_NGUI_ClickDamPanel_HitEffects>(m_EffectContainers);
        SetAtkClickButtonCollider();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            ClickAtkButton();

        if (nEffectCount <= 0)
            EnvironmentManager.isBoosting = false;
    }

    void SetAtkClickButtonCollider()
    {
        m_ClickButton.SetDimensions(Screen.width, Screen.height);
    }

    public void ClickAtkButton()
    {
        if(UICamera.hoveredObject == m_ClickButton.gameObject)
        {
            CreateButtonEffect();
            ClickButtonEvent();
        }
    }

    private void CreateButtonEffect()
    {
        GameObject CalledHitEffect = Instantiate(m_EffectContainers.GetRandomHitEffect()) as GameObject;

        CalledHitEffect.SetActive(false);

        CalledHitEffect.transform.parent = m_EffectContainers.transform;
        CalledHitEffect.transform.localScale = Vector3.one;
        CalledHitEffect.transform.position =
            BattleScene_NGUI_Panel.GetInstance(
                ).NGUICamera.ScreenToWorldPoint(
                Input.mousePosition);

        CalledHitEffect.SetActive(true);
    }

    private void ClickButtonEvent()
    {
        if(MonsterManager.MonsterCount > 0)
        {
            EnvironmentManager.isBoosting = false;
            MonsterManager.AttackFirstSummonedMonster();
        }
        else
        {
            EnvironmentManager.isBoosting = true;
        }
    }


    
}
