using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : Interactable
{
    [SerializeField] Button button;

    protected override void SetInteractSound()
        => interactSound = AudioManager.EventSounds.Null;

    protected override void Interaction()
    {
        base.Interaction();

        ColorSwap.Instance.ChangeColor(ColorSwap.Instance.OppositeColor(), gameObject, triggerSwapSounds: true);
    }

    protected override void FillExtraCaseguards()
    {
        base.FillExtraCaseguards();

        extraCaseGuards.Add(!button.IsActiveProperty);
    }
}
