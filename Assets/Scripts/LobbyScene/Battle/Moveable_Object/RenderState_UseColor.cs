using UnityEngine;
using System.Collections;
using Mecro;

public class RenderState_UseColor : MonoBehaviour {

    private SpriteRenderer Renderer;
    public Color ParentColor = Color.white;

	void Start () {

        //Player
        if(this.gameObject.CompareTag("Player"))
            Renderer = MecroMethod.CheckGetComponent<SpriteRenderer>(transform.parent.FindChild("Magician"));
        else if (this.gameObject.CompareTag("Monster"))
            Renderer =
                MecroMethod.CheckGetComponent<SpriteRenderer>(transform.parent.FindChild("MonsterBody"));
	}

    void Update() {
        Renderer.material.color = ParentColor;
    }
	
}
