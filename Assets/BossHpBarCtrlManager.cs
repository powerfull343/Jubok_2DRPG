using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BossHpBarCtrlManager : MonoBehaviour { 

    private Dictionary<string, GameObject> m_HealthBarList
        = new Dictionary<string, GameObject>();

    private BossHpBar m_SummonedBossHpBar;
    public BossHpBar SummonedBossHpBar
    { get { return m_SummonedBossHpBar; } set { m_SummonedBossHpBar = value; } }

    private UIPanel m_OwnPanel;

    void Start()
    {
        //transform.localPosition 
        //    = new Vector3(0f, (Screen.height / 2f) - 110, 0f);

        //Debug.Log("ScreenWidth : " + Screen.width);
        //Debug.Log("Screenheight : " + Screen.height);
        //Debug.Log(transform.position);

        m_OwnPanel =
            Mecro.MecroMethod.CheckGetComponent<UIPanel>(this.transform);
        //CheckingMonsterList();
        
        Invoke("CheckingBossMonsterList", 0.5f);
    }

    private void CheckingBossMonsterList()
    {
        Dictionary<MonsterKey_Extension, LoadedMonsterElement> GetMonsterData =
            MonsterManager.FieldEliteMonsterData;

        int EliteCount = GetMonsterData.Count;
        for(int i = 0; i < EliteCount; ++i)
            AddHealthBarList(GetMonsterData.Values.ToList()[i].OriginInterfaceComp);

    }

    private void AddHealthBarList(Monster_Interface Monster)
    {
        if (Monster.grade < MONSTERGRADEID.GRADE_BOSS)
            return;

        string strKeyName = string.Empty;

        GameObject BossHealthBarPrefab = InitHealthBar(Monster, out strKeyName);
        if (BossHealthBarPrefab == null)
        {
            Debug.LogError("Cannot Load Data : " + Monster.LoadPrefabName);
            return;
        }

        m_HealthBarList.Add(strKeyName, BossHealthBarPrefab);
    }

    GameObject InitHealthBar(Monster_Interface Monster, out string HealthKeyName)
    {
        string FileSubName = "_BossHpBar";
        string HealthBarPath = "BattleScene/UI/MonsterHealthBar/";

        string FullPath = HealthBarPath + Monster.LoadPrefabName + FileSubName;

        HealthKeyName = Monster.LoadPrefabName;

        return Resources.Load(FullPath) as GameObject; 
    }

    public bool CreateHpBar(string MonsterKey)
    {
        GameObject CreateHpBarObject =
            Instantiate(m_HealthBarList[MonsterKey], Vector3.zero, Quaternion.identity) as GameObject;

        if (CreateHpBarObject == null)
        {
            Debug.Log(CreateHpBarObject.name + " Hp Bar is Null!");
            return false;
        }

        CreateHpBarObject.SetActive(false);

        //transform.localPosition 
        //    = new Vector3(0f, (Screen.height / 2f) - 110, 0f);

        //Debug.Log("ScreenWidth : " + Screen.width);
        //Debug.Log("Screenheight : " + Screen.height);
        //Debug.Log(transform.position);

        UIWidget BossHpWidget = 
            CreateHpBarObject.transform.FindChild(
                "Sprite - BackGround").GetComponent<UIWidget>();

        if (!BossHpWidget)
            Debug.LogError("Cannot Find " + BossHpWidget.name);

        float fHeight = (BattleScene_NGUI_Panel.fScreenHeight / 2f) -
            LobbyController.GetInstance().UpperStatusPanel.m_fUISize -
            (BossHpWidget.localSize.y / 2f);

        transform.localPosition
            = new Vector3(0f, fHeight , 0f);

        CreateHpBarObject.transform.parent = this.transform;
        CreateHpBarObject.transform.localScale = Vector3.one;
        CreateHpBarObject.transform.localPosition = Vector3.zero;
        m_SummonedBossHpBar =
            Mecro.MecroMethod.CheckGetComponent<BossHpBar>(CreateHpBarObject);
        CreateHpBarObject.SetActive(true);
        m_OwnPanel.Update();

        return true;
    }
}
