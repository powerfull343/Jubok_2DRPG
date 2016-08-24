using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Selected_Animation : MonoBehaviour {

    private static List<Color> m_ColorList
        = new List<Color>();
    private static bool m_isColorSetting;

    void Start()
    {
        if (!m_isColorSetting)
        {
            m_ColorList.Add(new Color(0f, 0f, 0f, 0f));
            m_ColorList.Add(Color.black);
            m_ColorList.Add(Color.red);
            m_ColorList.Add(Color.yellow);
            m_ColorList.Add(Color.green);
            m_ColorList.Add(Color.cyan);
            m_ColorList.Add(Color.blue);
            m_ColorList.Add(Color.magenta);
            m_ColorList.Add(Color.white);
            m_ColorList.Add(Color.gray);
            m_ColorList.Add(new Color(0f, 0.5f, 0.5f, 0f));
            m_isColorSetting = true;
        }
        StartCoroutine("ColorAnimation");

    }

    IEnumerator ColorAnimation()
    {
        float fChangeDelayAmount = 0.2f;

        while (true)
        {
            for (int i = 0; i < m_ColorList.Count; ++i)
            {
                TweenColor.Begin(this.gameObject, fChangeDelayAmount, m_ColorList[i]);
                yield return new WaitForSeconds(fChangeDelayAmount);
            }
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
}