using UnityEngine;
using System.Collections;

public class SkeletonBomber_Bomb : MonoBehaviour {

    private Animator mAnim;

	void Start () {
        mAnim = Mecro.MecroMethod.CheckGetComponent<Animator>(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            mAnim.SetTrigger("Exploding");
        }
    }
}
    