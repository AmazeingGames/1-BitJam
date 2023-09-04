using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : Interactable
{
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        base.Start();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        base.Update();
    }

    protected override void SetInteractSound()
    {
        interactSound = AudioManager.EventSounds.Null;
    }

    protected override void GetInput()
    {
        base.GetInput();
    }

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
