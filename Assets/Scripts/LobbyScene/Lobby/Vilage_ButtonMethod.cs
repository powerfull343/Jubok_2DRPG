using UnityEngine;
using System.Collections;
using System;
using LobbyButtonFunc;

public class Vilage_ButtonMethod : LobbyButton {

    [SerializeField]
    private Animation ArrowAni;
    private UIButton m_ButtonComp;
    [SerializeField]
    private IDSUBPANEL m_ConnectedFunc;

    void Start()
    {
        m_ButtonComp = this.gameObject.GetComponent<UIButton>();
        AddButtonFunc();
    }
   
    void Update()
    {
        if (ArrowAni != null)
        {
            if (UICamera.IsHighlighted(this.gameObject))
            {
                PlayAnimation();
            }
            else
            {
                StopAnimation();
            }
        }
    }

    private void AddButtonFunc()
    {
        EventDelegate AddVilageFunc = new EventDelegate(
            LobbyController.GetInstance(), "OpenVilagePanel");
        AddVilageFunc.parameters[0] = Mecro.MecroMethod.CreateEventParm(
            m_ConnectedFunc, typeof(LobbyButtonFunc.IDSUBPANEL));
        m_ButtonComp.onClick.Add(AddVilageFunc);
    }
    
    protected override void PlayAnimation()
    {
        if (!ArrowAni.isPlaying)
            ArrowAni.Play();
    }

    protected override void StopAnimation()
    {
        if (ArrowAni.isPlaying)
            ArrowAni.Stop();
    }

    public override void ClickFunction()
    {

    }
}
