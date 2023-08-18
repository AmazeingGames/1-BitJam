using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject interactIcon;
    [SerializeField] float interactionCooldownLength;

    [SerializeField] float interactBuffer;
    [SerializeField] float canInteractTime;

    Vision vison;
    Button button;

    float lastPressedInteract;
    float lastCouldInteract;

    public bool PlayInteraction { get; private set; }
    public bool IsOnCooldown { get; private set; }

    float cooldownTimer;

    private void Start()
    {
        vison = GetComponent<Vision>();
        button = GetComponent<Button>();

        interactIcon.SetActive(false);
    }

    void Update()
    {
        UpdateTimers();

        GetInput();

        CanBeInteractedWith(vison.CanSeeCollider(Player.Instance.Collider));
    }

    void UpdateTimers()
    {
        lastPressedInteract -= Time.deltaTime;
        lastCouldInteract -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;
    }

    void GetInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            lastPressedInteract = interactBuffer;
        }
    }

    public void Initialize(ColorSwap.Color color)
    {
    }

    //If player is within range
    void CanBeInteractedWith(bool inRange)
    {
        interactIcon.SetActive(false);
        lastCouldInteract = -1;

        if (IsOnCooldown)
            return;

        if (!inRange)
            return;

        if (!button.IsActiveProperty)
            return;
        interactIcon.SetActive(true);
        lastCouldInteract = canInteractTime;

        ShouldPlayInteraction();
    }

    //If player presses interact
    void ShouldPlayInteraction()
    {
        if (lastCouldInteract < 0)
            return;

        if (lastPressedInteract < 0)
            return;

        Interaction();
    }

    //On interaction
    void Interaction()
    {
        cooldownTimer = interactionCooldownLength;

        ColorSwap.Instance.ChangeColor(ColorSwap.Instance.OppositeColor(), gameObject);
    }
}
