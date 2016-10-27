using UnityEngine;
using System.Collections;


public class UI_WorldTime : MonoBehaviour {

    [SerializeField]
    private UILabel TimeText;

    void Start()
    {
        Mecro.MecroMethod.CheckExistComponent<UILabel>(TimeText);
    }

    void Update()
    {
        TimeText.text = System.DateTime.Now.ToString("HH : mm : ss");
        
    }

}
