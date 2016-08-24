using UnityEngine;
using System.Collections;

public class ScrollObjectCtrl : MonoBehaviour {

    public PlayerStatWindowCtrl Manager;

    void Start()
    {
        if(Manager == null)
        {
            Debug.Log(Manager.name + "is null");
            return;
        }
    }

	public void EndOpenAnimation()
    {
        Manager.OpenSubPanel();
    }
}
