using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteraction : Interactable
{
    [SerializeField] Exit exit;

    protected override void FillExtraCaseguards()
    {
        // Do we need to clear the list?
        base.FillExtraCaseguards();

        extraCaseGuards.Add(!exit.IsActiveProperty);
    }

    protected override void Interaction()
    {
        base.Interaction();

        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelFinish);
        AudioManager.Instance.TriggerAudioClip(AudioManager.EventSounds.DoorEnter, gameObject);
    }

    protected override void SetInteractSound()
    {
        interactSound = AudioManager.EventSounds.DoorEnter;
    }
}
