using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : UIButtonBase
{
    public override void OnClick()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelSelect);
    }

    public override void OnEnter()
    {
    }
}
