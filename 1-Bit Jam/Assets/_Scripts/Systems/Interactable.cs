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
    [SerializeField] Vision vison;

    [Header("Properties")]
    [SerializeField] float interactionCooldownLength;
    [SerializeField] float interactBuffer;
    [SerializeField] float canInteractTime;


    float lastPressedInteract;
    float lastCouldInteract;

    public bool PlayInteraction { get; private set; }
    public bool IsOnCooldown { get; private set; }

    protected List<bool> extraCaseGuards = new(); 

    float cooldownTimer;

    protected AudioManager.EventSounds interactSound = AudioManager.EventSounds.Null;

    protected void Start()
    {
        SetInteractIconActive(false);
        SetInteractSound();
    }

    protected abstract void SetInteractSound();

    protected void Update()
    {
        if (!GameManager.IsGameRunning)
            return;

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
            lastPressedInteract = interactBuffer;
    }

    protected virtual void FillExtraCaseguards()
        => extraCaseGuards.Clear();

    // This could could probably be improved via a state machine
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

    protected virtual void Interaction()
        => cooldownTimer = interactionCooldownLength;
}
