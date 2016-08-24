using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetailStat_ClassInfo : MonoBehaviour
{
    private List<Transform> m_StatList
        = new List<Transform>();

    [SerializeField]
    private Transform m_GridTransform;
    
    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<Transform>(m_GridTransform);
        AddStatList();
    }

    void AddStatList()
    {
        m_StatList.Add(m_GridTransform.FindChild("Info - StatExp"));
        m_StatList.Add(m_GridTransform.FindChild("Info - ClassExp"));
        m_StatList.Add(m_GridTransform.FindChild("Info - SubJobExp"));
        //m_StatList.Add(m_GridTransform.FindChild("DetailInfo - StatExp"));
        //m_StatList.Add(m_GridTransform.FindChild("DetailInfo - ClassInfo"));
        //m_StatList.Add(m_GridTransform.FindChild("DetailInfo - SubJobinfo"));
    }

    public void MoveOtherElements(int choiceIndex, bool isShowDown, float SizeY)
    {
        Debug.Log("Move");
        float fSizeY = (isShowDown == true) ? -SizeY : SizeY;
        //float nMaxCount = (isShowDown == true) ? m_StatList.Count / 2 : m_StatList.Count;

        for (int i = choiceIndex + 1; i < m_StatList.Count; ++i)
            m_StatList[i].localPosition += new Vector3(0f, fSizeY, 0f);
    }
}	
