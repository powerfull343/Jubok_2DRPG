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

    public Transform m_UsingSkills;

    void Awake()
    {
        CreateInstance();

        if (m_UsingSkills == null)
            m_UsingSkills = transform.FindChild("UsingSkill");
                
    }

    void OnEnable()
    {
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

    public void UseSkill(string SkillName,
        Transform Target = null)
    {
        if (MonsterManager.GetInstance().MonsterList.Count <= 0)
            return;

        Skill_Key_Extension FindKey = FindKeyInstance(SkillName);
        if (FindKey == null)
            Debug.LogError("Cannot Find Skill Effect");

        //스킬 사용이 등록되었나 여부 검사하고 사용이 되었을시 
        //스킬을 사용하지 못하게 막는다.
        if (FindKey.m_isSkillUsing == true)
        {
            Debug.Log(FindKey.m_isSkillUsing);
            return;
        }
                
        GameObject newSkillObject = 
            Instantiate(LoadedSkill[FindKey] as GameObject);

        //Position Setting
        newSkillObject.SetActive(false);
        newSkillObject.transform.parent = m_UsingSkills;
        //newSkillObject.transform.parent = m_BulletParent;
        //newSkillObject.transform.localPosition = Vector3.zero;
        //newSkillObject.transform.localScale = Vector3.one;
        

        Mecro.MecroMethod.CheckGetComponent<
            Skill_Interface>(newSkillObject).InitializingSkill(Target);

        newSkillObject.SetActive(true);

        //스킬을 사용했다고 등록
        SkillList_KeyUseSetting(FindKey, true);
    }
    
    public bool CheckingSkillUse(string SkillName)
    {
        if (SkillName == string.Empty)
            return false;

        Skill_Key_Extension FindKeyInfo = FindKeyInstance(SkillName);
        if (FindKeyInfo == null)
            Debug.LogError("Cannot Find Skill Effect");

        //Debug.Log(FindKeyInfo.m_nSkillManaCost);
        //Debug.Log(PlayerCtrlManager.GetInstance().PlayerCtrl.Mp);

        if (PlayerCtrlManager.GetInstance().PlayerCtrl.Mp -
            FindKeyInfo.m_nSkillManaCost < 0)
        {
            PlayerCtrlManager.GetInstance().PlayerCtrl.ShowEventTextMsg(
                "Not Enough Mana", Color.red);
            return false;
        }

        if (FindKeyInfo.m_isSkillUsing)
        {
            PlayerCtrlManager.GetInstance().PlayerCtrl.ShowEventTextMsg(
                "Skill Still Using", Color.red);
            return false;
        }
        
        return true;
    }

    public Skill_Key_Extension FindKeyInstance(string SkillName)
    {
        for (int i = 0; i < LoadedSkill.Count; ++i)
        {
            if (LoadedSkill.Keys.ToList()[i].m_SkillName == SkillName)
                return LoadedSkill.Keys.ToList()[i];
        }

        return null;
    }

    public void SkillList_KeyUseSetting(Skill_Key_Extension SkillKey, bool isUse)
    {
        SkillKey.m_isSkillUsing = isUse;
    }

    public void SkillList_KeyUseSetting(string SkillName, bool isUse)
    {
        Skill_Key_Extension FindKey = FindKeyInstance(SkillName);
        if (FindKey == null)
            Debug.LogError("Cannot Find Skill Effect");
        FindKey.m_isSkillUsing = isUse;
    }

    public void RemoveAllSkillData()
    {
        //Using Skill Data Clear
        m_UsingSkills.DestroyChildren();


        //Prefab Data Clear
        int nIndex = m_LoadedSkill.Count;
        for(int i = 0; i < nIndex; ++i)
            m_LoadedSkill.ToList().RemoveAt(i);
        m_LoadedSkill.Clear();
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