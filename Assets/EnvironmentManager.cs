using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mecro;

public class EnvironmentManager :
    Singleton_Parent<EnvironmentManager> {

    //Debuging Object
    public GameObject DebugingObject;

    public static float WorldScreenWidth;
    public static float WorldScreenHeight;

    public static bool isMoved = true;
    public static bool isBoosting = false;
    public static float OriginMovingSpeed = 0.05f;
    public static float SpeedAmount = 1f;
    public static float fMidGroundHeight;

    public bool isShaked = false;

    private GameObject m_EnvironmentObject;

    private Dictionary<string, Environment_Element> m_GroundElements =
        new Dictionary<string, Environment_Element>();
    public Dictionary<string, Environment_Element> GroundElements
    {
        get { return m_GroundElements; }
    }

    void OnEnable()
    {
        //BackGround Hide
        DebugingObject.SetActive(false);

        Vector3 OverWorld = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));

        WorldScreenWidth = OverWorld.x;
        WorldScreenHeight = OverWorld.y;

        LoadResource();
        SetEnvironment_Variables();
        SetGroundHeight("MidGround");
    }

    void LoadResource()
    {
        //choice battle field type
        //You Must Setting BG, MG, FG

        //Test Func
        m_EnvironmentObject = Instantiate(Resources.Load(
            "BattleScene/Environments/GrassField_Environment") as GameObject);

        MecroMethod.CheckExistObject<GameObject>(m_EnvironmentObject);

        m_EnvironmentObject.transform.SetParent(transform);
    }

    void SetEnvironment_Variables()
    {
        m_GroundElements.Add("BackGround", ImportObject_ToCollection("BackGround"));
        m_GroundElements.Add("MidGround", ImportObject_ToCollection("MidGround"));
        m_GroundElements.Add("ForeGress", ImportObject_ToCollection("ForeGress"));
        m_GroundElements.Add("BackGress", ImportObject_ToCollection("BackGress"));
    }

    private void SetGroundHeight(string GroundName)
    {
        Transform GridPosition = m_GroundElements[GroundName].ElementGrid.transform;
        fMidGroundHeight = GridPosition.localPosition.y;
    }

    Environment_Element ImportObject_ToCollection(string childName)
    {
        Environment_Element element = new Environment_Element();
        Transform Parent = m_EnvironmentObject.transform.FindChild(childName);
        MecroMethod.CheckExistObject<Transform>(Parent);

        Transform Grid = Parent.FindChild("Grid");
        MecroMethod.CheckExistObject<Transform>(Grid);

        element.ElementGrid = MecroMethod.CheckGetComponent<UIGrid>(Grid);

        int ChildCnt = Grid.childCount;

        for (int i = 0; i < ChildCnt; ++i)
        {
            if (i == ChildCnt - 1)
                element.ElementLastNode = Grid.GetChild(i);

            element.AddElement(Grid.GetChild(i));
        }
        MecroMethod.CheckExistObject<Transform>(
            element.ElementLastNode);

        if (element.ElementList.Count <= 0)
        {
            Debug.Log("Cannot find Data Set");
            return null;
        }

        return element;
    }

    void Update()
    {
        if (isMoved)
        {
            foreach (KeyValuePair<string, Environment_Element> element
                in m_GroundElements)
            {
                element.Value.MoveElements();
            }
        }

        if (isShaked)
            ShakingAllArea();
        else
            transform.localPosition = Vector3.zero;
    }

    void ShakingAllArea()
    {
        transform.localPosition = new Vector3(
            Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
    }

    public void ClearAllEnvironmentElements()
    {
        string DeleteKeyTarget = string.Empty;
        Environment_Element DeleteValueTarget = null;
        int nEnvironmentLength = m_GroundElements.Count;

        for(int i = 0; i < nEnvironmentLength; ++i)
        {
            DeleteValueTarget = m_GroundElements.ToList()[i].Value;
            DeleteValueTarget.RemoveAllClassFunc();
            m_GroundElements.ToList().RemoveAt(i);
            Destroy(DeleteValueTarget);
        }
        m_GroundElements.Clear();
        Destroy(m_EnvironmentObject);
    }
}

public class Environment_Element
    : MonoBehaviour
{
    private List<Transform> Element_List 
        = new List<Transform>();
    public List<Transform> ElementList
    {
        get { return Element_List; }
    }
    public void AddElement(Transform _Element)
    {
        Element_List.Add(_Element);
    }

    public void RemoveElement(Transform _Element)
    {
        Element_List.Remove(_Element);
    }

    public void RemoveAllClassFunc()
    {
        Transform DeleteTarget = null;
        while(Element_List.Count > 0)
        { 
            DeleteTarget = Element_List[0];
            Element_List.Remove(DeleteTarget);
            Destroy(DeleteTarget.gameObject);
        }
        Element_List.Clear();
        Destroy(Element_Grid.gameObject);
    }

    private UIGrid Element_Grid = new UIGrid();
    public UIGrid ElementGrid
    {
        get { return Element_Grid; }
        set { Element_Grid = value; }
    }
    private Transform Element_LastNode;
    public Transform ElementLastNode
    {
        get { return Element_LastNode; }
        set { Element_LastNode = value; }
    }

    public void MoveElements()
    {
        float fListCount = Element_List.Count;

        float fBoostSpeed = 1f;
        if (EnvironmentManager.isBoosting)
            fBoostSpeed = 2f;

        for (int i = 0; i < fListCount; ++i)
        {
            Element_List[i].localPosition -= new Vector3(
                EnvironmentManager.OriginMovingSpeed * EnvironmentManager.SpeedAmount * fBoostSpeed,
                0f, 0f);

            float OutofRange = -EnvironmentManager.WorldScreenWidth - Element_Grid.cellWidth;

            if (Element_List[i].localPosition.x <= OutofRange)
                MoveNodeToRight(Element_List[i]);
        }
    }

    void MoveNodeToRight(Transform OutTrans)
    {
        //1. 맨 마지막 노드값이 제일 오른쪽에 있으므로
        //오른쪽에 있는 노드에서 일정 거리만큼 플러스로 계산한다.
        Vector3 vNewPos = new Vector3(
            (Element_LastNode.position.x + Element_Grid.cellWidth),
            0f, 0f);
        OutTrans.localPosition = vNewPos;

        //2. 위치가 바뀐 노드를 맨 마지막 노드로 등록한다.
        Element_LastNode = OutTrans;

        ////1. 화면 밖으로 나간 노드를 찾아 Dequeue를 실시한다.
        //Transform OutTrans = Element_List.Dequeue();

        ////2. 맨 마지막 노드값이 제일 오른쪽에 있으므로
        ////오른쪽에 있는 노드에서 일정 거리만큼 플러스로 계산한다.
        //Vector3 vNewPos = new Vector3(
        //    (Element_LastNode.position.x + Element_Grid.cellWidth),
        //    Element_LastNode.position.y, Element_LastNode.position.z);
        //OutTrans.localPosition = vNewPos;

        ////3. Dequeue를 실시한 노드를 마지막 노드로 등록한다.
        //Element_LastNode = OutTrans;

        ////4. Dequeue를 실시한 노드를 컨테이너에 다시 삽입한다.
        //Element_List.Enqueue(Element_LastNode);
    }
}