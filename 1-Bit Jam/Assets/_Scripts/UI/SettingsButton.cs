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

    void Update()
    {
        if (Input.GetButtonDown("Settings") && CanBeClicked())
        {
            OnClick();
        }
    }

    public override bool CanBeClicked()
    {
        return GameScreensManager.Instance.GameSettingsBarCanvas.gameObject.activeInHierarchy && gameObject.activeInHierarchy;
    }
}
