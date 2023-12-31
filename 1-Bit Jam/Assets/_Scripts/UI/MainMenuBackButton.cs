using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackButton : UIButtonBase
{
    public static event Action OnBack;

    public override void OnClick()
    {
        base.OnClick();

        OnBack?.Invoke();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
