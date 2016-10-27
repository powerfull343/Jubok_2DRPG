using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour {

	public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
