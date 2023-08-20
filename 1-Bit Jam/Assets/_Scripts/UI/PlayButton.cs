using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : UIButtonBase
{
    public override void OnClick()
    {
        base.OnClick();

        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelSelectMenu);
    }

    public override void OnEnter()
    {

    }
}
