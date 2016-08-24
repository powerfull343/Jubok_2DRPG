using UnityEngine;
using System.Collections;

public class ExecptBoxCollider : MonoBehaviour {

    private BoxCollider BoxCol;

    void Start() {
        BoxCol = GetComponent<BoxCollider>();
    }

    public void ExecptCollider() //Use To Cannot Button Click
    {   
        if(BoxCol == null)
        {
            Debug.Log("No Box Collider");
            return;
        }

        BoxCol.enabled = false;
    }
}
