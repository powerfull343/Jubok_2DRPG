using UnityEngine;
using System.Collections;
using Mecro;

public class OptionControll : 
    Singleton<OptionControll>
{
    [SerializeField]
    private AudioListener m_Listener;

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        MecroMethod.CheckExistComponent<AudioListener>(m_Listener);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
	
}
