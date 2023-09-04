using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : ColoredObject
{
    [SerializeField] new BoxCollider2D collider;
    public ColorSwap.Color CurrentState { get; private set; }

    private void Awake()
    {
        SetSpriteData();
    }

    void Start()
    {
        base.OnStart();
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        CurrentState = newColor;

        SetSpriteData();

        spriteRenderer.sprite = SpriteData.ActiveSprite;

        playPhaseAnimation = true;
    }

    protected override void SetSpriteData()
    {
        SpriteData = CurrentState switch
        {
            ColorSwap.Color.White => LightSpriteData,
            ColorSwap.Color.Black => DarkSpriteData,
            _ => throw new NotImplementedException(),
        };
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor) => true;
}
