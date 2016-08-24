using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mecro;

public class IntroSceneManager : MonoBehaviour {

    [SerializeField]    
    private GameObject[] DisableGameObjects;
    [SerializeField]
    private GameObject SubCam;
    [SerializeField]
    private UIWidget m_BackGround;
    [SerializeField]
    private UILabel m_TestConsole;


    void Start()
    {
        MecroMethod.CheckExistComponent<UIWidget>(m_BackGround);
        MecroMethod.CheckExistComponent<UILabel>(m_TestConsole);
        m_BackGround.SetDimensions(1280, 720);
    }

    public void StartButtonClick()
    {
        foreach(GameObject obj in DisableGameObjects)
        {
            if (!obj.GetComponent<TweenAlpha>().isActiveAndEnabled)
                obj.GetComponent<TweenAlpha>().enabled = true;
            //Alpha == 0 Object Delete
        }

        StartCoroutine("CheckContainerEmpty");
    }

    public IEnumerator CheckContainerEmpty()
    {
        while(true)
        {
            bool CheckObjects = true;

            foreach (GameObject obj in DisableGameObjects)
            {
                if (obj != null)
                    CheckObjects = false;
            }
            
            if (CheckObjects)
            {
                MovingIntoGate();
                yield break;
            }

            yield return new WaitForSeconds(0.5f);            
        }
        
    }

    public void OptionButtonClick()
    {

    }

    public void MovingIntoGate()
    {
        SubCam.SetActive(true); //SubCamera On
    }

    public void GoNextScene()
    {
        Application.LoadLevel(1);
    }
}
