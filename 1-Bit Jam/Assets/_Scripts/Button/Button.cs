using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//This, Button, and Enemy clearly share some repeating qualities.
//TO DO: Make changes to reduce repeated code.
public class Button : MonoBehaviour, IColored
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public ButtonData DarkButtonData { get; private set; }
    [field: SerializeField] public ButtonData LightButtonData { get; private set; }

    [SerializeField] bool showDebug;

    ColoredAnimator buttonAnimator;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public ButtonData ButtonData { get; private set; }
    public SpriteData SpriteData { get => ButtonData.SpriteData; }

    public bool IsActiveProperty { get; private set; }

    bool playPhaseAnimation;

    void Awake()
    {
        ButtonData = Color switch
        {
            ColorSwap.Color.White => LightButtonData,
            ColorSwap.Color.Black => DarkButtonData,
            ColorSwap.Color.Neutral => throw new NotImplementedException(),
            _ => throw new Exception(),
        };

        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = SpriteData.Controller;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        buttonAnimator = GetComponent<ColoredAnimator>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsLevelPlaying)
            return;

        CheckAnimations();
    }

    void CheckAnimations()
    {
        if (playPhaseAnimation)
        {
            buttonAnimator.ShouldPlayPhaseIn(true, IsActiveProperty);
            playPhaseAnimation = false;
        }
        else
        {
            buttonAnimator.ShouldPlayIdle(IsActiveProperty);
            buttonAnimator.ShouldPlayInactive(IsActiveProperty);
        }
    }

    private void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    private void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    void SubscribeToColorSwap(bool isSubscribing)
    {
        if (ColorSwap.Instance == null)
        {
            Debug.LogWarning("ColorSwap.Instance is null");
            return;
        }

        if (isSubscribing)
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
        else
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
    }


    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        Sprite newSprite;

        if (IsActiveProperty)
        {
            newSprite = SpriteData.ActiveSprite;
        }
        else
        {
            newSprite = SpriteData.InactiveSprite;
        }


        spriteRenderer.sprite = newSprite;

        playPhaseAnimation = true;
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (Color == ColorSwap.Color.Neutral)
            return true;

        return (backgroundColor != Color);
    }
}
