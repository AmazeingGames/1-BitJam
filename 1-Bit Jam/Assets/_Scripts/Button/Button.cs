using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// This and Enemy clearly share some repeating qualities.
// TO DO: Make changes to reduce repeated code.
public class Button : ColoredObject
{
    [SerializeField] bool showDebug;

    void Awake()
    {
        SetSpriteData();

        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = SpriteData.Controller;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        CheckAnimations();
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        base.HandleColorSwap(newColor);
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
        => Color == ColorSwap.Color.Neutral || backgroundColor != Color;
}
