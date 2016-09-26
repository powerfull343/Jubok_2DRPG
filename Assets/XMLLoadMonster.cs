using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using LobbyManager;

public class XMLLoadMonster : MonoBehaviour {

    void Start()
    {
        LoadXML();
    }

    private string ConvertFieldIDToXMLName(FIELDID Fieldid)
    {
        string result = "";

        switch (Fieldid)
        {
            case FIELDID.ID_BATTLEFIELD01:
                result = "BattleField01.xml";
                break;

            case FIELDID.ID_BATTLEFIELD02:
                break;

            case FIELDID.ID_CASTLE:
                break;

            case FIELDID.ID_DONGEON:
                break;

            case FIELDID.ID_SUDDENATTACK:
                break;

            case FIELDID.ID_VILAGE:
                break;

            default:
                Debug.Log("invaild Type");
                break;
        }

        return result;
    }


    private void LoadXML()
    {
        string XMLFileName
            = ConvertFieldIDToXMLName(
                LobbyController.GetInstance().mCurrentSceneID);

        Debug.Log(LobbyController.GetInstance().mCurrentSceneID);

        XmlDocument xmldocu = new XmlDocument();

        string XMLFileFullPath = Environment.CurrentDirectory +
            "\\Assets\\XML_LIST\\" + XMLFileName;

        xmldocu.Load(XMLFileFullPath);
        XmlNodeList nodeList = xmldocu.DocumentElement.SelectNodes("/Field/Monster");

        MonsterKey_Extension KeyExtension;
        foreach(XmlNode node in nodeList)
        {
            MONSTERGRADEID gradeid = MONSTERGRADEID.GRADE_ERROR;
            LoadedMonsterElement monster = Monster_Interface.ConvertXMLToMonster(node, out KeyExtension, out gradeid);

            if (gradeid == MONSTERGRADEID.GRADE_NORMAL)
                MonsterManager.FieldMonsterData.Add(KeyExtension, monster);
            else if(gradeid == MONSTERGRADEID.GRADE_HIDDEN)
                MonsterManager.FieldSpecialMonsterData.Add(KeyExtension, monster);
            else
            {
                MonsterManager.FieldEliteMonsterData.Add(KeyExtension, monster);
            }
        }

        //MonsterManager.FieldMonsterData.Add("MonsterName", class Monster_InterFace);
    }
}

public class LoadedMonsterElement
{
    private GameObject _OriginGameObject;
    public GameObject OriginGameObject
    {
        get { return _OriginGameObject; }
        set { _OriginGameObject = value; }
    }

    private Monster_Interface _OriginInterfaceComp;
    public Monster_Interface OriginInterfaceComp
    {
        get { return _OriginInterfaceComp; }
        set
        {
            _OriginInterfaceComp = value;
            _OriginInterfaceType = OriginInterfaceComp.GetType();
        }
    }

    private Type _OriginInterfaceType;
    public Type OriginInterfaceType
    {
        get { return _OriginInterfaceType; }
        set { _OriginInterfaceType = value; }
    }

}