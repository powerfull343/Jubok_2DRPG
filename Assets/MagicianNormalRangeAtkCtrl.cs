﻿using UnityEngine;
using System.Collections;

public class MagicianNormalRangeAtkCtrl : MonoBehaviour {

    private GameObject FireBall;

	// Use this for initialization
	void Start () {
        FireBall = Resources.Load("BattleScene/Skills/FireBall") as GameObject;
	}

    public void CallFireBall(int AtkPower)
    {
        GameObject NewFireBall = Instantiate(FireBall, transform.position, transform.rotation) as GameObject;

        NewFireBall.SetActive(false);

        NewFireBall.GetComponent<Bullet_Extension>().m_AtkPower = AtkPower;
        NewFireBall.transform.localScale = new Vector3(2f, 2f, 1f);
        NewFireBall.transform.parent = this.transform;

        NewFireBall.SetActive(true);

    }	
    
}
