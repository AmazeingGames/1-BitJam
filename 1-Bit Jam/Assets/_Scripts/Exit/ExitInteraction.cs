using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteraction : Interactable
{
    Exit exit;

    // Start is called before the first frame update
    void Start()
    {
        exit = GetComponent<Exit>();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        base.Update();
    }

    protected override void GetInput()
    {
        base.GetInput();
    }

    protected override void FillExtraCaseguards()
    {
        base.FillExtraCaseguards();

        extraCaseGuards.Add(!exit.IsActiveProperty);
    }

    protected override void Interaction()
    {
        base.Interaction();

        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelFinish);

        Debug.LogWarning("Interacted with door");
        AudioManager.Instance.TriggerAudioClip(AudioManager.EventSounds.DoorEnter, gameObject);
    }

    protected override void SetInteractSound()
    {
        interactSound = AudioManager.EventSounds.DoorEnter;
    }

    protected override void SetVision()
    {
        vison = transform.GetChild(0).GetComponent<Vision>();
    }
}
