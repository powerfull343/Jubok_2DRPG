using UnityEngine;
using System.Collections;

public class GameObject_Extension 
    : MonoBehaviour {

    public string Subtag;
    public int TagNum;
    [HideInInspector]
    public GameObject Own;
    
    /// <summary>
    /// Click Another Area to Hide Objects
    /// </summary>

    public bool ClickAnotherAreaHide;

    /// <summary>
    /// 1. ClickAnotherAreaHide == true
    /// 2. you must attach BoxCollider2D this GameObject
    /// </summary>

    [SerializeField]
    private BoxCollider2D colliderArea;

    /// <summary>
    /// What you want hiding Object?
    /// </summary>
    [SerializeField]
    private GameObject HideObject;

    /// <summary>
    /// if Last Monster is Die 
    /// Monster Moving Still Summoned
    /// </summary>
    [SerializeField]
    private bool m_isLastObjectMoving = false;

    [HideInInspector]
    public bool m_isScreenRate = false;


    void OnEnable()
    {
        if (ClickAnotherAreaHide)
            StartCoroutine("HideClickAnotherArea");
        if(m_isLastObjectMoving)
            StartCoroutine("DeadObjectMoving");
    }

    void Start()
    {
        Own = this.gameObject;
        //SetScreenRate();
    }

    public virtual void SelfActive()
    {
        this.gameObject.SetActive(true);
    }

    public void SelfHide()
    {
        this.gameObject.SetActive(false);
    }

    public void HideAndShow()
    {
        //Mecro.MecroMethod.ShowSceneLogConsole(gameObject.name + " : " +
        //     gameObject.activeSelf);

        if (!this.gameObject.activeSelf)
            SelfActive();
        else
            SelfHide();

        //Mecro.MecroMethod.ShowSceneLogConsole(gameObject.name + " : " +
        //     gameObject.activeSelf);
    }

    public virtual void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

    public void StartHidingClickAnotherArea()
    {
        ClickAnotherAreaHide = true;
        StartCoroutine("HideClickAnotherArea");
    }

    public void StopHidingClickAnotherArea()
    {
        ClickAnotherAreaHide = false;
        StopCoroutine("HideClickAnotherArea");
    }

    IEnumerator HideClickAnotherArea()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldpoint, Vector2.zero);

                if (hit != colliderArea)
                {
                    if (HideObject != null)
                    {
                        if (HideObject.activeSelf)
                            HideObject.SetActive(false);
                    }
                    else
                        SelfHide();

                    yield break;
                }
            }
            yield return null;
        }
    }

    IEnumerator DeadObjectMoving()
    {
        Vector3 vObjectSpeed;
        if (this.gameObject.GetComponent<UIWidget>())
        {
            vObjectSpeed = new Vector3(
                EnvironmentManager.OriginMovingSpeed * EnvironmentManager.SpeedAmount * 30f,
                0f, 0f);
        }
        else
        {
            vObjectSpeed = new Vector3(
                EnvironmentManager.OriginMovingSpeed * EnvironmentManager.SpeedAmount * 0.1f,
                0f, 0f);
        }

        while (true)
        {
            //if(MonsterManager.MonsterCount <= 0)
            //    transform.localPosition -= vObjectSpeed;
            if(EnvironmentManager.isMoved)
                transform.localPosition -= vObjectSpeed;

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    
    //private void SetScreenRate()
    //{
    //    UIRoot mRoot = gameObject.GetComponent<UIRoot>();

    //    float ratio = (float)mRoot.activeHeight / Screen.height;

    //    float width = Mathf.Ceil(Screen.width * ratio);
    //    float height = Mathf.Ceil(Screen.height * ratio);

    //    //Debug.Log("width : " + width);
    //    //Debug.LogError("height : " + height);
    //}
}
