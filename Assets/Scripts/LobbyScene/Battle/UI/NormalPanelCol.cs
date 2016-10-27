using UnityEngine;
using System.Collections;

public class NormalPanelCol : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("out");
        if (other.gameObject.tag == "BackGround")
        {
            Debug.Log("Background out");
            Vector3 pos = Camera.main.WorldToViewportPoint(other.transform.position);

            if (pos.x <= 0f)
                pos.x = 1f;

            other.transform.position = Camera.main.ViewportToWorldPoint(pos);
            
        }
    }
}
