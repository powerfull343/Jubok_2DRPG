using UnityEngine;
using System.Collections;
using Mecro;

public class MeleeColScript : MonoBehaviour {

    private MagicianCtrl _Player;

    void Start()
    {
        _Player =
            MecroMethod.CheckGetComponent<MagicianCtrl>(transform.parent.FindChild("Magician"));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Monster"))
        {
            Monster_Interface MonsterInfo =
                MecroMethod.CheckGetComponent<Monster_Interface>(other.transform);

            if (!MonsterInfo.m_isReadyFight)
                return;

            _Player.ResetRangeAtkTrigger();

            if (MagicianCtrl.ColMonsters.Count >= 15)
                Debug.LogError("Warning");
            
            MagicianCtrl.ColMonsters.Add(MonsterInfo);

            //Debug.Log("AddCol");
            //Debug.Log("ColMonsters : " + MagicianCtrl.ColMonsters.Count);
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Monster"))
    //    {

    //    }
    //}

    //void OnTrigterExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Monster"))
    //    {
    //        Debug.Log("DelCol");
    //        MagicianCtrl.ColMonsters.Remove(
    //            MecroMethod.CheckGetComponent<Monster_Interface>(other.transform));
    //    }
    //}
}
