using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : UIButtonBase
{
    public static event Action OnSettings;

    public override void OnClick()
    {
        base.OnClick();

        OnSettings.Invoke();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
