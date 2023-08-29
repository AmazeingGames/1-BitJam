using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExitToMenuButton : UIButtonBase
{
    public static event Action OnExit;

    public override void OnClick()
    {
        base.OnClick();

        OnExit?.Invoke();

        Debug.Log("Exit to Menu");
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
