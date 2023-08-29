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

    void Update()
    {
        if (Input.GetButtonDown("Restart") && CanBeClicked())
        {
            OnClick();
        }
    }

    public override bool CanBeClicked()
    {
        return ((GameScreensManager.Instance.GameSettingsBarCanvas.gameObject.activeInHierarchy && gameObject.activeInHierarchy) || GameScreensManager.Instance.GameOver.enabled && gameObject.activeInHierarchy);
    }
}
