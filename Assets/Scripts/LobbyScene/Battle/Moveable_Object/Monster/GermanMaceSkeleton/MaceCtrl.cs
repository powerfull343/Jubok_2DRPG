using UnityEngine;
using System.Collections;
using Mecro;

public class MaceCtrl : MonoBehaviour {

    private Monster_Interface MonsterBody;

    void Start()
    {
        MonsterBody
            = MecroMethod.CheckGetComponent<Monster_Interface>(
                transform.parent.parent.FindChild("MonsterBody"));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerCtrlManager.GetInstance().PlayerCtrl.SetHp(MonsterBody.Atk);
        }
    }
}
