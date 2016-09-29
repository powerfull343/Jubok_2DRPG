using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public bool isShaked = false;

    private GameObject Environment;

    private Dictionary<string, Environment_Element> Elements =
        new Dictionary<string, Environment_Element>();

    void OnEnable()
    {
        //BackGround Hide
        DebugingObject.SetActive(false);

        Vector3 OverWorld = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));

        WorldScreenWidth = OverWorld.x;
        WorldScreenHeight = OverWorld.y;

        LoadResource();
        SetEnvironment_Variables();
    }

    void LoadResource()
    {
        //choice battle field type
        //You Must Setting BG, MG, FG

        //Test Func
        Environment = Instantiate(Resources.Load(
            "BattleScene/Environments/GrassField_Environment") as GameObject);

        MecroMethod.CheckExistObject<GameObject>(Environment);

        Environment.transform.SetParent(transform);
    }

    void SetEnvironment_Variables()
    {
        Elements.Add("BackGround", ImportObject_ToCollection("BackGround"));
        Elements.Add("MidGround", ImportObject_ToCollection("MidGround"));
        Elements.Add("ForeGress", ImportObject_ToCollection("ForeGress"));
        Elements.Add("BackGress", ImportObject_ToCollection("BackGress"));
    }

    Environment_Element ImportObject_ToCollection(string childName)
    {
        Environment_Element element = new Environment_Element();
        Transform Parent = Environment.transform.FindChild(childName);
        MecroMethod.CheckExistObject<Transform>(Parent);

        Transform Grid = Parent.FindChild("Grid");
        MecroMethod.CheckExistObject<Transform>(Grid);

        element.ElementGrid = MecroMethod.CheckGetComponent<UIGrid>(Grid);

        int ChildCnt = Grid.childCount;

        for (int i = 0; i < ChildCnt; ++i)
        {
            if (i == ChildCnt - 1)
                element.ElementLastNode = Grid.GetChild(i);

            element.Element_EnQueue(Grid.GetChild(i));
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
                in Elements)
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
    public void Element_EnQueue(Transform item)
    {
        Element_List.Add(item);
    }

    public void Element_DeQueue()
    {
        Element_List.RemoveAt(Element_List.Count - 1);
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