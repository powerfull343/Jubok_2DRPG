using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SkillManager : 
    Singleton<SkillManager>
{
    private Dictionary<Skill_Key_Extension, GameObject> m_LoadedSkill
        = new Dictionary<Skill_Key_Extension, GameObject>();
    public Dictionary<Skill_Key_Extension, GameObject> LoadedSkill
    {
        get { return m_LoadedSkill; }
    } 

    void Awake()
    {
        CreateInstance();
        LoadSkillList();
    }

    public void LoadSkillList()
    {
        //Class Choice

        //Prefab Loading

        //Test One Obejct
        AddSkillSet("EarthForce", false);
        AddSkillSet("Thunder02", false);
        AddSkillSet("PinPanel", false);
        AddSkillSet("CriticalLighting", true);
    }

    private void AddSkillSet(string SkillName, bool isSkillMelee)
    {
        string strFullPath = "BattleScene/Skills/Skill - " + SkillName;

        GameObject SkillObject = Resources.Load(strFullPath) as GameObject;
        if (!SkillObject)
            Debug.LogError("Cannot Find Skill - " + SkillName);

        Skill_Key_Extension KeyExtension = new Skill_Key_Extension();
        Skill_Interface SkillInfo = Mecro.MecroMethod.CheckGetComponent<Skill_Interface>(SkillObject);

        KeyExtension.m_SkillName = SkillName;
        KeyExtension.m_isSkillMelee = isSkillMelee;
        KeyExtension.m_isSkillUsing = false;
        KeyExtension.m_nSkillManaCost = SkillInfo.CostMana;

        LoadedSkill.Add(KeyExtension, SkillObject);
    }

    private bool CheckingExistMonster()
    {    
        if (MonsterManager.GetInstance().MonsterList.Count <= 0)
            return false;

        return true;
    }

    public void UseSkill(string SkillName,
        Transform Target = null)
    {
        if (!CheckingExistMonster())
        {
            SkillList_KeyUseSetting(SkillName, false);
            return;
        }

        Skill_Key_Extension FindKey = FindKeyInstance(SkillName);

        GameObject SkillObject = LoadedSkill[FindKey] as GameObject;
        SkillObject = Instantiate(SkillObject);
        
        //Position Setting
        SkillObject.SetActive(false);
        SkillObject.transform.parent = this.transform;
        //SkillObject.transform.position = transform.position;

        Mecro.MecroMethod.CheckGetComponent<Skill_Interface>(SkillObject).InitializingSkill(Target);

        SkillObject.SetActive(true);
    }
    
    public bool CheckingSkillUse(string SkillName)
    {
        if (SkillName == string.Empty)
            return false;

        Skill_Key_Extension FindKeyInfo = FindKeyInstance(SkillName);

        //Debug.Log(FindKeyInfo.m_nSkillManaCost);
        //Debug.Log(PlayerCtrlManager.GetInstance().PlayerCtrl.Mp);

        if (PlayerCtrlManager.GetInstance().PlayerCtrl.Mp -
            FindKeyInfo.m_nSkillManaCost < 0)
        {
            BattleScene_NGUI_Panel.GetInstance().CallEventMessage("Not Enough Mana", Color.red);
            return false;
        }

        if (FindKeyInfo.m_isSkillUsing)
        {
            BattleScene_NGUI_Panel.GetInstance().CallEventMessage("Skill Still Using", Color.red);
            return false;
        }

        //스킬 두번 나가는거 방지
        SkillList_KeyUseSetting(FindKeyInfo, true);
        return true;
    }

    public Skill_Key_Extension FindKeyInstance(string SkillName)
    {
        Skill_Key_Extension ResultKey = new Skill_Key_Extension();
        for (int i = 0; i < LoadedSkill.Count; ++i)
        {
            if (LoadedSkill.Keys.ToList()[i].m_SkillName == SkillName)
            {
                ResultKey = LoadedSkill.Keys.ToList()[i];
                break;
            }
        }
        return ResultKey;
    }

    public void SkillList_KeyUseSetting(Skill_Key_Extension SkillKey, bool isUse)
    {
        SkillKey.m_isSkillUsing = isUse;
    }

    public void SkillList_KeyUseSetting(string SkillName, bool isUse)
    {
        Skill_Key_Extension FindKey = FindKeyInstance(SkillName);
        FindKey.m_isSkillUsing = isUse;
    }

    //public void MonsterHpZero(Moveable_Object otherInfo)
    //{
    //    MagicianCtrl.ColMonsters.Remove(otherInfo.gameObject.GetComponent<Monster_Interface>());
    //    MonsterManager.GetInstance().RemoveMonster(otherInfo.ObjectName,
    //        otherInfo.transform.parent.gameObject);
    //}
}

public class Skill_Key_Extension
{
    public string m_SkillName;
    public bool m_isSkillMelee;
    public bool m_isSkillUsing;
    public int m_nSkillManaCost;

}