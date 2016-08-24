using UnityEngine;
using System.Collections;

public class TestLabel : MonoBehaviour {

    private Moveable_Object PlayerCtrl;
    private UILabel labelComp;

	// Use this for initialization
	void Start () {
        PlayerCtrl = PlayerCtrlManager.GetInstance().PlayerCtrl;
        labelComp = Mecro.MecroMethod.CheckGetComponent<UILabel>(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        labelComp.text = PlayerCtrl.Hp.ToString() + " / " + PlayerCtrl.MaxHp.ToString();

    }
}
