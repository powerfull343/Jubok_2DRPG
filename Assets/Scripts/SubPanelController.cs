using UnityEngine;
using System.Collections;
using Mecro;

public class SubPanelController : MonoBehaviour {

    private Animator animator;

    void OnEnable()
    {
        Invoke("DisableAnimator", 1f);
    }

    void Start()
    {
        animator = MecroMethod.CheckGetComponent<Animator>(
            gameObject);
    }
    
    public void AppearThis()
    {
        this.gameObject.SetActive(true);
    }

    public void DisableAnimator()
    {
        if(animator.enabled != false)
            animator.enabled = false;
    }

    public void EnableAnimator()
    {
        if(animator.enabled != true)
            animator.enabled = true;
    }

    public void DisAppearThis()
    {
        this.gameObject.SetActive(false);
    }

    public void disappearWithAnimation()
    {
        EnableAnimator();
        animator.SetBool("Close", true);
    }

    public void DestroyThis()
    {
        Destroy(this);
    }

    public void EnableChildren()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

}
