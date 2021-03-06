﻿using UnityEngine;
using System.Collections;
using Mecro;

public class BattleFieldDetailScrollManager
    : Singleton_Parent<BattleFieldDetailScrollManager>
{

    [SerializeField]
    private GameObject[] ScrollEdgeCollider;
    [SerializeField]
    private GameObject PreviousButton;
    [SerializeField]
    private GameObject BehindCollider;
    [SerializeField]
    private GameObject SelectLevelPopupList;
    [SerializeField]
    private GameObject StartLevelButton;
    [SerializeField]
    private GameObject_Extension PopupPanel;
    [SerializeField]
    private UILabel innerPopupButtonLabel;

    /// <summary>
    /// Selected Map Icon Parent Transform
    /// </summary>

    public static Transform Currenticon { get; set; }

    /// <summary>
    /// use add popup box items
    /// </summary>

    public Transform GridTrans;

    void Start()
    {
        InitPopupBoxList();

        int arrCount = ScrollEdgeCollider.Length;

        if (arrCount == 0)
        {
            Debug.Log("ScrollEdgeCollider not have object");
            return;
        }

        foreach (GameObject EdgeCol in ScrollEdgeCollider)
            MecroMethod.CheckExistObejct<GameObject>(EdgeCol);
        
        MecroMethod.CheckExistObejct<GameObject>(PreviousButton);
        MecroMethod.CheckExistObejct<GameObject>(BehindCollider);
        MecroMethod.CheckExistObejct<GameObject>(SelectLevelPopupList);
        MecroMethod.CheckExistObejct<GameObject>(StartLevelButton);
        MecroMethod.CheckExistComponent<GameObject_Extension>(PopupPanel);
        MecroMethod.CheckExistComponent<UILabel>(innerPopupButtonLabel);

        MecroMethod.CheckExistObejct<Transform>(GridTrans);
    }

    public void ControllChildObjects(bool isShow)
    {
        foreach (GameObject EdgeCol in ScrollEdgeCollider)
        {
            if (EdgeCol.activeSelf != isShow)
                EdgeCol.SetActive(isShow);
        }

        if (PreviousButton.activeSelf != isShow)
            PreviousButton.SetActive(isShow);

        if (SelectLevelPopupList.activeSelf != isShow)
            SelectLevelPopupList.SetActive(isShow);

        if (StartLevelButton.activeSelf != isShow)
            StartLevelButton.SetActive(isShow);
    }

    public void ControllBehindCollider(bool isShow)
    {
        if (BehindCollider.activeSelf != isShow)
            BehindCollider.SetActive(isShow);
    }

    public void ClickScrollEdge(int EdgeId)
    {
        int SelectLevelidx = (int)BattleFieldManager.mSelectID;

        HidePreviousiconObjects(SelectLevelidx);

        SelectLevelidx = (EdgeId == 0) ?
            (SelectLevelidx - 1) :
            (SelectLevelidx + 1);

        if (EdgeId == 0 && SelectLevelidx < 0)
            SelectLevelidx = (int)FIELDID.ID_MAX - 1;

        if (EdgeId == 1 && SelectLevelidx >= (int)FIELDID.ID_MAX)
            SelectLevelidx = 0;

        BattleFieldManager.mSelectID = (FIELDID)SelectLevelidx;

        InnerPopupButtonLabelSetting();

        ChoiceSelectArea();
    }

    public void ClickPopupIcon(int LevelNum)
    {
        if (LevelNum == (int)BattleFieldManager.mSelectID)
            return;

        //1. Hide Scroll icons
        HidePreviousiconObjects((int)BattleFieldManager.mSelectID);

        //2. Apply SelectID
        BattleFieldManager.mSelectID = (FIELDID)LevelNum;

        //3. Box Label Setting
        InnerPopupButtonLabelSetting();

        //4. Move Panel
        ChoiceSelectArea();

        //5. Hide PopupBox
        PopupPanel.SelfHide();
    }

    //====== Select Move Area ======//
    void HidePreviousiconObjects(int SelectedArea)
    {
        BattleFieldManager.ChangeDepthtoSelectObejct(
            Currenticon.gameObject,
            false);
    }

    void ChoiceSelectArea()
    {
        SetMovedPosition();
        //Move Panel & Scroll
        BattleFieldManager.MovePanelObjects(Currenticon.localPosition);
        MoveScrollObjects();

        //Move end - Show icon Object
        Invoke("ShowNexticonObjects",
            (BattleFieldManager.GetInstance().fAnimationMoveSpeed + 0.05f));
    }

    //3-1
    void SetMovedPosition()
    {
        Currenticon = BattleFieldManager.GetLevels()[(int)BattleFieldManager.mSelectID].parent;

        BattleFieldManager._SelectedPosition =
            Currenticon.localPosition;
    }

    //3-2
    void MoveScrollObjects()
    {
        Vector3 MovePosition = BattleFieldManager._SelectedPosition;

        //Setting Offset Position
        MovePosition += new Vector3(0f, 30f, 0f);

        TweenPosition.Begin(this.gameObject,
            BattleFieldManager.GetInstance().fAnimationMoveSpeed,
            (MovePosition));
    }

    //3-3
    void ShowNexticonObjects()
    {
        BattleFieldManager.ChangeDepthtoSelectObejct(
            Currenticon.gameObject,
            true);
    }

    //===============================//

    //====== Popup Box Methods ======//

    void InitPopupBoxList()
    {
        int havelevelcount = BattleFieldManager.GetLevels().Length;

        UIGrid grid = GridTrans.GetComponent<UIGrid>();

        for (int i = 0; i < havelevelcount; ++i)
        {
            GameObject CreatedObject = SettingPopupBox(i);

            //grid.AddChild(CreatedObject.transform);
            NGUITools.AddChild(GridTrans.gameObject, CreatedObject);
        }

        grid.Reposition();
        grid.transform.parent.GetComponent<UIPanel>().Refresh();

    }

    GameObject SettingPopupBox(int levelidx)
    {
        //Create GameObject
        GameObject oneBox = Resources.Load("Mission/MissionSelectPrefab") as GameObject;
        MecroMethod.CheckExistObejct<GameObject>(oneBox);

        Transform SelectedLevelTrans = 
            BattleFieldManager.GetLevels()[levelidx].parent;

        oneBox.name = SelectedLevelTrans.name;

        //Init GameObject
        UISprite sprite = SelectedLevelTrans.GetComponentInChildren<UISprite>();

        MissionItemController item
            = MecroMethod.CheckGetComponent<MissionItemController>(oneBox);

        item.InitMissionBox(sprite.atlas.name,
            sprite.spriteName, levelidx, SelectedLevelTrans.gameObject.name);

        return oneBox;
    }

    //====Popup Label Setting====

    public void InnerPopupButtonLabelSetting()
    {
        //SelectLevels - (14)
        innerPopupButtonLabel.text =
            BattleFieldManager.GetLevels()[
            (int)BattleFieldManager.mSelectID].parent.name.Substring(14);
    }

    
}
