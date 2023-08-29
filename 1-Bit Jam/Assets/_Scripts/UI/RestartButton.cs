using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : UIButtonBase
{
    public static event Action OnRestart;

    public override void OnClick()
    {
        base.OnClick();

        Debug.Log("Restart");

        OnRestart?.Invoke();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

}
