using UnityEngine;
using System.Collections;
using Mecro;

//모든 전투 클래스의 공통으로 컨트롤을 담당하는 역할을 수행한다.
public class PlayerCtrlManager 
    : Singleton<PlayerCtrlManager>
{ 
    [SerializeField]
    private Transform _Player;
    public Transform Player
    {
        get { return _Player; }
        set { _Player = value; }
    }

    private Moveable_Object _PlayerCtrl;
    public Moveable_Object PlayerCtrl
    {
        get { return _PlayerCtrl; }
        set { _PlayerCtrl = value; }
    }
    private Animator _PlayerAnim;
    public Animator PlayerAnim
    {
        get { return _PlayerAnim; }
        set { _PlayerAnim = value; }
    }

    void Awake()
    {
        CreateInstance();
        MecroMethod.CheckExistObject<Transform>(_Player);
        _PlayerCtrl = MecroMethod.CheckGetComponent<Moveable_Object>(_Player);
        _PlayerAnim = MecroMethod.CheckGetComponent<Animator>(_Player);

    }

    void Update()
    {
        PlayerCtrl.AutoAction();

        if (!PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            EnvironmentManager.isMoved = false;
        else
            EnvironmentManager.isMoved = true;
    }

    public void PlayerIdleAction()
    {
        PlayerAnim.ResetTrigger("Move");
        PlayerAnim.SetTrigger("Idle");
    }
	
    public void ClearBattleScenePlayerData()
    {
        _PlayerCtrl.ClearAllData();
    }
}
