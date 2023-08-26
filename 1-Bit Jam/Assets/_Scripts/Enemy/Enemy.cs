using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//This, Button, and Enemy clearly share some repeating qualities.
//TO DO: Make changes to reduce repeated code.
public class Enemy : ColoredObject
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public SpriteData DarkSpriteData { get; private set; }
    [field: SerializeField] public SpriteData LightSpriteData { get; private set; }

    public SpriteData SpriteData { get; private set; }

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;
    ColoredAnimator enemyAnimator;
    Animator animator;

    public bool IsActiveProperty { get; private set; }

    bool playPhaseAnimation;
    
    void Awake()
    {
        SetSpriteData();
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
        enemyAnimator = GetComponent<ColoredAnimator>();
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = SpriteData.Controller;
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
            enemyAnimator.ShouldPlayPhaseIn(true, IsActiveProperty);
            playPhaseAnimation = false;
        }
        else
        {
            enemyAnimator.ShouldPlayIdle(IsActiveProperty);
            enemyAnimator.ShouldPlayInactive(IsActiveProperty);
        }
    }

    public override void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        Sprite newSprite;

        DebugHelper.ShouldLog($"Set enemy sprite active : {IsActiveProperty}", showDebug);

        if (IsActiveProperty)
        {
            newSprite = SpriteData.ActiveSprite;
            spriteRenderer.sortingOrder = 1;
        }
        else
        {
            newSprite = SpriteData.InactiveSprite;
            spriteRenderer.sortingOrder = 0;
        }

        spriteRenderer.sprite = newSprite;

        playPhaseAnimation = true;
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (Color == ColorSwap.Color.Neutral)
            return true;

        return Color == backgroundColor;
    }
}
