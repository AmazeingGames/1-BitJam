using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreditsButton : UIButtonBase
{
    public static event Action OnCredits;

    public override void OnClick()
    {
        base.OnClick();

        OnCredits?.Invoke();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
