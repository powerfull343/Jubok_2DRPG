using UnityEngine;
using System.Collections;
using Mecro;

public class MissionItemController : MonoBehaviour
{

    [SerializeField]
    private UISprite MissionIconBackGround;
    [SerializeField]
    private UISprite MissionIcon;
    [SerializeField]
    private UILabel MissionLabel;
    [SerializeField]
    private GameObject_Extension OwnProfile;
    [SerializeField]
    private UIButton SelectButton;

    void Start()
    {
        MecroMethod.CheckExistComponent<UISprite>(MissionIconBackGround);
        MecroMethod.CheckExistComponent<UISprite>(MissionIcon);
        MecroMethod.CheckExistComponent<UILabel>(MissionLabel);
        OwnProfile = MecroMethod.CheckGetComponent<GameObject_Extension>(this.gameObject);
        MecroMethod.CheckExistComponent<UIButton>(SelectButton);
    }

    public void InitMissionBox(string atlasName,
        string atlasSpriteName, int FieldNum, string PanelContants)
    {
        string LoadPath = "StructureIconAtlas/" + atlasName;

        MissionIcon.atlas =
            Resources.Load(LoadPath, typeof(UIAtlas)) as UIAtlas;
        MissionIcon.spriteName = atlasSpriteName;

        //SelectLevel - (index : 14)
        string Labelstring =
            PanelContants.Substring(14);

        MissionLabel.text = Labelstring;

        OwnProfile.TagNum = FieldNum;
    }

    void Update()
    {
        ClickIconButton();
    }

    private void ClickIconButton()
    {
        if (UICamera.IsHighlighted(SelectButton.gameObject))
        {
            if (Input.GetMouseButtonDown(0))
                BattleFieldDetailScrollManager.GetInstance().ClickPopupIcon(OwnProfile.TagNum);
        }
    }


}
