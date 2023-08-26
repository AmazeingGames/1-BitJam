using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//This, Button, and Enemy clearly share some repeating qualities.
//TO DO: Make changes to reduce repeated code.
public class Button : ColoredObject
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public SpriteData DarkSpriteData { get; private set; }
    [field: SerializeField] public SpriteData LightSpriteData { get; private set; }

    [SerializeField] bool showDebug;

    ColoredAnimator buttonAnimator;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public SpriteData SpriteData { get; private set; }

    public bool IsActiveProperty { get; private set; }

    bool playPhaseAnimation;

    void Awake()
    {
        SetSpriteData();

        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = SpriteData.Controller;
    }

    void SetSpriteData()
    {
        SpriteData = Color switch
        {
            ColorSwap.Color.White => LightSpriteData,
            ColorSwap.Color.Black => DarkSpriteData,
            ColorSwap.Color.Neutral => throw new NotImplementedException(),
            _ => throw new Exception(),
        };
    }

    public SpriteData GetCurrentSpriteData()
    {
        if (SpriteData == null)
            SetSpriteData();

        return SpriteData;
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

    public override void HandleColorSwap(ColorSwap.Color newColor)
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

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (Color == ColorSwap.Color.Neutral)
            return true;

        return (backgroundColor != Color);
    }
}
