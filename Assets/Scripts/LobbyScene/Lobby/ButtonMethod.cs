using UnityEngine;
using System.Collections;
using System;
using LobbyButtonFunc;


public class ButtonMethod : LobbyButton {

    [SerializeField]
    private Animation ArrowAni;
   
   
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
