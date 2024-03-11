using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColoredObject : Colored
{
    [field: SerializeField] public ColorSwap.Color Color { get; protected set; }
    [field: SerializeField] public SpriteData DarkSpriteData { get; protected set; }
    [field: SerializeField] public SpriteData LightSpriteData { get; protected set; }

    public bool IsActiveProperty { get; protected set; }

    protected bool playPhaseAnimation;
    
    protected Animator animator;
    protected ColoredAnimator coloredAnimator;
    protected SpriteRenderer spriteRenderer;

    SpriteData spriteData;
    public SpriteData SpriteData
    {
        get
        {
            SetSpriteData();
            return spriteData;
        }
        protected set
        {
            spriteData = value;
        }
    }


    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coloredAnimator = GetComponent<ColoredAnimator>();
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = SpriteData.Controller;
    }

    protected virtual void SetSpriteData()
    {
        SpriteData = Color switch
        {
            ColorSwap.Color.White => LightSpriteData,
            ColorSwap.Color.Black => DarkSpriteData,
            _ => throw new Exception(),
        };
    }

    protected void CheckAnimations()
    {
        if (coloredAnimator == null)
            throw new NullReferenceException("Colored Animator null");

        if (playPhaseAnimation)
        {
            coloredAnimator.ShouldPlayPhaseIn(true, IsActiveProperty);
            playPhaseAnimation = false;
        }
        else
        {
            coloredAnimator.ShouldPlayIdle(IsActiveProperty);
            coloredAnimator.ShouldPlayInactive(IsActiveProperty);
        }
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        Sprite newSprite;

        if (IsActiveProperty)
            newSprite = SpriteData.ActiveSprite;
        else
            newSprite = SpriteData.InactiveSprite;

        spriteRenderer.sprite = newSprite;

        playPhaseAnimation = true;
    }
}
