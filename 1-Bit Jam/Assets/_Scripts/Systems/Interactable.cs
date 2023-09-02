using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] GameObject interactIcon;
    [SerializeField] float interactionCooldownLength;

    [SerializeField] float interactBuffer;
    [SerializeField] float canInteractTime;

    protected Vision vison;

    float lastPressedInteract;
    float lastCouldInteract;

    public bool PlayInteraction { get; private set; }
    public bool IsOnCooldown { get; private set; }

    protected List<bool> extraCaseGuards = new(); 

    float cooldownTimer;

    protected AudioManager.EventSounds interactSound = AudioManager.EventSounds.Null;

    protected void Start()
    {
        SetVision();
            
        SetInteractIconActive(false);

        SetInteractSound();
    }

    protected abstract void SetInteractSound();

    protected virtual void SetVision()
    {
        vison = GetComponent<Vision>();
    }

    protected void Update()
    {
        UpdateTimers();

        GetInput();

        CanBeInteractedWith();
        ShouldPlayInteraction();
    }

    protected void UpdateTimers()
    {
        lastPressedInteract -= Time.deltaTime;
        lastCouldInteract -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void GetInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            lastPressedInteract = interactBuffer;
        }
    }

    protected virtual void FillExtraCaseguards()
    {
        extraCaseGuards.Clear();
    }

    //This could could probably be improved via a state machine. This would also help with optimization
    //While the player is within range, the button can be interacted with
    //The button can still be interacted with for a short time after the player leaves the range
    protected void CanBeInteractedWith()
    {
        SetInteractIconActive(false);

        if (cooldownTimer > 0)
            return;

        bool canSeePlayer = vison.CanSeeCollider(Player.Instance.Collider);

        if (!canSeePlayer)
            return;

        FillExtraCaseguards();

        foreach (bool caseGaurd in extraCaseGuards)
            if (caseGaurd)
                return;

        SetInteractIconActive(true);
        lastCouldInteract = canInteractTime;
    }

    protected void SetInteractIconActive(bool setActive)
    {
        if (interactIcon != null)
            interactIcon.SetActive(setActive);
    }


    //If player presses interact
    protected void ShouldPlayInteraction()
    {
        if (cooldownTimer > 0)
            return;

        if (lastCouldInteract < 0)
            return;

        if (lastPressedInteract < 0)
            return;

        Interaction();
    }

    //On interaction
    protected virtual void Interaction()
    {
        cooldownTimer = interactionCooldownLength;

        if (interactSound != AudioManager.EventSounds.Null)
        {
            Debug.Log("Played Interact Trigger");
            AudioManager.Instance.TriggerAudioClip(interactSound, gameObject);
        }
    }
}
