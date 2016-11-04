using UnityEngine;
using System.Collections;
using Mecro;


public class BattleFieldManager
    : Singleton_Parent<BattleFieldManager> {

    //[SerializeField] - private 
    [SerializeField]
    private Camera UICameraObject;
    [SerializeField]
    private GameObject BattleFieldPanel;
    public GameObject GetBattleFieldPanel
    { get { return BattleFieldPanel; } }
    [SerializeField]
    private BattleFieldDetailScrollManager ScrollManager;

    [SerializeField]
    private GameObject DetailStat;

    //public 
    [SerializeField]
    private Transform[] SelectLevels;

    /// <summary>
    /// Use Focus BattleField icon
    /// </summary>

    public static Vector3 _SelectedPosition
        = new Vector3(0f, 0f, 0f);

    ///<summary>
    ///Animation Speed
    ///</summary>
    public float fAnimationMoveSpeed = 1f;

    /// <summary>
    /// Use Camera Effect variable
    /// </summary>

    private float OriginSize;

    /// <summary>
    /// Use Camera Effect variable
    /// </summary>
    /// 
    private float ConvertSize = 0.25f;

    
    void Start()
    {
        if (UICameraObject == null)
        {
            Debug.Log(UICameraObject.name + " object is null");
            return;
        }

        OriginSize = UICameraObject.orthographicSize;

        if (BattleFieldPanel == null)
        {
            Debug.Log(BattleFieldPanel.name + " object is null");
            return;
        }

        if (DetailStat == null)
        {
            Debug.Log(DetailStat.name + " obj is null");
            return;
        }

        if(ScrollManager == null)
        {
            Debug.Log(ScrollManager.name + "object is null");
            return;
        }

        //DetailStat's Child Component initalizing

        //DetailStat / Sprite - BackGround transform

        if (DetailStat.activeSelf)
            DetailStat.SetActive(false);

        Transform DetailBGtrans =
            DetailStat.transform.GetChild(0);
        // = Sprite - BackGround_0

        UISprite DetailBGSprite =
            MecroMethod.CheckGetComponent<UISprite>(DetailBGtrans);

        DetailBGSprite.fillAmount = 0f;
        //Initalizing End.

        int iArrCount = SelectLevels.Length;
        if (iArrCount == 0)
        {
            Debug.Log("Cannot find Transform object");
            return;
        }

        //Select Level Objects
        for (int i = 0; i < iArrCount; ++i)
        {
            SelectLevels[i] = SelectLevels[i].FindChild("Button - ClickEvent");
            if (SelectLevels[i] == null)
            {
                Debug.Log("Cannot find Child Transform Object");
                return;
            }
        }
    }


    //===========Static Methods=============//

    public static Transform[] GetLevels()
    {
        return GetInstance().SelectLevels;
    }

    /// <summary>
    /// Click EdgeScroll & Select Popup Box
    /// </summary>
    
    public static void MovePanelObjects(Vector3 MovePosition)    {

        float MoveSpeed = MovePosition == Vector3.zero ?
            (GetInstance().fAnimationMoveSpeed / 1.25f) :
            GetInstance().fAnimationMoveSpeed;

        TweenPosition.Begin(GetInstance().BattleFieldPanel,
            MoveSpeed, (MovePosition * -1f));
    }

    public static void ApplyCurrentSelectTransform(int SelectedArea)
    {
        BattleFieldDetailScrollManager.Currenticon =
           GetInstance().SelectLevels[SelectedArea].parent;
    }

    //===========Enable Detail Stat============//
    //0. Click Something Area use this method
    public void ClickArea(int SelectedArea)
    {
        //LobbyController.mSelectedSceneID = (FIELDID)SelectedArea;
        LobbyController.mSelectedSceneID = FIELDID.ID_BATTLEFIELD01;

        BattleFieldDetailScrollManager.GetInstance(
            ).InnerPopupButtonLabelSetting();

        _SelectedPosition = 
            SelectLevels[SelectedArea].parent.localPosition;

        ApplyCurrentSelectTransform(SelectedArea);

        //==Camera Setting Methods==//
        ControllOrthoSize(_SelectedPosition);
        //==========================//
        
        ChangeDepthtoSelectObejct(
            SelectLevels[SelectedArea].parent.gameObject, true);

        Invoke("EnableDetailObject", fAnimationMoveSpeed);
    }

    //1. change button & sprite image depth
    public static void ChangeDepthtoSelectObejct(GameObject parent, bool isUpper)
    {
        UIWidget[] childwidget =
            parent.GetComponentsInChildren<UIWidget>();

        foreach (UIWidget widget in childwidget)
        {
            if (widget.depth < 10 && isUpper)
                widget.depth += 10;
            else if (widget.depth > 10 && !isUpper)
                widget.depth -= 10;
        }
    }

    //2. DetailStat activate
    void EnableDetailObject()
    {
        if (!DetailStat.activeSelf)
            DetailStat.SetActive(true);

        SettingScrollPosition();

        StartCoroutine("ChangeScrollFillAmount", true);
    }

    //2 - 1. DetailStat BG Setting Position
    void SettingScrollPosition()
    {
        //Setting Offset Position
        DetailStat.transform.localPosition =
            _SelectedPosition + new Vector3(0f, 30f, 0f);
    }

    //===========Disable Detail Stat============//
    //0. Click Previous button
    public void DisableDetailScroll()
    {
        ScrollManager.ControllChildObjects(false);
        ScrollManager.ControllBehindCollider(false);

        ChangeDepthtoSelectObejct(
            SelectLevels[(int)LobbyController.mSelectedSceneID
            ].parent.gameObject, false);

        StartCoroutine("ChangeScrollFillAmount", false);

        Invoke("DisableScrollObject", fAnimationMoveSpeed);

        ControllOrthoSize(Vector3.zero);
    }

    //2. Disable Scorll Object
    void DisableScrollObject()
    {
        if (DetailStat.activeSelf)
            DetailStat.SetActive(false);
    }

    //==========Use Both Method=========//

    //if You Enable
    //2 - 2. DetailStat BG Fill it
    //if You Disable
    //1. DetailStat BG Hide it
    IEnumerator ChangeScrollFillAmount(bool isHigher)
    {
        //(BattleField - AreaDetailStat\Sprite - BackGround) object numbering//0
        Transform scrolltrans =
            DetailStat.transform.GetChild(0);

        UISprite scrollsprite =
            MecroMethod.CheckGetComponent<UISprite>(scrolltrans.gameObject);

        //isHiger = isShow = true;
        if(isHigher)
            ScrollManager.ControllBehindCollider(isHigher);

        float Changedir = isHigher ? 1f : -1f;
        float ChangeAmount = 0.05f;

        do
        {
            scrollsprite.fillAmount += ChangeAmount * Changedir;
            yield return new WaitForSeconds(0.01f);
        }
        while (scrollsprite.fillAmount > 0 &&
            scrollsprite.fillAmount < 1);

        //End BG Fill Show Child Objects
        if (isHigher)
            ScrollManager.ControllChildObjects(isHigher);

        yield break;
    }

    //if You Show Scorll
    //0 - 1. Camera Orthograpic Setting
    //if You Hide Scroll
    //3. Camera Orthograpic Setting
    void ControllOrthoSize(Vector3 MovePosition)
    {
        //Panel Movement
        //TweenPosition.Begin(BattleFieldPanel,
        //    1f, (MovePosition * -1f));
        MovePanelObjects(MovePosition);

        //Lower Camera size value 
        TweenOrthoSize.Begin(
            UICameraObject.gameObject,
            fAnimationMoveSpeed,
            MovePosition != Vector3.zero ?
            ConvertSize : OriginSize);
    }

    //========SelectLevel=========//
    public void SetSelectLevel(int Level)
    {
        LobbyController.mSelectedSceneID = (FIELDID)Level;
    }
}
